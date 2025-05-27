using System.Collections.Generic;
using Interface;
using JSON;
using UnityEngine;

namespace Shop
{
    public class ProductCreator : IService
    {
        private List<ProductData> _productData =  new List<ProductData>();
        private string _productPath => "Prefabs/Shop/Product";

        public void Initialize()
        {
            var products = ServicesLocator.ServiceLocator.Current.Get<JsonShopReader>().LoadProducts();

            foreach (var product in products)
                _productData.Add(product);
        }

        public void CreateProducts(GameObject  productParent)
        {
            foreach (var product in _productData)
            {
                var prefab = Resources.Load<GameObject>(_productPath);
                var productPanel = prefab.GetComponent<ShopProduct>();
                var productPanelInstantiate = GameObject.Instantiate(productPanel, productParent.transform);
                productPanelInstantiate.name = "Product " + product.Name;
                productPanelInstantiate.Initialize(product);
            }
        }
    }
}
