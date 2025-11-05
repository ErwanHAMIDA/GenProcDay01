using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VTools.Grid;

[CreateAssetMenu(menuName = "Procedural Generation Method/Cellular Automata")]
public class CellularAutomata : ProceduralGenerationMethod
{
    [SerializeField] private int _noiseDensity = 50;
    [SerializeField] private int _width = 50;
    [SerializeField] private int _height = 50;
    Dictionary<Cell, string> cellsByTileName;
    protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
    {
        for (int i = 0; i < _maxSteps; i++)
        {
            int Grass = 0;
            cancellationToken.ThrowIfCancellationRequested();

            Cell actualCell = new Cell(i / Grid.Width , i % Grid.Lenght, 1);

            InitMap(actualCell);

            //Step i de l'algo
            for (int j = 0; j < 8; j++)
            {
                if (j == 4) continue;

                if (Grid.TryGetCellByCoordinates(j / 3, j % 3, out Cell cell))
                {
                    if (cell.GridObject.Template.Name == GRASS_TILE_NAME)
                    {
                        cellsByTileName.Add(cell, GRASS_TILE_NAME);
                        Grass++;
                    }
                    else
                    {
                        cellsByTileName.Add(cell, WATER_TILE_NAME);
                    }
                }
            }

            if (Grass >= 4)
            {
            }


            await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
        }
    }

    private void InitMap(Cell actualCell)
    {
        int gridSize = Grid.Lenght * Grid.Width;

        for (int i = 0; i < gridSize; i++)
        {
            int randTile = RandomService.Range(0, 2);

            switch (randTile)
            {
                case 0:
                    AddTileToCell(actualCell, GRASS_TILE_NAME, true);
                    break;
                case 1:
                    AddTileToCell(actualCell, WATER_TILE_NAME, true);
                    break;
            }
        }
    }

    private void CheckCell(int index)
    {
        
    }
}
