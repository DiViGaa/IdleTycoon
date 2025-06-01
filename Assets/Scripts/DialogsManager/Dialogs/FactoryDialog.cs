using Buildings;
using Player;
using ServicesLocator;
using SoundManager;
using UnityEngine;
using UnityEngine.UI;

namespace DialogsManager.Dialogs
{
    public class FactoryDialog : BuildingUpgradeDialog
    {
        [SerializeField] private Button cenoxiumButton;
        [SerializeField] private Button thaloriteButton;
        
        private FactoryBuilding _factoryBuilding;
        public override void Initialize(Building building)
        {
            _factoryBuilding = (FactoryBuilding)building;
            cenoxiumButton.onClick.AddListener(ChangeCenoxiumCraft);
            thaloriteButton.onClick.AddListener(ChangeThaloriteCraft);
            base.Initialize(building);
        }

        private void ChangeCenoxiumCraft()
        {
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
            _factoryBuilding.SetProducedResource(ResourceType.Cenoxium);
        }

        private void ChangeThaloriteCraft()
        {
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
            _factoryBuilding.SetProducedResource(ResourceType.Thalorite);
        }
    }
}
