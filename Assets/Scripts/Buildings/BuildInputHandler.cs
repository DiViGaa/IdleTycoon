using UnityEngine;
using EnterPoints;

namespace Buildings
{
    public class BuildInputHandler : IBuildingService
    {
        private readonly BuildingManager _manager;
        private Building _current;

        public BuildInputHandler(BuildingManager manager)
        {
            _manager = manager;
        }

        public void Initialize()
        {
            MainGameEnterPoint.OnUpdate += OnUpdate;
        }

        public void Dispose()
        {
            MainGameEnterPoint.OnUpdate -= OnUpdate;
        }

        private void OnUpdate()
        {
            if (_current == null) return;
            
            if (Input.GetMouseButtonDown(2))
            {
                Object.Destroy(_current.gameObject);
                _current = null;
                return;
            }
            
            var ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!_manager.PlacementHandler.TryGetPlacement(ray, _current, new Vector2Int(_manager.GridSystem.Width, _manager.GridSystem.Height), out var gridPos, out var hit))
                return;

            _current.transform.position = new Vector3(gridPos.x, 0, gridPos.y);
            bool canPlace = _manager.PlacementHandler.CanPlace(_current, gridPos, hit);
            _current.SetTransparent(canPlace);

            if (canPlace && Input.GetMouseButtonDown(0))
            {
                _manager.PlacementHandler.TryRemoveObstacle(hit);
                _manager.GridSystem.PlaceBuilding(gridPos, _current);
                _current.SetNormal();
                _current.gameObject.isStatic = true;
                _manager.Saver.AddSaved(_current, gridPos);
                _manager.Saver.Save();
                _current = null;
            }
        }

        public void StartPlacement(Building prefab)
        {
            if (_current != null)
                Object.Destroy(_current.gameObject);

            _current = Object.Instantiate(prefab);
        }
    }
}