using UnityEngine;

namespace Upgrade
{
    [System.Serializable]
    public class LogisticsCenterUpgrade : BuildingUpgradeState
    {
        public int StorageCapacity = 100;
        public float DistributionSpeed = 1.0f;

        public void Upgrade()
        {
            Level++;
            StorageCapacity += 50;
            DistributionSpeed += 0.1f;
            UpgradeCost = Mathf.CeilToInt(UpgradeCost * 1.5f);
        }
    }
}