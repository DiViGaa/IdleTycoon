using System.Collections.Generic;
using Interface;
using Newtonsoft.Json;
using Shop;
using UnityEngine;

namespace JSON
{
    public class JsonShopReader:IService
    {
        private const string ResourcePath = "JSON/Product";

        public List<ProductData> LoadProducts()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>(ResourcePath);

            if (jsonFile == null)
            {
                return new List<ProductData>();
            }

            return JsonConvert.DeserializeObject<List<ProductData>>(jsonFile.text);
        }
    }
}