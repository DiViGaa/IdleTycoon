using ServicesLocator;
using SoundManager;
using UnityEngine;
using UnityEngine.UI;

namespace DialogsManager.Dialogs
{
    public class GameUIDialog : Dialog
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _shopButton;

        public void Initialize()
        {
            _pauseButton.onClick.AddListener(ShowPauseDialog);
            _shopButton.onClick.AddListener(ShowShopDialog);
        }

        private void ShowShopDialog()
        {
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
            var dialog = DialogManager.ShowDialog<MainGameShopDialog>();
            dialog.Initialize();
            Hide();
        }

        private void ShowPauseDialog()
        {
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
            var dialog = DialogManager.ShowDialog<GamePauseDialog>();
            dialog.Initialize();
            Hide();
        }

        public override void Dispose()
        {
            _pauseButton.onClick.RemoveAllListeners();
            base.Dispose();
        }
    }
}
