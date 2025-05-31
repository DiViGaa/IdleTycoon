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
        
        public Dictionary<string, BuildingUpgradeState> GetAllStates()
        {
            return _upgradeStates;
        }

        public void SetAllStates(Dictionary<string, BuildingUpgradeState> states)
        {
            _upgradeStates = states;
        }


        public BuildingUpgradeState GetUpgrade(string instanceId, string typeId)
        {
            if (!_upgradeStates.TryGetValue(instanceId, out var state))
            {
                if (_upgradeTypes.TryGetValue(typeId, out var type))
                {
                    state = (BuildingUpgradeState)System.Activator.CreateInstance(type);
                }
                else
                {
                    state = new BuildingUpgradeState();
                }

                _upgradeStates[instanceId] = state;
            }

            return state;
        }

        public bool TryUpgrade(string instanceId, string typeId, int playerCoins)
        {
            var upgrade = GetUpgrade(instanceId, typeId);

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
