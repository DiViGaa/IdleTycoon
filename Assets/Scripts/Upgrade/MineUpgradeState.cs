using UnityEngine;
using Upgrade;

public enum MineType
{
    Crysalor,
    Velorith,
    Zenthite
}

[System.Serializable]
public class MineUpgradeState : BuildingUpgradeState
{
    public MineType MineType;
    
    public float ExtractionRate = 1.0f;
    public float StorageBuffer = 50f;

    public void Upgrade()
    {
        Level++;
        ExtractionRate += 0.5f;
        StorageBuffer += 25f;
        UpgradeCost = Mathf.CeilToInt(UpgradeCost * 1.5f);
    }
}