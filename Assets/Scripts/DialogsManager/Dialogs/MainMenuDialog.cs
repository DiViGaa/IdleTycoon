using ServicesLocator;
using SoundManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DialogsManager.Dialogs
{
    public class MainMenuDialog : Dialog
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button exitButton;

        public void Initialize()
        {
            playButton.onClick.AddListener(Play);
            settingButton.onClick.AddListener(ShowSettings);
            exitButton.onClick.AddListener(ExitGame);
        }

        private void Play()
        {
            SceneManager.LoadScene("MainGame");
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
        }

        private void ShowSettings()
        {
            var dialog = DialogManager.ShowDialog<MainMenuSettingDialog>();
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
            dialog.Initialize();
            Hide();
        }

        private void ExitGame()
        {
            Application.Quit();
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
        }

        public override void Dispose()
        {
            playButton.onClick.RemoveAllListeners();
            settingButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
        }
    }
}
