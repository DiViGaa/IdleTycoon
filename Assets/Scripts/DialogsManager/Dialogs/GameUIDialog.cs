using ServicesLocator;
using SoundManager;
using UnityEngine;
using UnityEngine.UI;

namespace DialogsManager.Dialogs
{
    public class GameUIDialog : Dialog
    {
        [SerializeField] private Button _pauseButton;

        public void Initialize()
        {
            _pauseButton.onClick.AddListener(ShowPauseDialog);
        }

        private void ShowPauseDialog()
        {
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
            var dialog = DialogManager.ShowDialog<GamePauseDialog>();
            dialog.Initialize();
        }

        public override void Dispose()
        {
            _pauseButton.onClick.RemoveAllListeners();
            base.Dispose();
        }
    }
}
