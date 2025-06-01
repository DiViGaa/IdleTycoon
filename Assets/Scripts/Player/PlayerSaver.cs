using Interface;
using JSON;

namespace Player
{
    public class PlayerSaver : IService
    {
        private PlayerDataIO _saveHandler;
        private Player _player;

        public void Initialize()
        {
            _saveHandler = ServicesLocator.ServiceLocator.Current.Get<PlayerDataIO>();
        }

        public void SetPlayer(Player player)
        {
            _player = player;
        }

        public void Save()
        {
            _saveHandler.Save(_player.Data);
        }

        public void Load()
        {
            var loadedData = _saveHandler.Load();
            _player.SetData(loadedData);
        }
    }
}