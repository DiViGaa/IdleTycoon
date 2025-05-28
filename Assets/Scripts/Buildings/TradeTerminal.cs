namespace Buildings
{
    public class TradeTerminal : Building
    {
        public override string GetUpgradeInfo()
        {
            return $"Level: {UpgradeLevel}\nTrade Speed: {1 + 0.15f * (UpgradeLevel - 1):P0}\nProfit Bonus: {5 * UpgradeLevel}%";
        }
    }
}