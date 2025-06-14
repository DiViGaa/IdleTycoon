using System.Collections.Generic;
using Data;
using Interface;
using JSON;
using UnityEngine;

namespace Buildings
{
    public class BuildingSaver : IBuildingService
    {
        private readonly GridSystem _grid;
        private readonly BuildingFactory _factory;
        private readonly BuildingSaveHandler _saveHandler = new();

        public List<SavedBuildingData> SavedBuildings { get; private set; } = new();

        public BuildingSaver(GridSystem grid, BuildingFactory factory)
        {
            _grid = grid;
            _factory = factory;
        }

        public void Initialize() => Load();
        public void Dispose() { }

        public void Save()
        {
            _saveHandler.Save(new SavedBuildingsWrapper { Buildings = SavedBuildings });
        }

        public void Load()
        {
            var wrapper = _saveHandler.Load();
            if (wrapper?.Buildings == null) return;

            foreach (var data in wrapper.Buildings)
            {
                var building = _factory.CreateBuilding(data.BuildingId);
                if (building == null) continue;

                building.transform.position = new Vector3(data.Position.x, 0, data.Position.y);
                building.gameObject.isStatic = true;

                building.SetInstanceId(data.InstanceId);

                _grid.PlaceBuilding(data.Position, building);
                
                building.FinalizePlacement(data.IsBuilt);
            }


            SavedBuildings = wrapper.Buildings;
        }


        public void AddSaved(Building building, Vector2Int pos)
        {
            SavedBuildings.Add(new SavedBuildingData
            {
                BuildingId = building.TypeId,
                Position = pos,
                InstanceId = building.InstanceId,
                IsBuilt = building.IsBuilt
            });
        }

    }
}