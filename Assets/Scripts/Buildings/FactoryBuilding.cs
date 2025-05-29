using UnityEngine;
using System.Collections.Generic;
using Interface;
using Player;
using Upgrade;

namespace Buildings
{
    public class FactoryBuilding : Building, IResourceProvider, IResourceReceiver
    {
        public ResourceType ProducedResource { get; private set; } = ResourceType.Cenoxium;

        private FactoryUpgrade Plant => (FactoryUpgrade)upgradeState;

        private float _storedOutput = 0f;

        private readonly Dictionary<ResourceType, float> _inputResources = new();

        private readonly Dictionary<ResourceType, (ResourceType input1, float ratio1, ResourceType input2, float ratio2)> productionRecipes
            = new()
        {
            { ResourceType.Cenoxium, (ResourceType.Zenthite, 1f, ResourceType.Velorith, 1f) },
            { ResourceType.Thalorite, (ResourceType.Crysalor, 2f, ResourceType.Velorith, 1f) }
        };

        private float StorageBuffer => Plant.StorageBuffer;

        public override void Start()
        {
            base.Start();

            foreach (var res in System.Enum.GetValues(typeof(ResourceType)))
                _inputResources[(ResourceType)res] = 0f;
        }

        private void Update()
        {
            Produce(Time.deltaTime);
        }

        private void Produce(float deltaTime)
        {
            if (!productionRecipes.TryGetValue(ProducedResource, out var recipe))
                return;

            float efficiency = 1f + 0.25f * (UpgradeLevel - 1);
            float productionAmount = efficiency * deltaTime;

            float availableInput1 = _inputResources[recipe.input1];
            float availableInput2 = _inputResources[recipe.input2];

            float maxFromInput1 = availableInput1 / recipe.ratio1;
            float maxFromInput2 = availableInput2 / recipe.ratio2;

            float producible = Mathf.Min(productionAmount, maxFromInput1, maxFromInput2);

            if (producible <= 0f)
                return;

            _inputResources[recipe.input1] -= producible * recipe.ratio1;
            _inputResources[recipe.input2] -= producible * recipe.ratio2;

            _storedOutput += producible;
            _storedOutput = Mathf.Min(_storedOutput, StorageBuffer);
        }


        public void AddResource(ResourceType type, float amount)
        {
            if (!_inputResources.ContainsKey(type))
                _inputResources[type] = 0;

            _inputResources[type] += amount;
        }
        
        public float GetAmount(ResourceType type)
        {
            return type == ProducedResource ? _storedOutput : 0f;
        }

        public bool TryTakeResource(ResourceType type, float amount)
        {
            if (type != ProducedResource || _storedOutput < amount) return false;

            _storedOutput -= amount;
            return true;
        }

        public override string GetUpgradeInfo()
        {
            float efficiency = 1f + 0.25f * (UpgradeLevel - 1);

            if (productionRecipes.TryGetValue(ProducedResource, out var recipe))
            {
                return $"Level: {UpgradeLevel}\n" +
                       $"Producing: {ProducedResource}\n" +
                       $"Requires: {recipe.input1} x{recipe.ratio1}, {recipe.input2} x{recipe.ratio2}\n" +
                       $"Efficiency: {efficiency:P0}\n" +
                       $"Storage Buffer: {StorageBuffer}";
            }

            return $"Level: {UpgradeLevel}\nUnknown recipe";
        }

        public void SetProducedResource(ResourceType resourceType)
        {
            if (productionRecipes.ContainsKey(resourceType))
                ProducedResource = resourceType;
            else
                Debug.LogWarning($"Ресурс {resourceType} не поддерживается заводом.");
        }

        protected override void OnUpgraded()
        {
            Plant.Upgrade();
            base.OnUpgraded();
        }
    }
}
