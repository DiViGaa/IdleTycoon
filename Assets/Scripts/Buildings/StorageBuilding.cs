using UnityEngine;
using System.Collections.Generic;
using Interface;
using Player;
using Upgrade;

namespace Buildings
{
    public class StorageBuilding : Building, IResourceProvider, IResourceReceiver, IStorageMarker
    {
        private Dictionary<ResourceType, float> _storage = new();

        private float CapacityPerResource => 1000f * UpgradeLevel;
        private LogisticsCenterUpgrade Storage => (LogisticsCenterUpgrade)upgradeState;

        public override string GetUpgradeInfo()
        {
            return $"Level: {UpgradeLevel}\n" +
                   $"Storage Capacity: {CapacityPerResource} per resource";
        }

        public override void Start()
        {
            base.Start();

            foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
            {
                _storage[type] = 0;
            }
        }


        public void AddResource(ResourceType type, float amount)
        {
            if (!_storage.ContainsKey(type))
                _storage[type] = 0;

            float newAmount = Mathf.Min(_storage[type] + amount, CapacityPerResource);
            float actuallyAdded = newAmount - _storage[type]; 

            _storage[type] = newAmount;

            if (actuallyAdded > 0)
            {
                var player = ServicesLocator.ServiceLocator.Current.Get<Player.Player>();
                if (player != null)
                {
                    player.ChangeResource(type, +actuallyAdded);
                }
            }
        }

        
        public float GetAmount(ResourceType type)
        {
            return _storage.TryGetValue(type, out float val) ? val : 0;
        }

        public bool TryTakeResource(ResourceType type, float amount)
        {
            if (!_storage.TryGetValue(type, out float val)) return false;

            if (val < amount) return false;

            _storage[type] -= amount;

            var player = ServicesLocator.ServiceLocator.Current.Get<Player.Player>();
            if (player != null)
            {
                player.ChangeResource(type, -amount);
            }

            return true;
        }

        protected override void OnUpgraded()
        {
            Storage.Upgrade();
            base.OnUpgraded();
        }
    }
}