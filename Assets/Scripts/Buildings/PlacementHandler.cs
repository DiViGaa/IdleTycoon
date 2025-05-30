using UnityEngine;

namespace Buildings
{
    public class PlacementHandler : IBuildingService
    {
        private readonly UnityEngine.Camera _camera;
        private readonly GridSystem _grid;
        
        public PlacementHandler(UnityEngine.Camera camera, GridSystem grid)
        {
            _camera = camera;
            _grid = grid;
        }

        public void Initialize() { }
        public void Dispose() { }

        public bool TryGetPlacement(Ray ray, Building building, Vector2Int maxSize, out Vector2Int gridPos, out RaycastHit hit)
        {
            gridPos = Vector2Int.zero;
            hit = new RaycastHit();

            Plane groundPlane = new(Vector3.up, Vector3.zero);
            if (!groundPlane.Raycast(ray, out float distance)) return false;

            Vector3 world = ray.GetPoint(distance);
            int x = Mathf.Clamp(Mathf.FloorToInt(world.x), 0, maxSize.x - building.Size.x);
            int y = Mathf.Clamp(Mathf.FloorToInt(world.z), 0, maxSize.y - building.Size.y);

            gridPos = new Vector2Int(x, y);
            return Physics.Raycast(ray, out hit, 100f);
        }

        public bool CanPlace(Building building, Vector2Int pos, RaycastHit hit)
        {
            if (!_grid.IsInsideBounds(pos, building.Size)) return false;
            if (_grid.IsOccupied(pos, building.Size)) return false;

            int hitLayer = hit.collider.gameObject.layer;
            return (building.AllowedBuildLayers.value & (1 << hitLayer)) != 0;
        }
        
        public void TryRemoveObstacle(RaycastHit hit)
        {
            int groundLayer = LayerMask.NameToLayer("Ground");
            if (hit.collider != null && hit.collider.gameObject.layer != groundLayer)
            {
                Object.Destroy(hit.collider.gameObject);
            }
        }

    }
}