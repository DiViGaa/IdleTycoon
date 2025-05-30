using JSON;
using ServicesLocator;
using SoundManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DialogsManager.Dialogs
{
    public class MainMenuDialog : Dialog
    {
        [SerializeField] private Button сontinueButton;
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button exitButton;

        public void Initialize()
        {
            сontinueButton.onClick.AddListener(Сontinue);
            newGameButton.onClick.AddListener(NewGame);
            settingButton.onClick.AddListener(ShowSettings);
            exitButton.onClick.AddListener(ExitGame);

            сontinueButton.interactable = AreSavesAvailable();
        }

        private void Сontinue()
        {
            SceneManager.LoadScene("MainGame");
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
        }
        
        private void NewGame()
        {
            SceneManager.LoadScene("MainGame");
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
        }
        
        private bool AreSavesAvailable()
        {
            return ServiceLocator.Current.Get<UpgradeSaveHandler>().Exists() &&
                   ServiceLocator.Current.Get<BuildingSaveHandler>().Exists() &&
                   ServiceLocator.Current.Get<PlayerDataIO>().Exists();
        }
        
        private void DeleteAllSaves()
        {
            ServiceLocator.Current.Get<UpgradeSaveHandler>().Delete();
            ServiceLocator.Current.Get<BuildingSaveHandler>().Delete();
            ServiceLocator.Current.Get<PlayerDataIO>().Delete();
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
            сontinueButton.onClick.RemoveAllListeners();
            settingButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
        }
    }
}
