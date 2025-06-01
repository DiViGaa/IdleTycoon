using Player;
using UnityEngine;

namespace Upgrade
{
    [System.Serializable]
    public class MineUpgradeState : BuildingUpgradeState
    {
        public ResourceType MineType;
    
        public float ExtractionRate = 5.0f;
        public float StorageBuffer = 50f;

        public void Upgrade()
        {
            Level++;
            ExtractionRate += 0.5f;
            StorageBuffer += 25f;
            UpgradeCost = Mathf.CeilToInt(UpgradeCost * 1.5f);
        }
    }
}