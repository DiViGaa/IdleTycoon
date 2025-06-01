using ServicesLocator;
using SoundManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogsManager.Dialogs
{
    public class HelpDialog : Dialog
    {
        [SerializeField] private Button closeButton;

        public void Initialize()
        {
            closeButton.onClick.AddListener(Close);
        }

        private void Close()
        {
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
            var dialog = DialogManager.ShowDialog<GamePauseDialog>();
            dialog.Initialize();
            Hide();
        }

        public override void Dispose()
        {
            closeButton.onClick.RemoveListener(Close);
            base.Dispose();
        }
    }
}
