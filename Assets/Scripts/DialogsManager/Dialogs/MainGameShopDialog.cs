using ServicesLocator;
using Shop;
using SoundManager;
using UnityEngine;
using UnityEngine.UI;

namespace DialogsManager.Dialogs
{
    public class MainGameShopDialog : Dialog
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private GameObject content;

        public void Initialize()
        {
            closeButton.onClick.AddListener(CloseShop);
            ServiceLocator.Current.Get<ProductCreator>().CreateProducts(content);
        }

        private void CloseShop()
        {
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
            var dialog = DialogManager.ShowDialog<GameUIDialog>();
            dialog.Initialize();
            Hide();
        }
    }
}
