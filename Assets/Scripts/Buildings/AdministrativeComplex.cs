using Upgrade;

namespace Buildings
{
    public class AdministrativeComplex : Building
    {
        private AdministrativeComplexUpgrade AdminUpgrade => (AdministrativeComplexUpgrade)upgradeState;

        public override string GetUpgradeInfo()
        {
            return $"Level: {UpgradeLevel}\n" +
                   $"Efficiency Bonus: {AdminUpgrade.EfficiencyBonus}%\n" +
                   $"Max Personnel: {AdminUpgrade.MaxPersonnel}";
        }

        protected override void OnUpgraded()
        {
            AdminUpgrade.Upgrade();
            base.OnUpgraded();
        }
    }
}