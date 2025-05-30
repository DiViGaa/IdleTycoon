using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    public class BuildingFactory
    {
        private readonly List<Building> _prefabs;

        public BuildingFactory(List<Building> prefabs)
        {
            _prefabs = prefabs;
        }

        public Building GetPrefabById(string id)
        {
            return _prefabs.Find(b => b.BuildingId == id);
        }

        public Building CreateBuilding(string id)
        {
            var prefab = GetPrefabById(id);
            return prefab != null ? Object.Instantiate(prefab) : null;
        }
    }
}