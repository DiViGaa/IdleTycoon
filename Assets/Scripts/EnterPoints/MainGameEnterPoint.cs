using System;
using Buildings;
using Camera;
using DialogsManager;
using DialogsManager.Dialogs;
using JSON;
using ServicesLocator;
using Settings;
using Shop;
using SoundManager;
using UnityEngine;

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
        
        public static Action OnUpdate;
        
        private void Start()
        {
            _jsonSetting = new JsonSetting();
            _audioManager = new AudioManager();
            _settingManager = new SettingManager(_audioManager);
            _productCreator = new ProductCreator();
            
            Register();
            CreateGameUIDialog();
            Initialize();
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
            _settingManager.Initialize();
            cameraMovement.Initialize();
            _productCreator.Initialize();
            buildingManager.Initialize();
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
        }
        
        private void OnDisable() => Dispose();

        public void Dispose()
        {
            _gameUIDialog.Dispose();
            cameraMovement.Dispose();
            buildingManager.Dispose();
        }
    }
}
