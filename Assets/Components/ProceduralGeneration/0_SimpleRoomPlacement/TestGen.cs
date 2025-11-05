using System.Threading;
using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VTools.Grid;
using VTools.ScriptableObjectDatabase;
using VTools.Utility;
using VTools.RandomService;

public class TestGen
{
    [CreateAssetMenu(menuName = "Procedural Generation Method/Test algo")]
    public class TestAlgo : ProceduralGenerationMethod
    {
        protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
        {
            Debug.Log("Test algo");
            var allGrid = new RectInt(0, 0, Grid.Width, Grid.Lenght);
            var root = new TestNode(allGrid, RandomService);
        }
    }

    public class TestNode
    {
        private readonly RectInt _bounds;
        private readonly RandomService _randomService;
        private TestNode _child1, _child2;

        private Vector2Int _roomMinSize = new(5, 5);

        public TestNode(RectInt bounds, RandomService randomService)
        {
            _bounds = bounds;
            _randomService = randomService;

            RectInt splitBoundsLeft = new RectInt(_bounds.xMin, _bounds.yMin, _bounds.width / 2, _bounds.height);
            RectInt splitBoundsRight = new RectInt(_bounds.xMin + _bounds.width / 2, _bounds.yMax, _bounds.width / 2, _bounds.height);

            if (splitBoundsLeft.width < _roomMinSize.x || splitBoundsLeft.height < _roomMinSize.y)
            {
                // It's a Leaf !
                //Place....

                return;
            }

            _child1 = new TestNode(splitBoundsLeft, _randomService);
            _child2 = new TestNode(splitBoundsRight, _randomService);
        }
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
}
