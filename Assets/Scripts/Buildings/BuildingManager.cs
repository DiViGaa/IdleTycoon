using System;
using System.Collections.Generic;
using Interface;
using UnityEngine;

namespace Buildings
{
    public class BuildingManager : MonoBehaviour, IService, IDisposable
    {
        [SerializeField] private Vector2Int gridSize = new(10, 10);
        
        private List<Building> _availablePrefabs;
        private readonly List<IBuildingService> _services = new();

        public GridSystem GridSystem { get; private set; }
        public PlacementHandler PlacementHandler { get; private set; }
        public BuildingFactory Factory { get; private set; }
        public BuildingSaver Saver { get; private set; }
        public BuildInputHandler InputHandler { get; private set; }

        public void Initialize()
        {
            LoadAvailablePrefabs();
            GridSystem = new GridSystem(gridSize.x, gridSize.y);
            PlacementHandler = new PlacementHandler(GridSystem);
            Factory = new BuildingFactory(_availablePrefabs);
            Saver = new BuildingSaver(GridSystem, Factory);
            InputHandler = new BuildInputHandler(this);

            _services.AddRange(new IBuildingService[]
            {
                PlacementHandler, Saver, InputHandler
            });

            foreach (var service in _services)
                service.Initialize();
        }
        
        private void LoadAvailablePrefabs()
        {
            _availablePrefabs = new List<Building>(
                Resources.LoadAll<Building>("Prefabs/Buildings")
            );
        }

        public void Dispose()
        {
            foreach (var service in _services)
                service.Dispose();
            _availablePrefabs.Clear();
        }
    }
}