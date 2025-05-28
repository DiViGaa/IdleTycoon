using System;
using System.Collections.Generic;
using System.IO;
using EnterPoints;
using Interface;
using Newtonsoft.Json;
using ServicesLocator;
using UnityEngine;
using Upgrade;

namespace Buildings
{
    public class BuildingManager : MonoBehaviour, IService, IDisposable
    {
        [SerializeField] Vector2Int GridSize = new Vector2Int(10, 10);
        [SerializeField] private List<Building> availablePrefabs;

        private Building[,] grid;
        private Building _flyingBuilding;
        private UnityEngine.Camera mainCamera;
        private List<SavedBuildingData> _savedBuildings = new();
        private string SavePath => Path.Combine(Application.persistentDataPath, "buildings.json");


        
        public void Initialize()
        {
            MainGameEnterPoint.OnUpdate += OnUpdate;
            grid = new Building[GridSize.x, GridSize.y];
            LoadBuildings();
            mainCamera = UnityEngine.Camera.main;
        }

        public void StartPlacingBuilding(Building buildingPrefab)
        {
            if (_flyingBuilding != null)
            {
                Destroy(_flyingBuilding.gameObject);
            }
        
            _flyingBuilding = Instantiate(buildingPrefab);
        }
        
        private void OnUpdate()
        {
            if (_flyingBuilding == null) return;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (!TryGetPlacementPosition(ray, out Vector3Int gridPosition, out RaycastHit hitInfo)) return;

            _flyingBuilding.transform.position = new Vector3(gridPosition.x, 0, gridPosition.z);

            bool canPlace = CanPlaceBuildingAt(gridPosition, hitInfo);

            _flyingBuilding.SetTransparent(canPlace);

            if (canPlace && Input.GetMouseButtonDown(0))
            {
                PlaceFlyingBuilding(gridPosition.x, gridPosition.z);
                TryDestroyHitObject(hitInfo);
            }
        }
        
        private bool TryGetPlacementPosition(Ray ray, out Vector3Int gridPosition, out RaycastHit hitInfo)
        {
            gridPosition = Vector3Int.zero;
            hitInfo = new RaycastHit();

            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            if (!groundPlane.Raycast(ray, out float distance))
                return false;

            Vector3 worldPos = ray.GetPoint(distance);

            int maxX = GridSize.x - _flyingBuilding.Size.x;
            int maxY = GridSize.y - _flyingBuilding.Size.y;

            int x = Mathf.Clamp(Mathf.FloorToInt(worldPos.x), 0, maxX);
            int y = Mathf.Clamp(Mathf.FloorToInt(worldPos.z), 0, maxY);

            gridPosition = new Vector3Int(x, 0, y);

            return Physics.Raycast(ray, out hitInfo, 100f);
        }

        private bool CanPlaceBuildingAt(Vector3Int pos, RaycastHit hit)
        {
            int x = pos.x;
            int y = pos.z;

            if (x < 0 || x > GridSize.x - _flyingBuilding.Size.x ||
                y < 0 || y > GridSize.y - _flyingBuilding.Size.y)
                return false;

            if (IsPlaceTaken(x, y))
                return false;

            if (IsOverlappingEnvironment(new Vector3(x, 0, y), _flyingBuilding.Size))
                return false;

            int hitLayer = hit.collider.gameObject.layer;
            if ((_flyingBuilding.AllowedBuildLayers.value & (1 << hitLayer)) == 0)
                return false;

            return true;
        }
        
        private bool IsOverlappingEnvironment(Vector3 position, Vector2Int size)
        {
            Vector3 center = position + new Vector3(size.x / 2f, 0.5f, size.y / 2f);
            Vector3 halfExtents = new Vector3(size.x / 2f, 1f, size.y / 2f);

            Collider[] hits = Physics.OverlapBox(center, halfExtents, Quaternion.identity, ~0, QueryTriggerInteraction.Ignore);

            int environmentLayer = LayerMask.NameToLayer("Environment");

            foreach (var col in hits)
            {
                if (col.gameObject == _flyingBuilding.gameObject)
                    continue;

                if (col.gameObject.layer == environmentLayer)
                    return true;
            }

            return false;
        }
        
        private void TryDestroyHitObject(RaycastHit hit)
        {
            int hitLayer = hit.collider.gameObject.layer;
            int groundLayer = LayerMask.NameToLayer("Ground");

            if (hitLayer != groundLayer)
            {
                Destroy(hit.collider.gameObject);
            }
        }

        
        private bool IsPlaceTaken(int placeX, int placeY)
        {
            for (int x = 0; x < _flyingBuilding.Size.x; x++)
            {
                for (int y = 0; y < _flyingBuilding.Size.y; y++)
                {
                    if (grid[placeX + x, placeY + y] != null) return true;
                }
            }

            return false;
        }

        private void PlaceFlyingBuilding(int placeX, int placeY)
        {
            for (int x = 0; x < _flyingBuilding.Size.x; x++)
            {
                for (int y = 0; y < _flyingBuilding.Size.y; y++)
                {
                    grid[placeX + x, placeY + y] = _flyingBuilding;
                }
            }
            
            _flyingBuilding.SetNormal();
            _flyingBuilding.gameObject.isStatic = true;
            _savedBuildings.Add(new SavedBuildingData
            {
                BuildingId = _flyingBuilding.BuildingId,
                Position = new Vector2Int(placeX, placeY),
            });
            SaveBuildings();
            _flyingBuilding = null;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(transform.position, new Vector3(GridSize.x, 1, GridSize.y));
            Gizmos.color = new Color(Color.green.r, Color.green.g, Color.green.b, 0.5f);
        }

        public void Dispose()
        {
            MainGameEnterPoint.OnUpdate -= OnUpdate;
            grid = null;
            mainCamera = null;
        }
        
        public void LoadBuildings()
        {
            if (!File.Exists(SavePath))
            {
                Debug.Log("[BuildingManager] No building save file found.");
                return;
            }

            string json = File.ReadAllText(SavePath);
            var wrapper = JsonUtility.FromJson<SavedBuildingsWrapper>(json);
    
            if (wrapper == null || wrapper.Buildings == null)
            {
                Debug.LogWarning("[BuildingManager] Failed to deserialize buildings.");
                return;
            }

            foreach (var saved in wrapper.Buildings)
            {
                Debug.Log($"[LoadBuildings] Try loading: {saved.BuildingId} at {saved.Position}");

                var prefab = GetBuildingPrefabById(saved.BuildingId);
                if (prefab == null)
                {
                    Debug.LogWarning($"[LoadBuildings] Prefab with ID '{saved.BuildingId}' not found.");
                    continue;
                }

                var building = Instantiate(prefab);
                building.transform.position = new Vector3(saved.Position.x, 0, saved.Position.y);
                building.gameObject.isStatic = true;
                Debug.Log($"[LoadBuildings] Instantiated {saved.BuildingId} at {saved.Position}");
                
                for (int x = 0; x < building.Size.x; x++)
                for (int y = 0; y < building.Size.y; y++)
                    grid[saved.Position.x + x, saved.Position.y + y] = building;
            }


            _savedBuildings = wrapper.Buildings;
            Debug.Log($"[BuildingManager] Loaded {_savedBuildings.Count} buildings.");
        }

        
        private Building GetBuildingPrefabById(string id)
        {
            return availablePrefabs.Find(b => b.BuildingId == id);
        }
        
        public void SaveBuildings()
        {
            var wrapper = new SavedBuildingsWrapper { Buildings = _savedBuildings };
            string json = JsonUtility.ToJson(wrapper, true);
            File.WriteAllText(SavePath, json);
            Debug.Log("[BuildingManager] Saved " + _savedBuildings.Count + " buildings.");
        }

    }
}
