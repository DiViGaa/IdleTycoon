using DialogsManager;
using DialogsManager.Dialogs;
using Interface;
using LocalizationTool;
using Player;
using UnityEngine;
using Upgrade;

namespace Buildings
{
    public class MineBuilding : Building, IResourceProvider
    {
        [SerializeField] private ResourceType mineType;

        private float _storedResources = 0f;
        private float _extractionTimer = 0f;
        private float _extractionInterval = 1f;

        private MineUpgradeState Mine => (MineUpgradeState)upgradeState;

        public ResourceType MineType => mineType;

        public override void Start()
        {
            base.Start();

            if (upgradeState is MineUpgradeState mine)
            {
                mine.MineType = mineType;
            }
        }

        private void Update()
        {
            if (!IsBuilt) return;
            _extractionTimer += Time.deltaTime;
            if (_extractionTimer >= _extractionInterval)
            {
                _extractionTimer = 0f;
                ExtractResource();
            }
        }

        private void ExtractResource()
        {
            float extractionRate = Mine.ExtractionRate;
            float buffer = Mine.StorageBuffer;

            if (_storedResources < buffer)
            {
                _storedResources += extractionRate;
                _storedResources = Mathf.Min(_storedResources, buffer);
                Debug.Log($"{TypeId} → добыто: +{extractionRate} {mineType}. Текущее: {_storedResources}/{buffer}");
            }
        }
        
        public float GetAmount(ResourceType type)
        {
            return type == mineType ? _storedResources : 0f;
        }

        public bool TryTakeResource(ResourceType type, float amount)
        {
            if (type != mineType) return false;
            if (_storedResources < amount) return false;

            _storedResources -= amount;
            return true;
        }

        public override string GetUpgradeInfo()
        {
            return 
                LocalizationSystem.Format("upgradeLevel", UpgradeLevel)+ "\n" +
                LocalizationSystem.Format("miningSpeed", Mine.ExtractionRate)+ "\n" +
                LocalizationSystem.Format("buffer", Mine.StorageBuffer);
        }


        protected override void OnUpgraded()
        {
            Mine.Upgrade();
            base.OnUpgraded();
        }
        
        public override void OnInteract()
        {
            base.OnInteract();
            var dialog = DialogManager.ShowDialog<BuildingUpgradeDialog>();
            dialog.Initialize(this);
        }
    }
}
