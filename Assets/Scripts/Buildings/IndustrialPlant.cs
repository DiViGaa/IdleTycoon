namespace Buildings
{
    public class IndustrialPlant : BuildingBase
    {
        public override string GetUpgradeInfo()
        {
            return $"Level: {UpgradeLevel}\nProduction Efficiency: {1 + 0.25f * (UpgradeLevel - 1):P0}";
        }
    }
}