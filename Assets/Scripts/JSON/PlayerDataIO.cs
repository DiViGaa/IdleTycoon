using System.IO;
using Newtonsoft.Json;
using Player;
using UnityEngine;

namespace JSON
{
    public static class PlayerDataIO
    {
        private static readonly string FileName = "player_data.json";
        private static string FilePath => Path.Combine(Application.persistentDataPath, FileName);

        public static void Save(PlayerData data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(FilePath, json);
            Debug.Log($"[SAVE] Player data saved to: {FilePath}");
        }

        public static PlayerData Load()
        {
            if (!File.Exists(FilePath))
            {
                Debug.LogWarning("[LOAD] Save file not found, creating new PlayerData and saving it.");
                var newData = new PlayerData()
                {
                    Cenoxium = 0,
                    Coins = 5700,
                    Crysalor = 0,
                    Thalorite = 0,
                    Velorith = 0,
                    Zenthite = 0
                };
                Save(newData);
                return newData;
            }

            string json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<PlayerData>(json);
        }

        public static void DeleteSave()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
                Debug.Log("[DELETE] Save file deleted.");
            }
        }

        public static bool SaveExists() => File.Exists(FilePath);
    }
}
