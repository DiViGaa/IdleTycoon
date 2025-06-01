using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace JSON
{
    public abstract class JsonFileHandler<T> where T : new()
    {
        protected abstract string FileName { get; }

        protected string FilePath => Path.Combine(Application.persistentDataPath, FileName);

        public virtual void Save(T data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public virtual T Load()
        {
            if (!File.Exists(FilePath))
            {
                T defaultData = new T();
                Save(defaultData);
                return defaultData;
            }

            string json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public virtual void Delete()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }

        public bool Exists() => File.Exists(FilePath);
    }
}