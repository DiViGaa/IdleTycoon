using System.IO;
using Interface;
using Newtonsoft.Json;
using Settings;
using UnityEngine;

namespace JSON
{
    public class JsonSetting: IService
    {
        private string GetFilePath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }

        public SettingData LoadJson(string fileName)
        {
            string path = GetFilePath(fileName);

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<SettingData>(json);
            }
            else
            {
                Debug.Log($"File not found, creating default settings at {path}");

                SettingData defaultSettings = new SettingData
                {
                    LanguageIndex = 0,
                    MusicVolume = 0,
                    UIVolume = 0
                };

                SaveJson(fileName, defaultSettings);

                return defaultSettings;
            }
        }

        public void SaveJson(string fileName, SettingData settingsData)
        {
            string path = GetFilePath(fileName);

            string json = JsonConvert.SerializeObject(settingsData, Formatting.Indented);

            File.WriteAllText(path, json);

            Debug.Log($"Settings saved to: {path}");
        }
    }
}