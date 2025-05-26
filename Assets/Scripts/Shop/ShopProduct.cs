using Buildings;
using DialogsManager;
using DialogsManager.Dialogs;
using EnterPoints;
using LocalizationTool;
using ServicesLocator;
using SoundManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopProduct : MonoBehaviour
    {
        [SerializeField] private Image productImage;
        [SerializeField] private TextMeshProUGUI productName;
        [SerializeField] private TextMeshProUGUI productPrice;
        [SerializeField] private TextMeshProUGUI productDescription;
        
        [SerializeField] private Button buyButton;
        
        [SerializeField] private TextLocaliserUI nameLocalizationKey;
        [SerializeField] private TextLocaliserUI descriptionLocalizationKey;
        
        private ProductData _productData;

        public void Initialize(ProductData product)
        {
            _productData = product;
            productImage.sprite =  Resources.Load<Sprite>(product.PreliminaryPath);
            productName.text = product.Name;
            productPrice.text = product.Price.ToString();
            productDescription.text = product.Description;
            nameLocalizationKey.SetKey(product.NameLocalizationKey);
            descriptionLocalizationKey.SetKey(product.DescriptionLocalizationKey);
            
            buyButton.onClick.AddListener(Buy);
        }

        private void Buy()
        {
            var prefab =  Resources.Load<Building>(_productData.PrefabPath);
            ServiceLocator.Current.Get<BuildingManager>().StartPlacingBuilding(prefab);
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
            MainGameShopDialog.CloseEvent.Invoke();
            DialogManager.ShowDialog<GameUIDialog>().Initialize();
        }
    }
}
