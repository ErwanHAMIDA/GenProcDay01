using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using VTools.Grid;

[CreateAssetMenu(menuName = "Procedural Generation Method/Cellular Automata")]
public class CellularAutomata : ProceduralGenerationMethod
{
    [SerializeField, Range(0, 100), Tooltip("Grass proportion")] private int _noiseDensity = 60;
    [SerializeField, Range(2, 6), Tooltip("2-3 less water | 4 balance | 5-6 big water")] private int _neighbor = 3;
    [SerializeField, Range(2, 6), Tooltip("Sand proportion")] private int _sandProportion = 5;
    [SerializeField] private int _width = 50;
    [SerializeField] private int _height = 50;
    private Dictionary<Vector2Int, string> cellsByTileName = new Dictionary<Vector2Int, string>();

    protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
    {
        InitMap();

        for (int i = 0; i < _maxSteps; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            cellsByTileName.Clear();
            
            foreach (Cell cell in Grid.Cells) 
                CheckCell(cell);

            foreach (var cell in cellsByTileName)
                MakeMap(cell);

            await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
        }
    }

    private void MakeMap(KeyValuePair<Vector2Int, string> cell)
    {
        if (Grid.TryGetCellByCoordinates(cell.Key.x, cell.Key.y, out Cell actualCell))
        {
            AddTileToCell(actualCell, cell.Value);
        }
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

        //for (int y = 0; y < Grid.Lenght; y++)
        //{
        //    for (int x = 0; x < Grid.Width; x++)
        //    {
        //        if (Grid.TryGetCellByCoordinates(x, y, out Cell actualCell))
        //        {
        //            int randTile = RandomService.Range(0, 100);

        //            switch (randTile < _noiseDensity)
        //            {
        //                case true:
        //                    AddTileToCell(actualCell, GRASS_TILE_NAME, true);
        //                    break;
        //                case false:
        //                    AddTileToCell(actualCell, WATER_TILE_NAME, true);
        //                    break;
        //            }
        //        }
        //    }
        //}
    }

    private void CheckCell(Cell cell)
    {
        int Grass = 0;
        int Sand = 0;

        //How to one loop ?
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (x == 0 && y == 0) continue;

                if (Grid.TryGetCellByCoordinates(cell.Coordinates.x + x, cell.Coordinates.y + y, out Cell sideCell))
                {
                    if (sideCell.GridObject.Template.Name == GRASS_TILE_NAME)
                        Grass++;
                    if (sideCell.GridObject.Template.Name == WATER_TILE_NAME)
                        Sand++;
                }
            }
        }

        if (Sand == _sandProportion && Grass >= 2 && Grass <= 5)
            cellsByTileName[new Vector2Int(cell.Coordinates.x, cell.Coordinates.y)] = SAND_TILE_NAME;
        else if (Grass >= _neighbor)
            cellsByTileName[new Vector2Int(cell.Coordinates.x, cell.Coordinates.y)] = GRASS_TILE_NAME;
        else
            cellsByTileName[new Vector2Int(cell.Coordinates.x, cell.Coordinates.y)] = WATER_TILE_NAME;
    }
}
