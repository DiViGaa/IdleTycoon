namespace Buildings
{
    public class LogisticsCenter : Building
    {
        public override string GetUpgradeInfo()
        {
            return $"Level: {UpgradeLevel}\nStorage Capacity: {1000 * UpgradeLevel} units";
        }
    }
}