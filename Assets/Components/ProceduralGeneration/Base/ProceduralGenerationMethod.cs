using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using VTools.Grid;
using VTools.RandomService;
using VTools.ScriptableObjectDatabase;

namespace Components.ProceduralGeneration
{
    public abstract class ProceduralGenerationMethod : ScriptableObject
    {
        [Header("Generation")] 
        [SerializeField, Range(3, 10), Tooltip("Number of steps")] protected int _maxSteps = 1000;

        // Injected at runtime, not serialized
        [NonSerialized] public ProceduralGridGenerator GridGenerator;
        [NonSerialized] protected RandomService RandomService;
        [NonSerialized] private CancellationTokenSource _cancellationTokenSource;
        private GridObjectTemplate sandTileTemplate;
        private GridObjectTemplate waterTileTemplate;
        private GridObjectTemplate grassTileTemplate;

        protected VTools.Grid.Grid Grid => GridGenerator.Grid;

        protected const string ROOM_TILE_NAME = "Room";
        protected const string CORRIDOR_TILE_NAME = "Corridor";
        protected const string GRASS_TILE_NAME = "Grass";
        protected const string WATER_TILE_NAME = "Water";
        protected const string ROCK_TILE_NAME = "Rock";
        protected const string SAND_TILE_NAME = "Sand";

        protected string ACTUAL_TILE_NAME;

        // -------------------------------------- BASE ----------------------------------------------------

        public void Initialize(ProceduralGridGenerator gridGenerator, RandomService randomService)
        {
            GridGenerator = gridGenerator;
            RandomService = randomService;
            grassTileTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>(GRASS_TILE_NAME);
            sandTileTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>(SAND_TILE_NAME);
            waterTileTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>(WATER_TILE_NAME);
        }

        public async UniTask Generate()
        {
            // Cancel any ongoing generation
            _cancellationTokenSource?.Cancel();

            // Give it a moment to actually cancel
            await UniTask.Delay(GridGenerator.StepDelay + 100);
            _cancellationTokenSource?.Dispose();

            // Create a new cancellation token for this generation
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                // Stay on the main thread to safely manipulate Unity objects
                await UniTask.SwitchToMainThread();
                
                await ApplyGeneration(_cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Generation was cancelled.");
            }
            finally
            {
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
        }

        protected abstract UniTask ApplyGeneration(CancellationToken cancellationToken);
        
        // -------------------------------------- HELPERS ----------------------------------------------------
        
        /// Checks if the room can be placed.
        protected bool CanPlaceRoom(RectInt room, int spacing)
        {
            // optional spacing: extend by one tile around for buffer
            int xMin = Mathf.Max(room.xMin - spacing, 0);
            int yMin = Mathf.Max(room.yMin - spacing, 0);
            int xMax = Mathf.Min(room.xMax + spacing, Grid.Width);
            int yMax = Mathf.Min(room.yMax + spacing, Grid.Lenght);

            for (int ix = xMin; ix < xMax; ix++)
            {
                for (int iy = yMin; iy < yMax; iy++)
                {
                    if (Grid.TryGetCellByCoordinates(ix, iy, out var cell) && cell.ContainObject && cell.GridObject.Template.Name == "Room") 
                        return false;
                }
            }

            return true;
        }
        
        protected void AddTileToCell(Cell cell, string tileName)
        {
            if (ACTUAL_TILE_NAME == tileName)
            {
                switch (tileName)
                {
                    case GRASS_TILE_NAME:
                        GridGenerator.AddGridObjectToCell(cell, grassTileTemplate, false);
                        break;
                    case WATER_TILE_NAME:
                        GridGenerator.AddGridObjectToCell(cell, waterTileTemplate, false);
                        break;
                    case SAND_TILE_NAME:
                        GridGenerator.AddGridObjectToCell(cell, sandTileTemplate, false);
                        break;
                }
            }
            else
            {
                switch (tileName)
                {
                    case GRASS_TILE_NAME:
                        GridGenerator.AddGridObjectToCell(cell, grassTileTemplate, true);
                        break;
                    case WATER_TILE_NAME:
                        GridGenerator.AddGridObjectToCell(cell, waterTileTemplate, true);
                        break;
                    case SAND_TILE_NAME:
                        GridGenerator.AddGridObjectToCell(cell, sandTileTemplate, true);
                        break;
                }
            }
        }
    }
}