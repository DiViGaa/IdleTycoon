using System.Collections.Generic;
using Interface;
using JSON;
using UnityEngine;

namespace Upgrade
{
    public class UpgradeManager : IService
    {
        private Dictionary<string, BuildingUpgradeState> _upgradeStates = new();
        private readonly UpgradeSaveHandler _saveHandler = new();

        private readonly Dictionary<string, System.Type> _upgradeTypes = new()
        {
            { "MineCrysalor", typeof(MineUpgradeState) },
            { "MineVelorith", typeof(MineUpgradeState) },
            { "MineZenthite", typeof(MineUpgradeState) },
            { "Factory", typeof(FactoryUpgrade) },
            { "Office", typeof(AdministrativeComplexUpgrade) },
            { "Market", typeof(TradeTerminalUpgrade) },
            { "Storage", typeof(LogisticsCenterUpgrade) },
        };

        public void Initialize()
        {
            _upgradeStates = _saveHandler.Load();
        }

        public BuildingUpgradeState GetUpgrade(string buildingId)
        {
            if (!_upgradeStates.TryGetValue(buildingId, out var state))
            {
                if (_upgradeTypes.TryGetValue(buildingId, out var type))
                {
                    state = (BuildingUpgradeState)System.Activator.CreateInstance(type);
                }
                else
                {
                    state = new BuildingUpgradeState();
                }

                _upgradeStates[buildingId] = state;
                _saveHandler.Save(_upgradeStates);
            }

            return state;
        }

        public bool TryUpgrade(string buildingId, int playerCoins)
        {
            var upgrade = GetUpgrade(buildingId);

            if (playerCoins >= upgrade.UpgradeCost)
            {
                upgrade.Level++;
                upgrade.UpgradeCost += Mathf.RoundToInt(upgrade.UpgradeCost * 0.5f);
                _saveHandler.Save(_upgradeStates);
                return true;
            }

            return false;
        }

        public void ResetAll()
        {
            _upgradeStates.Clear();
            _saveHandler.Save(_upgradeStates);
        }
    }
}
