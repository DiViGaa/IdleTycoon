using Interface;
using JSON;

namespace Player
{
    using DialogsManager.Dialogs;
    using UnityEngine;

    public class Player: IService
    {
        public PlayerData Data { get; private set; }

        public Player(PlayerData data)
        {
            Data = data;
        }

        public void ChangeResource(ResourceType type, float amount)
        {
            switch (type)
            {
                case ResourceType.Cenoxium: Data.Cenoxium = Mathf.Max(0, Data.Cenoxium + amount); break;
                case ResourceType.Crysalor: Data.Crysalor = Mathf.Max(0, Data.Crysalor + amount); break;
                case ResourceType.Thalorite: Data.Thalorite = Mathf.Max(0, Data.Thalorite + amount); break;
                case ResourceType.Velorith: Data.Velorith = Mathf.Max(0, Data.Velorith + amount); break;
                case ResourceType.Zenthite: Data.Zenthite = Mathf.Max(0, Data.Zenthite + amount); break;
                case ResourceType.Coins: Data.Coins = Mathf.Max(0, Data.Coins + amount); break;
            }

            GameUIDialog.UpdateUI?.Invoke();
            PlayerDataIO.Save(Data);
        }

        public bool HasEnough(ResourceType type, float requiredAmount)
        {
            return GetResource(type) >= requiredAmount;
        }

        public float GetResource(ResourceType type)
        {
            return type switch
            {
                ResourceType.Cenoxium => Data.Cenoxium,
                ResourceType.Crysalor => Data.Crysalor,
                ResourceType.Thalorite => Data.Thalorite,
                ResourceType.Velorith => Data.Velorith,
                ResourceType.Zenthite => Data.Zenthite,
                ResourceType.Coins => Data.Coins,
                _ => 0
            };
        }
    }
    
    public enum ResourceType
    {
        Cenoxium,
        Crysalor,
        Thalorite,
        Velorith,
        Zenthite,
        Coins
    }
}