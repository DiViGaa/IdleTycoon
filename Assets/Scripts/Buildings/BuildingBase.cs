using DialogsManager;
using DialogsManager.Dialogs;
using UnityEngine;
using Upgrade;
using ServicesLocator;

namespace Buildings
{
    public abstract class BuildingBase : MonoBehaviour, IInteractable
    {
        [SerializeField] private string buildingId;

        protected BuildingUpgradeState upgradeState;
        protected UpgradeManager upgradeManager;

        protected virtual void Start()
        {
            upgradeManager = ServiceLocator.Current.Get<UpgradeManager>();
            upgradeState = upgradeManager.GetUpgrade(buildingId);
        }

        public string BuildingId => buildingId;
        public int UpgradeLevel => upgradeState.Level;
        public int UpgradeCost => upgradeState.UpgradeCost;

        public virtual bool TryUpgrade(int playerCoins)
        {
            if (upgradeManager.TryUpgrade(buildingId, playerCoins))
            {
                upgradeState = upgradeManager.GetUpgrade(buildingId);
                OnUpgraded();
                return true;
            }

            return false;
        }

        protected virtual void OnUpgraded()
        {
            Debug.Log($"{buildingId} upgraded to level {upgradeState.Level}");
        }

        public virtual string GetUpgradeInfo()
        {
            return $"Level: {UpgradeLevel}";
        }
        
        public virtual void OnInteract()
        {
            var dialog = DialogManager.ShowDialog<BuildingUpgradeDialog>();
            dialog.Initialize(this);
        }
    }
}