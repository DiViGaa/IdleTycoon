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

        public void Initialize(ProductData product)
        {
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
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
        }
    }
}
