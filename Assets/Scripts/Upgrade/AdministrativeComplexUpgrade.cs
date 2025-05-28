using UnityEngine;

namespace Upgrade
{
    [System.Serializable]
    public class AdministrativeComplexUpgrade : BuildingUpgradeState
    {
        public int EfficiencyBonus = 5;
        public int MaxPersonnel = 10;

        public void Upgrade()
        {
            Level++;
            EfficiencyBonus += 2;
            MaxPersonnel += 5;
            UpgradeCost = Mathf.CeilToInt(UpgradeCost * 1.4f);
        }
    }
}