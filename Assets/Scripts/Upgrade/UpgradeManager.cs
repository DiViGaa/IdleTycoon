using System.Collections.Generic;
using System.IO;
using Interface;
using Newtonsoft.Json;
using UnityEngine;

namespace Upgrade
{
    public class UpgradeManager : IService
    {
        private Dictionary<string, BuildingUpgradeState> _upgradeStates = new();
        
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

        private const string FileName = "building_upgrades.json";
        private string FilePath => Path.Combine(Application.persistentDataPath, FileName);

        public void Initialize()
        {
            Load();
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
                    state = new BuildingUpgradeState(); // fallback
                }

                _upgradeStates[buildingId] = state;
                Save();
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
                Save();
                return true;
            }

            return false;
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(_upgradeStates, Formatting.Indented);
            File.WriteAllText(FilePath, json);
            Debug.Log($"[UpgradeManager] Data saved to {FilePath}");
        }

        public void Load()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                _upgradeStates = JsonConvert.DeserializeObject<Dictionary<string, BuildingUpgradeState>>(json);
                Debug.Log("[UpgradeManager] Data loaded.");
            }
            else
            {
                _upgradeStates = new Dictionary<string, BuildingUpgradeState>();
                Save(); // создаем новый файл
                Debug.Log("[UpgradeManager] New upgrade save file created.");
            }
        }

        public void ResetAll()
        {
            _upgradeStates.Clear();
            Save();
        }
    }
}
