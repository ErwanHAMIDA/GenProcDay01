using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VTools.Grid;

[CreateAssetMenu(menuName = "Procedural Generation Method/Noise")]
public class Noise : ProceduralGenerationMethod
{
    [SerializeField, Range(0, 100), Tooltip("Grass proportion")] private int _noiseDensity = 60;
    [SerializeField, Range(0, 0.2f), Tooltip("Noise frequency")] private float _frequency = 0.1f;
    [SerializeField] private int _noiseSeed = 50;
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
                    noiseData[x, y] = noiseLite.GetNoise(x, y);
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
        noiseLite.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
        noiseLite.SetRotationType3D(FastNoiseLite.RotationType3D.ImproveXYPlanes);
        noiseLite.SetSeed(RandomService.Seed);
        noiseLite.SetFrequency(_frequency);
        noiseLite.SetFractalType(FastNoiseLite.FractalType.Ridged);
        noiseLite.SetFractalOctaves(3);
        noiseLite.SetFractalLacunarity(1.0f);
        noiseLite.SetFractalGain(0.9f);
        noiseLite.SetFractalWeightedStrength(0.5f);
    }

    private void ChangeNoise()
    {
        noiseLite.SetSeed(RandomService.Range(0, 1000)); 
    }

    private void InitMap()
    {
        int gridSize = Grid.Lenght * Grid.Width;

        for (int l = 0; l < gridSize; l++)
        {
            if (Grid.TryGetCellByCoordinates(l / Grid.Width, l % Grid.Width, out Cell actualCell))
            {
                int randTile = RandomService.Range(0, 100);

                switch (randTile < _noiseDensity)
                {
                    case true:
                        AddTileToCell(actualCell, GRASS_TILE_NAME);
                        break;
                    case false:
                        AddTileToCell(actualCell, WATER_TILE_NAME);
                        break;
                }
            }
        }
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
        switch (rand)
        {
            case < -0.2f:
                cellsByTileName[new Vector2Int(cell.Coordinates.x, cell.Coordinates.y)] = GRASS_TILE_NAME;
                break;
            case > 0.2f:
                cellsByTileName[new Vector2Int(cell.Coordinates.x, cell.Coordinates.y)] = WATER_TILE_NAME;
                break;
            default:
                cellsByTileName[new Vector2Int(cell.Coordinates.x, cell.Coordinates.y)] = SAND_TILE_NAME;
                break;
        }
    }
}
