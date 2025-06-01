using System;
using Buildings;
using Camera;
using DialogsManager;
using DialogsManager.Dialogs;
using JSON;
using Player;
using Services;
using ServicesLocator;
using Settings;
using Shop;
using SoundManager;
using UnityEngine;
using Upgrade;

namespace EnterPoints
{
    public class MainGameEnterPoint : MonoBehaviour
    {
        [SerializeField] private GUIHolder guiHolder;
        [SerializeField] private CameraMovement  cameraMovement;
        [SerializeField] private BuildingManager  buildingManager;
        
        private SettingManager _settingManager;
        private AudioManager _audioManager;
        private JsonSetting _jsonSetting;
        private ProductCreator  _productCreator;
        private GameUIDialog  _gameUIDialog;
        private Player.Player _player;
        private JsonShopReader _jsonShopReader;
        private PlayerDataIO  _playerDataIO;
        private UpgradeManager _upgradeManager;
        private BuildingClickHandler _buildingClickHandler;
        private UpgradeSaveHandler _upgradeSaveHandler;
        
        private PlayerSaver _playerSaver;
        private UpgradeSaver _upgradeSaver;
        private SaverManager _saverManager;

        
        public static Action OnUpdate;
        
        private void Start()
        {
            _jsonShopReader = new JsonShopReader();
            _jsonSetting = new JsonSetting();
            _playerDataIO = new PlayerDataIO();
            _audioManager = new AudioManager();
            _settingManager = new SettingManager(_audioManager);
            _productCreator = new ProductCreator();
            _upgradeManager = new UpgradeManager();
            _buildingClickHandler = new BuildingClickHandler();
            _upgradeSaveHandler  = new UpgradeSaveHandler();

            var playerData = _playerDataIO.Load();
            _player = new Player.Player(playerData);

            _playerSaver = new PlayerSaver();
            _playerSaver.SetPlayer(_player);
            
            _upgradeSaver = new UpgradeSaver(_upgradeManager, _upgradeSaveHandler);
            _saverManager = new SaverManager(_playerSaver, _upgradeSaver);
    
            Register();
            CreateGameUIDialog();
            Initialize();
            
            if (WelcomeDialog.ShouldShow())
            {
                DialogManager.ShowDialog<WelcomeDialog>();
            }
        }


        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void CreateGameUIDialog()
        {
            _gameUIDialog = DialogManager.ShowDialog<GameUIDialog>();
            _gameUIDialog.Initialize();
        }

        private void Initialize()
        {
            _upgradeManager.Initialize();
            buildingManager.Initialize();
            _saverManager.Initialize();
            _settingManager.Initialize();
            cameraMovement.Initialize();
            _productCreator.Initialize();
            _buildingClickHandler.Initialize();
        }
        
        private void Register()
        {
            ServiceLocator.Initialize();
        
            ServiceLocator.Current.Register<GUIHolder>(guiHolder);
            ServiceLocator.Current.Register<JsonSetting>(_jsonSetting);
            ServiceLocator.Current.Register<AudioManager>(_audioManager);
            ServiceLocator.Current.Register<SettingManager>(_settingManager);
            ServiceLocator.Current.Register<ProductCreator>(_productCreator);
            ServiceLocator.Current.Register<BuildingManager>(buildingManager);
            ServiceLocator.Current.Register<Player.Player>(_player);
            ServiceLocator.Current.Register<JsonShopReader>(_jsonShopReader);
            ServiceLocator.Current.Register<PlayerDataIO>(_playerDataIO);
            ServiceLocator.Current.Register<UpgradeManager>(_upgradeManager);
            ServiceLocator.Current.Register<BuildingClickHandler>(_buildingClickHandler);
            ServiceLocator.Current.Register<UpgradeSaveHandler>(_upgradeSaveHandler);
            ServiceLocator.Current.Register<PlayerSaver>(_playerSaver);
            ServiceLocator.Current.Register<UpgradeSaver>(_upgradeSaver);
            ServiceLocator.Current.Register<SaverManager>(_saverManager);
        }
        
        private void OnDisable() => Dispose();

        public void Dispose()
        {
            _gameUIDialog.Dispose();
            cameraMovement.Dispose();
            buildingManager.Dispose();
            _buildingClickHandler.Dispose();
        }
    }
}
