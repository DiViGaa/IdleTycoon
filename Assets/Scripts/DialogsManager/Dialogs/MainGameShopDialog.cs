using System;
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

        public static Action CloseEvent;
        
        public void Initialize()
        {
            closeButton.onClick.AddListener(CloseShop);
            ServiceLocator.Current.Get<ProductCreator>().CreateProducts(content);
            CloseEvent += Close;
        }

        private void CloseShop()
        {
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
            var dialog = DialogManager.ShowDialog<GameUIDialog>();
            dialog.Initialize();
            Hide();
        }

        private void Close()
        {
            Hide();
        }

        public override void Dispose()
        {
            CloseEvent -= Close;
            base.Dispose();
        }
    }
}
