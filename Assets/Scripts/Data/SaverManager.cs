using Buildings;
using Interface;
using Player;
using UnityEngine;
using Upgrade;

namespace Services
{
    public class SaverManager : IService
    {
        private BuildingSaver _buildingSaver;
        private readonly PlayerSaver _playerSaver;
        private readonly UpgradeSaver _upgradeSaver;
        
        public SaverManager(PlayerSaver playerSaver, UpgradeSaver upgradeSaver)
        {
            _playerSaver = playerSaver;
            _upgradeSaver = upgradeSaver;
        }

        public void Initialize()
        {
            _buildingSaver = ServicesLocator.ServiceLocator.Current.Get<BuildingManager>().Saver;
            _playerSaver.Initialize();
            _upgradeSaver.Initialize();
        }

        public void SaveAll()
        {
            _playerSaver.Save();
            _upgradeSaver.Save();
            _buildingSaver.Save();
        }
    }
}