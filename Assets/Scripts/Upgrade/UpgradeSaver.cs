using Interface;
using JSON;
using UnityEngine;
using Upgrade;

namespace Services
{
    public class UpgradeSaver : IService
    {
        private UpgradeManager _manager;
        private UpgradeSaveHandler _saveHandler;

        public UpgradeSaver(UpgradeManager manager, UpgradeSaveHandler saveHandler)
        {
            _manager = manager;
            _saveHandler = saveHandler;
        }

        public void Initialize()
        {
            _saveHandler ??= ServicesLocator.ServiceLocator.Current.Get<UpgradeSaveHandler>();
            _manager ??= ServicesLocator.ServiceLocator.Current.Get<UpgradeManager>();
        }

        public void Save()
        {
            var states = _manager.GetAllStates();
            _saveHandler.Save(states);
            Debug.Log("[UpgradeSaver] Saved upgrade data.");
        }

        public void Load()
        {
            var states = _saveHandler.Load();
            _manager.SetAllStates(states);
            Debug.Log("[UpgradeSaver] Loaded upgrade data.");
        }
    }
}