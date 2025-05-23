using ServicesLocator;
using Settings;
using SoundManager;
using UnityEngine;
using UnityEngine.UI;

namespace DialogsManager.Dialogs
{
    public class SettingDialog : Dialog
    {
        [SerializeField] protected Button uaButton;
        [SerializeField] protected Button enButton;
        [SerializeField] protected Button ruButton;
        [SerializeField] protected Button backToMenuButton;
        
        [SerializeField] protected Slider uiSlider;
        [SerializeField] protected Slider musicSlider;

        public void Initialize()
        {
            uaButton.onClick.AddListener(SwitchToUaButton);
            enButton.onClick.AddListener(SwitchToEnButton);
            ruButton.onClick.AddListener(SwitchToRuButton);
            backToMenuButton.onClick.AddListener(BackToMenu);
            
            SetSliderVolume();
            
            uiSlider.onValueChanged.AddListener(ChangeUiSlider);
            musicSlider.onValueChanged.AddListener(ChangeMusicSlider);
        }

        private void ChangeUiSlider(float value)
        {
            ServiceLocator.Current.Get<SettingManager>().SetUiVolume(value);
        }

        private void ChangeMusicSlider(float value)
        {
            ServiceLocator.Current.Get<SettingManager>().SetMusicVolume(value);
        }

        private void SetSliderVolume()
        {
            uiSlider.value = ServiceLocator.Current.Get<AudioManager>().UIVolume;
            musicSlider.value = ServiceLocator.Current.Get<AudioManager>().MusicVolume;
        }

        private void SwitchToUaButton()
        {
            ServiceLocator.Current.Get<SettingManager>().SwitchLanguage(0);
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
            ReloadSettings();
        }
        
        private void SwitchToEnButton()
        {
            ServiceLocator.Current.Get<SettingManager>().SwitchLanguage(1);
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
            ReloadSettings();
        }
        
        private void SwitchToRuButton()
        {
            ServiceLocator.Current.Get<SettingManager>().SwitchLanguage(2);
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
            ReloadSettings();
        }

        protected virtual void ReloadSettings()
        {
            Hide();
        }

        protected virtual void BackToMenu()
        {
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
            ServiceLocator.Current.Get<SettingManager>().Save();
            Hide();
        }

        public override void Dispose()
        {
            uaButton.onClick.RemoveListener(SwitchToUaButton);
            enButton.onClick.RemoveListener(SwitchToEnButton);
            ruButton.onClick.RemoveListener(SwitchToRuButton);
            backToMenuButton.onClick.RemoveListener(BackToMenu);
            musicSlider.onValueChanged.RemoveListener(ChangeMusicSlider);
            uiSlider.onValueChanged.RemoveListener(ChangeUiSlider);
        }
    }
}
