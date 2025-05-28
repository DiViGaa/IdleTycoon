using UnityEngine;

namespace Upgrade
{
    [System.Serializable]
    public class FactoryUpgrade : BuildingUpgradeState
    {
        public float ProcessingSpeed = 1.0f;
        public float OutputYield = 1.0f;

        public void Upgrade()
        {
            Level++;
            ProcessingSpeed += 0.2f;
            OutputYield += 0.1f;
            UpgradeCost = Mathf.CeilToInt(UpgradeCost * 1.6f);
        }
    }
}