using System.Collections;
using System.Collections.Generic;
using DialogsManager;
using DialogsManager.Dialogs;
using Interface;
using LocalizationTool;
using Player;
using ServicesLocator;
using UnityEngine;
using Upgrade;

namespace Buildings
{
    public class MarketBuilding : Building, IResourceReceiver
    {
        private Dictionary<ResourceType, float> _storage = new();

        private Player.Player _player;
        private Coroutine _sellCoroutine;

        private MarketUpgrade Terminal => (MarketUpgrade)upgradeState;

        private void Awake()
        {
            foreach (ResourceType type in new[] { ResourceType.Cenoxium, ResourceType.Thalorite })
            {
                _storage[type] = 0;
            }
            
            _player = ServiceLocator.Current.Get<Player.Player>();
        }

        public override string GetUpgradeInfo()
        {
            return
                LocalizationSystem.Format("upgradeLevel", Terminal.Level) + "\n" +
                LocalizationSystem.Format("tradeSpeed", Terminal.TradeSpeed.ToString("F2")) + "\n" +
                LocalizationSystem.Format("profitBonus", Terminal.ProfitBonusPercent);
        }

        public void AddResource(ResourceType type, float amount)
        {
            if (_storage.ContainsKey(type))
            {
                _storage[type] += amount;
            }
        }

        protected override void OnPlacementFinalized()
        {
            base.OnPlacementFinalized();

            if (_sellCoroutine == null)
                _sellCoroutine = StartCoroutine(SellRoutine());
        }

        private IEnumerator SellRoutine()
        {
            while (upgradeState == null)
                yield return new WaitForSeconds(1f);
            while (true)
            {
                float interval = 1f / Mathf.Max(Terminal.TradeSpeed, 0.01f);
                yield return new WaitForSeconds(interval);

                float totalGoldGained = 0f;

                const float cenoxiumPrice = 1000f;
                const float thaloritePrice = 1200f;
                float profitMultiplier = 1f + (Terminal.ProfitBonusPercent / 100f);

                if (_storage[ResourceType.Cenoxium] > 0)
                {
                    float amount = _storage[ResourceType.Cenoxium];
                    totalGoldGained += amount * cenoxiumPrice * profitMultiplier;
                    _storage[ResourceType.Cenoxium] = 0;
                }

                if (_storage[ResourceType.Thalorite] > 0)
                {
                    float amount = _storage[ResourceType.Thalorite];
                    totalGoldGained += amount * thaloritePrice * profitMultiplier;
                    _storage[ResourceType.Thalorite] = 0;
                }

                if (totalGoldGained > 0 && _player != null)
                {
                    _player.ChangeResource(ResourceType.Coins, totalGoldGained);
                }
            }
        }

        protected override void OnUpgraded()
        {
            Terminal.Upgrade();
            base.OnUpgraded();
        }

        private void OnDisable()
        {
            if (_sellCoroutine != null)
            {
                StopCoroutine(_sellCoroutine);
                _sellCoroutine = null;
            }
        }

        public override void OnInteract()
        {
            base.OnInteract();
            var dialog = DialogManager.ShowDialog<BuildingUpgradeDialog>();
            dialog.Initialize(this);
        }
    }
}
