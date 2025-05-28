using UnityEngine;

namespace Upgrade
{
    [System.Serializable]
    public class TradeTerminalUpgrade : BuildingUpgradeState
    {
        public float TradeSpeed = 1.0f;
        public float ProfitBonusPercent = 5f;

        public void Upgrade()
        {
            Level++;
            TradeSpeed += 0.2f;
            ProfitBonusPercent += 2.5f;
            UpgradeCost = Mathf.CeilToInt(UpgradeCost * 1.5f);
        }
    }
}