using Services;
using ServicesLocator;
using SoundManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DialogsManager.Dialogs
{
    public class GamePauseDialog : Dialog
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button exitButton;

        public void Initialize()
        {
            resumeButton.onClick.AddListener(Resume);
            saveButton.onClick.AddListener(Save);
            settingButton.onClick.AddListener(ShowSettings);
            exitButton.onClick.AddListener(ExitGame);
        }

        private void Save()
        {
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
            ServiceLocator.Current.Get<SaverManager>().SaveAll();
        }

        private void Resume()
        {
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
            var dialog = DialogManager.ShowDialog<GameUIDialog>();
            dialog.Initialize();
            Hide();
        }

        private void ShowSettings()
        {
            var dialog = DialogManager.ShowDialog<GameSettingDialog>();
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
            dialog.Initialize();
            Hide();
        }

        private void ExitGame()
        {
            SceneManager.LoadScene("Menu");
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
        }

        public override void Dispose()
        {
            resumeButton.onClick.RemoveAllListeners();
            settingButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
        }
    }
}
