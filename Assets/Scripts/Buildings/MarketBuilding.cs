using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interface;
using Player;
using ServicesLocator;
using Upgrade;

namespace Buildings
{
    public class MarketBuilding : Building, IResourceReceiver
    {
        private Dictionary<ResourceType, float> _storage = new();

        private Player.Player _player;
        private TradeTerminalUpgrade _upgradeState;

        private void Awake()
        {
            foreach (ResourceType type in new ResourceType[] { ResourceType.Cenoxium, ResourceType.Thalorite })
            {
                _storage[type] = 0;
            }
        }

        public override void Start()
        {
            base.Start();

            _player = ServiceLocator.Current.Get<Player.Player>();

            _upgradeState = upgradeManager.GetUpgrade(BuildingId) as TradeTerminalUpgrade;
            if (_upgradeState == null)
            {
                Debug.LogWarning("MarketBuilding: апгрейд не TradeTerminalUpgrade, использую дефолтные значения");
                _upgradeState = new TradeTerminalUpgrade();
            }

            StartCoroutine(SellRoutine());
        }

        public override string GetUpgradeInfo()
        {
            return $"Level: {_upgradeState.Level}\n" +
                   $"Trade Speed: {_upgradeState.TradeSpeed:F2}x\n" +
                   $"Profit Bonus: {_upgradeState.ProfitBonusPercent}%";
        }

        public void AddResource(ResourceType type, float amount)
        {
            if (!_storage.ContainsKey(type)) return;

            _storage[type] += amount;
        }

        private IEnumerator SellRoutine()
        {
            while (true)
            {
                float interval = 1f / Mathf.Max(_upgradeState.TradeSpeed, 0.01f);
                yield return new WaitForSeconds(interval);

                float totalGoldGained = 0f;

                float cenoxiumPrice = 10f;
                float thaloritePrice = 15f;

                float profitMultiplier = 1f + (_upgradeState.ProfitBonusPercent / 100f);

                if (_storage[ResourceType.Cenoxium] > 0)
                {
                    float amountToSell = _storage[ResourceType.Cenoxium];
                    float goldEarned = amountToSell * cenoxiumPrice * profitMultiplier;
                    totalGoldGained += goldEarned;
                    _storage[ResourceType.Cenoxium] = 0;
                }

                if (_storage[ResourceType.Thalorite] > 0)
                {
                    float amountToSell = _storage[ResourceType.Thalorite];
                    float goldEarned = amountToSell * thaloritePrice * profitMultiplier;
                    totalGoldGained += goldEarned;
                    _storage[ResourceType.Thalorite] = 0;
                }

                if (totalGoldGained > 0 && _player != null)
                {
                    _player.ChangeResource(ResourceType.Coins, +totalGoldGained);
                    Debug.Log($"Market sold resources for {totalGoldGained} Gold");
                }
            }
        }
    }
}
