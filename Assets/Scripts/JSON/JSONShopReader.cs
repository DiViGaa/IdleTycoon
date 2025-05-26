using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Shop;
using UnityEngine;

namespace JSON
{
    public class JsonShopReader
    {
        public static List<ProductData> LoadJson(string fileName)
        {
            string url = string.Empty;
            url = Application.dataPath + "/Resources/JSON/" + fileName;

            string json = File.ReadAllText(url);
            
            List<ProductData> upgrades = JsonConvert.DeserializeObject<List<ProductData>>(json);
            return upgrades;
        }
    }
}
