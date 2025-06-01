using Interface;
using Player;

namespace JSON
{
    public class PlayerDataIO : JsonFileHandler<PlayerData>, IService
    {
        protected override string FileName => "player_data.json";

        public override PlayerData Load()
        {
            if (!Exists())
            {
                var newData = new PlayerData
                {
                    Cenoxium = 0,
                    Coins = 18000,
                    Crysalor = 0,
                    Thalorite = 0,
                    Velorith = 0,
                    Zenthite = 0
                };
                Save(newData);
                return newData;
            }

            return base.Load();
        }
    }
}