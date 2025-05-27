using Settings;

namespace JSON
{
    public class JsonSetting : JsonFileHandler<SettingData>, Interface.IService
    {
        protected override string FileName => "settings_data.json";

        public override SettingData Load()
        {
            if (!Exists())
            {
                var defaultSettings = new SettingData
                {
                    LanguageIndex = 0,
                    MusicVolume = 0,
                    UIVolume = 0
                };

                Save(defaultSettings);
                return defaultSettings;
            }

            return base.Load();
        }
    }
}