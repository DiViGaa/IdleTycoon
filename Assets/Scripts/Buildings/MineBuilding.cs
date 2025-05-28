namespace Buildings
{
    public class MineBuilding : Building
    {
        private MineUpgradeState Mine => (MineUpgradeState)upgradeState;

        public override string GetUpgradeInfo()
        {
            return $"Level: {UpgradeLevel}\n" +
                   $"Mining Speed: {Mine.ExtractionRate}\n" +
                   $"Storage Buffer: {Mine.StorageBuffer}";
        }

        protected override void OnUpgraded()
        {
            Mine.Upgrade();
            base.OnUpgraded();
        }
    }
}
