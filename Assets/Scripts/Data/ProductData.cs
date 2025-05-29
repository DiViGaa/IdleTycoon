using System;

namespace Shop
{
    [Serializable]
    public class ProductData
    {
        public string Name { get; set; }
        public string NameLocalizationKey { get; set; }
        public string Description { get; set; }
        public string DescriptionLocalizationKey { get; set; }
        public int Price { get; set; }
        public string PreliminaryPath { get; set; }
        public string PrefabPath { get; set; }
    }
}