using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VTools.Grid;

[CreateAssetMenu(menuName = "Procedural Generation Method/Noise")]
public class Noise : ProceduralGenerationMethod
{
    [Header ("Noise Parameters")]
    [SerializeField, Range(0.01f, 0.2f), Tooltip("Noise frequency")] private float _frequency = 0.1f;
    [SerializeField, Range(0, 100), Tooltip("Grass proportion")] private int _noiseDensity = 60;

    [Header ("Fractal Parameters")]
    [SerializeField, Range(1, 5), Tooltip("Noise fractal Octave")] private int _fracOctave = 3;
    [SerializeField, Range(0, 20), Tooltip("Noise fractal Lacunarity")] private int _fracLacunarity = 3;
    [SerializeField, Range(0, 3), Tooltip("Noise fractal Gain")] private int _fracGain = 3;
    [SerializeField, Range(-3, 6), Tooltip("Noise fractal WeightedStrength")] private int _fracWeightedStrength = 3;
    [SerializeField, Range(0, 6), Tooltip("Noise fractal WeightedStrength")] private float _amplitude = 0.5f;

    [Header ("Height Parameters")]
    [SerializeField, Range(0.0f, 1.0f), Tooltip("Noise fractal WeightedStrength")] private float _grassQuantity = 0.3f;
    [SerializeField, Range(0.0f, 1.0f), Tooltip("Noise fractal WeightedStrength")] private float _waterQuantity = 0.3f;
    [SerializeField, Range(-1000.0f, 1000.0f), Tooltip("Noise fractal WeightedStrength")] private float _grassHeight = 0.5f;
    [SerializeField, Range(-1000.0f, 1000.0f), Tooltip("Noise fractal WeightedStrength")] private float _waterHeight = 0.5f;
    [SerializeField, Range(-1000.0f, 1000.0f), Tooltip("Noise fractal WeightedStrength")] private float _sandHeight = 0.5f;

    [SerializeField, Tooltip("Noise type")] private FastNoiseLite.NoiseType _noiseType = FastNoiseLite.NoiseType.OpenSimplex2S;
    [SerializeField, Tooltip("Noise rotation type")] private FastNoiseLite.RotationType3D _rotationType = FastNoiseLite.RotationType3D.ImproveXZPlanes;
    [SerializeField, Tooltip("Noise cellular distance function")] private FastNoiseLite.CellularDistanceFunction _cellularDistance = FastNoiseLite.CellularDistanceFunction.Euclidean;
    [SerializeField, Range(0.0f, 6.0f), Tooltip("Noise rotation type")] private float _cellJitter = 2.0f;
    [SerializeField, Range(0.0f, 6.0f), Tooltip("Noise rotation type")] private float _pingpongStrength = 2.0f;

    private FastNoiseLite noiseLite = FastNoiseLite.Instance;
    private Dictionary<Vector2Int, string> cellsByTileName = new Dictionary<Vector2Int, string>();
    float[,] noiseData;
    protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
    {
        noiseData = new float[Grid.Width, Grid.Lenght];

        InitNoise();

        for (int i = 0; i < _maxSteps; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            cellsByTileName.Clear();

            for (int x = 0; x < Grid.Width; x++)
            {
                for (int y = 0; y < Grid.Lenght; y++)
                {
                    noiseData[x, y] = GetNoiseData(noiseLite, x, y);
                    CheckCell(Grid.Cells[x * Grid.Width + y], noiseData[x, y]);
                }
            }

            foreach (var cell in cellsByTileName)
                MakeMap(cell);

            await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
        }
    }
    private void InitNoise()
    {
        noiseLite = new FastNoiseLite();
        noiseLite.SetNoiseType(_noiseType);
        noiseLite.SetRotationType3D(_rotationType);
        noiseLite.SetSeed(RandomService.Seed);
        noiseLite.SetFrequency(_frequency);
        noiseLite.SetFractalType(FastNoiseLite.FractalType.Ridged);
        noiseLite.SetFractalOctaves(_fracOctave);
        noiseLite.SetFractalLacunarity(_fracLacunarity);
        noiseLite.SetFractalGain(_fracGain);
        noiseLite.SetFractalWeightedStrength(_fracWeightedStrength);
        noiseLite.SetCellularDistanceFunction(_cellularDistance);
        noiseLite.SetCellularJitter(_cellJitter);
        noiseLite.SetFractalPingPongStrength(_pingpongStrength);
    }

    private float GetNoiseData(FastNoiseLite noise, int x, int y)
    {
        var amplifiedNoise = noise.GetNoise(x, y) * _amplitude;

        return Mathf.Clamp(amplifiedNoise, -1.0f, 1.0f);
    }

    private float Get01NoiseData(FastNoiseLite noise, int x, int y)
    {
        return (GetNoiseData(noise, x, y) + 1.0f / 2.0f);
    }

    private void MakeMap(KeyValuePair<Vector2Int, string> cell)
    {
        if (Grid.TryGetCellByCoordinates(cell.Key.x, cell.Key.y, out Cell actualCell))
        {
            AddTileToCell(actualCell, cell.Value);
        }
    }

    private void CheckCell(Cell cell, float rand)
    {
        if (rand <= _grassQuantity)
            cellsByTileName[new Vector2Int(cell.Coordinates.x, cell.Coordinates.y)] = GRASS_TILE_NAME;
        else if (rand <= _waterQuantity + _grassQuantity)
            cellsByTileName[new Vector2Int(cell.Coordinates.x, cell.Coordinates.y)] = WATER_TILE_NAME;
        else
            cellsByTileName[new Vector2Int(cell.Coordinates.x, cell.Coordinates.y)] = SAND_TILE_NAME;
    }
}
