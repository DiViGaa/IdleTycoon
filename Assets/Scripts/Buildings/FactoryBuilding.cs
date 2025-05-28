using System.Collections.Generic;
using Player;
using UnityEngine;
using ServicesLocator;
using Upgrade;

namespace Buildings
{
    public class FactoryBuilding : Building
    {
        private FactoryUpgrade Plant => (FactoryUpgrade)upgradeState;

        private float _storedOutput = 0f;
        
        private Player.Player _player;

        public ResourceType ProducedResource { get; private set; } = ResourceType.Cenoxium;

        private readonly Dictionary<ResourceType, (ResourceType input1, float ratio1, ResourceType input2, float ratio2)> productionRecipes
            = new()
        {
            { ResourceType.Cenoxium, (ResourceType.Zenthite, 1f, ResourceType.Velorith, 1f) },
            { ResourceType.Thalorite, (ResourceType.Crysalor, 2f, ResourceType.Velorith, 1f) }
        };

        private float StorageBuffer => Plant.StorageBuffer;

        public override string GetUpgradeInfo()
        {
            float efficiency = 1 + 0.25f * (UpgradeLevel - 1);
            return $"Level: {UpgradeLevel}\nProduction Efficiency: {efficiency:P0}\nProducing: {ProducedResource}\nStorage Buffer: {StorageBuffer}";
        }

        public override void Start()
        {
            base.Start();
            _player = ServiceLocator.Current.Get<Player.Player>();
        }

        private void Update()
        {
            Produce(Time.deltaTime);
            TryStoreOutput();
        }

        private void Produce(float deltaTime)
        {
            if (!productionRecipes.TryGetValue(ProducedResource, out var recipe))
                return;
            
            float efficiency = 1 + 0.25f * (UpgradeLevel - 1);

            float productionAmount = efficiency * deltaTime;

            float availableInput1 = _player.GetResource(recipe.input1);
            float availableInput2 = _player.GetResource(recipe.input2);

            float maxFromInput1 = availableInput1 / recipe.ratio1;
            float maxFromInput2 = availableInput2 / recipe.ratio2;

            float producible = Mathf.Min(productionAmount, maxFromInput1, maxFromInput2);

            if (producible <= 0f)
                return;

            _player.ChangeResource(recipe.input1, -producible * recipe.ratio1);
            _player.ChangeResource(recipe.input2, -producible * recipe.ratio2);

            _storedOutput += producible;
        }

        private void TryStoreOutput()
        {
            if (_storedOutput >= StorageBuffer)
            {
                float sendAmount = Mathf.Floor(_storedOutput);
                _storedOutput -= sendAmount;

                _player.ChangeResource(ProducedResource, sendAmount);
            }
        }

        public void SetProducedResource(ResourceType resourceType)
        {
            if (productionRecipes.ContainsKey(resourceType))
            {
                ProducedResource = resourceType;
            }
            else
            {
                Debug.LogWarning($"Resource {resourceType} cannot be produced by this plant.");
            }
        }

        protected override void OnUpgraded()
        {
            Plant.Upgrade();
            base.OnUpgraded();
        }
    }
}
