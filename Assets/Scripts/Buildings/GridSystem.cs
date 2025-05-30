using UnityEngine;

namespace Buildings
{
    public class GridSystem
    {
        private readonly int _width;
        private readonly int _height;
        private readonly Building[,] _grid;

        public int Width => _width;
        public int Height => _height;

        public GridSystem(int width, int height)
        {
            _width = width;
            _height = height;
            _grid = new Building[width, height];
        }

        public bool IsOccupied(Vector2Int pos, Vector2Int size)
        {
            for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
                if (_grid[pos.x + x, pos.y + y] != null) return true;

            return false;
        }

        public void PlaceBuilding(Vector2Int pos, Building building)
        {
            for (int x = 0; x < building.Size.x; x++)
            for (int y = 0; y < building.Size.y; y++)
                _grid[pos.x + x, pos.y + y] = building;
        }

        public bool IsInsideBounds(Vector2Int pos, Vector2Int size)
        {
            return pos.x >= 0 && pos.y >= 0 &&
                   pos.x + size.x <= _width &&
                   pos.y + size.y <= _height;
        }
    }
}