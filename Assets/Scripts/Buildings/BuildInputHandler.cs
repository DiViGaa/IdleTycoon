using UnityEngine;
using EnterPoints;
using Interface;
using Player;
using ServicesLocator;

namespace Buildings
{
    public class BuildInputHandler : IBuildingService
    {
        private readonly BuildingManager _manager;
        private Building _current;
        private int _pendingCost;
        private bool _hasPendingCost;

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
                _hasPendingCost = false;
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
                _current.FinalizePlacement(true);
                _manager.Saver.AddSaved(_current, gridPos);
                _current = null;
                
                if (_hasPendingCost)
                {
                    ServiceLocator.Current.Get<Player.Player>().ChangeResource(ResourceType.Coins, -_pendingCost);
                    _hasPendingCost = false;
                }
            }
        }

        public void StartPlacementWithCost(Building prefab, int cost)
        {
            if (_current != null)
                Object.Destroy(_current.gameObject);

            _current = Object.Instantiate(prefab);
            _pendingCost = cost;
            _hasPendingCost = true;
        }

    }
}