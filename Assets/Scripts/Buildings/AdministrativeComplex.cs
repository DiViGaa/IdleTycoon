using DialogsManager;
using DialogsManager.Dialogs;
using LocalizationTool;
using Upgrade;

namespace Buildings
{
    public class AdministrativeComplex : Building
    {
        private AdministrativeComplexUpgrade AdminUpgrade => (AdministrativeComplexUpgrade)upgradeState;

        public override string GetUpgradeInfo()
        {
            return LocalizationSystem.Format("upgradeLevel", UpgradeLevel)+ "\n" +
                LocalizationSystem.Format("efficiencyBonus", AdminUpgrade.EfficiencyBonus)+ "\n" +
                LocalizationSystem.Format("maxPersonnel", AdminUpgrade.MaxPersonnel);

        }
        
        protected override void OnUpgraded()
        {
            AdminUpgrade.Upgrade();
            base.OnUpgraded();
        }
        
        public override void OnInteract()
        {
            base.OnInteract();
            var dialog = DialogManager.ShowDialog<BuildingUpgradeDialog>();
            dialog.Initialize(this);
        }
    }
}