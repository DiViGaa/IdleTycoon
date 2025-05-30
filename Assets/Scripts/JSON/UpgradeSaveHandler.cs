using System.Collections.Generic;
using Interface;
using Upgrade;

namespace JSON
{
    public class UpgradeSaveHandler : JsonFileHandler<Dictionary<string, BuildingUpgradeState>>, IService
    {
        protected override string FileName => "building_upgrades.json";
        
        public UpgradeSaveHandler()
        {
            _settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto
            };
        }

        private readonly Newtonsoft.Json.JsonSerializerSettings _settings;

        public override void Save(Dictionary<string, BuildingUpgradeState> data)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented, _settings);
            System.IO.File.WriteAllText(FilePath, json);
            UnityEngine.Debug.Log($"[UpgradeSaveHandler] Data saved to {FilePath}");
        }

        public override Dictionary<string, BuildingUpgradeState> Load()
        {
            if (!Exists())
            {
                UnityEngine.Debug.LogWarning($"[UpgradeSaveHandler] File not found. Creating default.");
                var defaultData = new Dictionary<string, BuildingUpgradeState>();
                Save(defaultData);
                return defaultData;
            }

            string json = System.IO.File.ReadAllText(FilePath);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, BuildingUpgradeState>>(json, _settings);
        }
    }
}