using System;
using System.Collections.Generic;

namespace LocalizationTool
{
    public class LocalizationSystem
    {
        public enum Language : int
        {
            Ukrainian = 0,
            English = 1
        }
       
        private static Language CurrentLanguage = Language.Ukrainian;
       
        private static Dictionary<string, string> _localizedUA;
        private static Dictionary<string, string> _localizedEN;
       
        public static bool _isInitialized = false;

        public static void Initialize()
        {
            CSVLoader loader = new CSVLoader();
            loader.LoadCsv();

            _localizedEN = loader.GetDictionaryValues("en");
            _localizedUA = loader.GetDictionaryValues("ua");
           
            _isInitialized = true;
        }

        public static void SetLanguageByEnum(Language language)
        {
            CurrentLanguage  = language;
        }

        public static void SetLanguageByIndex(int index)
        {
            Array values = Enum.GetValues(typeof(Language));

            if (index >= 0 && index < values.Length)
            {
                CurrentLanguage = (Language)values.GetValue(index);
            }
        }

        public static string GetLocalizedString(string key)
        {
            if (!_isInitialized)
                Initialize();

            var value = key;
           
            switch (CurrentLanguage)
            {
                case Language.English:
                    _localizedEN.TryGetValue(key, out value);
                    break;
                case Language.Ukrainian:
                    _localizedUA.TryGetValue(key, out value);
                    break;
            }
            return value;
        }
    }
}