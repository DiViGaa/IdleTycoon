namespace Buildings
{
    public class AdministrativeComplex : BuildingBase
    {
        public override string GetUpgradeInfo()
        {
            return $"Level: {UpgradeLevel}\nEfficiency: {1 + 0.1f * (UpgradeLevel - 1):P0}\nUnlocks personnel management.";
        }
    }
}