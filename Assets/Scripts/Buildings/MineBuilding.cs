using Player;
using UnityEngine;

namespace Buildings
{
    public class MineBuilding : Building
    {
        [SerializeField] private ResourceType mineType;

        private float _timer;
        private float _storedResources;
        private float _extractionInterval = 1f;

        private MineUpgradeState Mine => (MineUpgradeState)upgradeState;

        public override void Start()
        {
            base.Start();

            if (upgradeState is MineUpgradeState mine)
            {
                mine.MineType = mineType;
            }
            else
            {
                Debug.LogError($"{BuildingId} has invalid upgrade state type.");
            }
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _extractionInterval)
            {
                _timer = 0;
                ExtractResource();
            }

            if (_storedResources >= Mine.StorageBuffer)
            {
                ServicesLocator.ServiceLocator.Current.Get<Player.Player>().ChangeResource(mineType, +Collect());
            }
        }

        private void ExtractResource()
        {
            if (_storedResources < Mine.StorageBuffer)
            {
                _storedResources += Mine.ExtractionRate;
                _storedResources = Mathf.Min(_storedResources, Mine.StorageBuffer);

                Debug.Log($"{BuildingId} Mine {Mine.MineType}: +{Mine.ExtractionRate} | " +
                          $"Save: {_storedResources}/{Mine.StorageBuffer}");
            }
        }

        public float Collect()
        {
            float collected = _storedResources;
            _storedResources = 0;
            return collected;
        }

        public float GetStoredResources() => _storedResources;

        public override string GetUpgradeInfo()
        {
            return $"Level: {UpgradeLevel}\n" +
                   $"Mining Speed: {Mine.ExtractionRate}\n" +
                   $"Storage Buffer: {Mine.StorageBuffer}";
        }

        protected override void OnUpgraded()
        {
            Mine.Upgrade();
            base.OnUpgraded();
        }
    }
}
