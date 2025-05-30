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
        
        private UpgradeSaveHandler  _upgradeSaveHandler;
        private PlayerDataIO  _playerDataIO;
        private BuildingSaveHandler  _buildingSaveHandler;
        
        private MainMenuDialog _mainMenuDialog;
        
        private void Start()
        {
            _jsonSetting = new JsonSetting();
            _audioManager = new AudioManager();
            _settingManager = new SettingManager(_audioManager);
            _upgradeSaveHandler = new UpgradeSaveHandler();
            _playerDataIO = new PlayerDataIO();
            _buildingSaveHandler = new BuildingSaveHandler();
            
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
            
            ServiceLocator.Current.Register<UpgradeSaveHandler>(_upgradeSaveHandler);
            ServiceLocator.Current.Register<PlayerDataIO>(_playerDataIO);
            ServiceLocator.Current.Register<BuildingSaveHandler>(_buildingSaveHandler);
        }
        
        private void OnDisable() => Dispose();

        public void Dispose()
        {
            _mainMenuDialog.Dispose();
        }
    }
}
