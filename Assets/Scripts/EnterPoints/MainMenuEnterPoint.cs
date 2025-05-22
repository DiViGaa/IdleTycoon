using DialogsManager;
using DialogsManager.Dialogs;
using JSON;
using ServicesLocator;
using Settings;
using SoundManager;
using UnityEngine;

namespace EnterPoints
{
    public class MainMenuEnterPoint : MonoBehaviour
    {
        [SerializeField] private GUIHolder guiHolder;
        
        private SettingManager _settingManager;
        private AudioManager _audioManager;
        private JsonSetting _jsonSetting;
        
        private MainMenuDialog _mainMenuDialog;
        
        private void Start()
        {
            _jsonSetting = new JsonSetting();
            _audioManager = new AudioManager();
            _settingManager = new SettingManager(_audioManager);
            
            Register();
            CreateMainMenuDialog();
            Initialize();
        }

        private void Initialize()
        {
            _settingManager.Initialize();
        }

        private void CreateMainMenuDialog()
        {
            _mainMenuDialog = DialogManager.ShowDialog<MainMenuDialog>();
            _mainMenuDialog.Initialize();
        }

        private void Register()
        {
            ServiceLocator.Initialize();
        
            ServiceLocator.Current.Register<GUIHolder>(guiHolder);
            ServiceLocator.Current.Register<JsonSetting>(_jsonSetting);
            ServiceLocator.Current.Register<AudioManager>(_audioManager);
            ServiceLocator.Current.Register<SettingManager>(_settingManager);
        }
        
        private void OnDisable() => Dispose();

        public void Dispose()
        {
            _mainMenuDialog.Dispose();
        }
    }
}
