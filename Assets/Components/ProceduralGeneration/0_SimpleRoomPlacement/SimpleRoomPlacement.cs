using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using VTools.Grid;
using VTools.ScriptableObjectDatabase;
using VTools.Utility;

namespace Components.ProceduralGeneration.SimpleRoomPlacement
{
    [CreateAssetMenu(menuName = "Procedural Generation Method/Simple Room Placement")]
    public class SimpleRoomPlacement : ProceduralGenerationMethod
    {
        [Header("Room Parameters")]
        [SerializeField] private int _maxRooms = 10;
        protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
        {
            // Declare variables here
            // ........
            int width;
            int height;
            int coordX;
            int coordY;

            for (int i = 0; i < _maxSteps; i++)
            {
                // Check for cancellation
                cancellationToken.ThrowIfCancellationRequested();

                width = RandomService.Range(0, 10);
                height = RandomService.Range(0, 10);
                coordX = RandomService.Range(0, Grid.Width);
                coordY = RandomService.Range(0, Grid.Lenght);

                RectInt smallRoom = new RectInt(coordX, coordY, width, height);

                // Your algorithm here
                // .......
                PlaceRoom(smallRoom);

                // Waiting between steps to see the result.
                await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
            }

            // Final ground building.
            BuildGround();
        }

        private void PlaceRoom(RectInt room)
        {
            for (int column = room.xMin; column < room.xMax; column++)
            {
                for (int row = room.yMin; row < room.yMax; row++)
                {
                    if (!Grid.TryGetCellByCoordinates(column, row, out var cell))
                        continue;
                    else if (CanPlaceRoom(room, 2))
                        AddTileToCell(cell, WATER_TILE_NAME, true);
                    
                }
            }
        }

        private void BuildGround()
        {
            var groundTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Grass");
            
            // Instantiate ground blocks
            for (int x = 0; x < Grid.Width; x++)
            {
                for (int z = 0; z < Grid.Lenght; z++)
                {
                    if (!Grid.TryGetCellByCoordinates(x, z, out var chosenCell))
                    {
                        Debug.LogError($"Unable to get cell on coordinates : ({x}, {z})");
                        continue;
                    }
                    
                    GridGenerator.AddGridObjectToCell(chosenCell, groundTemplate, false);
                }
            }
        }
    }
}