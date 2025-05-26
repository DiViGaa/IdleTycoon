using System;
using EnterPoints;
using Interface;
using UnityEngine;

namespace Buildings
{
    public class BuildingManager : MonoBehaviour, IService, IDisposable
    {
        [SerializeField] Vector2Int GridSize = new Vector2Int(10, 10);

        private Building[,] grid;
        private Building _flyingBuilding;
        private UnityEngine.Camera mainCamera;
    
        public void Initialize()
        {
            MainGameEnterPoint.OnUpdate += OnUpdate;
            grid = new Building[GridSize.x, GridSize.y];
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
            if (_flyingBuilding != null)
            {
                var groundPlane = new Plane(Vector3.up, Vector3.zero);
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                if (groundPlane.Raycast(ray, out float position))
                {
                    Vector3 worldPosition = ray.GetPoint(position);

                    int maxX = GridSize.x - _flyingBuilding.Size.x;
                    int maxY = GridSize.y - _flyingBuilding.Size.y;

                    int x = Mathf.Clamp(Mathf.FloorToInt(worldPosition.x), 0, maxX);
                    int y = Mathf.Clamp(Mathf.FloorToInt(worldPosition.z), 0, maxY);


                    bool available = true;

                    if (x < 0 || x > GridSize.x - _flyingBuilding.Size.x)
                    {
                        available = false;
                    }

                    if (y < 0 || y > GridSize.y - _flyingBuilding.Size.y)
                    {
                        available = false;
                    }

                    if (available && IsPlaceTaken(x, y)) available = false;
                    _flyingBuilding.transform.position = new Vector3(x, 0, y);

                    if (available && Input.GetMouseButtonDown(0))
                    {
                        if (Physics.Raycast(ray, out RaycastHit hitInfoClick, 100f))
                        {
                            int clickedLayer = hitInfoClick.collider.gameObject.layer;
                            if (((_flyingBuilding.AllowedBuildLayers.value & (1 << clickedLayer)) != 0))
                            {
                                PlaceFlyingBuilding(x, y);
                                Destroy(hitInfoClick.collider.gameObject);
                            }
                        }
                    }
                }
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
    }
}
