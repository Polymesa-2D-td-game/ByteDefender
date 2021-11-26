using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrader : MonoBehaviour
{
    [SerializeField] Path[] paths;

    private int upgradeLevel = 0;

    Tower tower;
    // Start is called before the first frame update
    void Start()
    {
        tower = GetComponent<Tower>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Upgrade(int pathIndex)
    {
        if (upgradeLevel < paths.Length)
        {
            SimpleUpgrade upgrade = paths[upgradeLevel].upgrades[pathIndex];
            tower.Power += upgrade.powerUpgrade;
            tower.Speed += upgrade.speedUpgrade;
            tower.Range += upgrade.rangeUpgrade;
            tower.EmmitForce += upgrade.emmitForceUpgrade;
            if (upgrade.spriteUpgrade)
            {
                tower.GetComponent<SpriteRenderer>().sprite = upgrade.spriteUpgrade;
            }
            if (upgrade.spawnObjectUpgrade)
            {
                tower.SpawnObject = upgrade.spawnObjectUpgrade;
            }
            tower.InitializeStats();
            tower.UpdateRangeIndicator();
            upgradeLevel++;
        }
    }

    public int UpgradeCost(int pathIndex)
    {
        if(upgradeLevel < paths.Length)
        {
            return paths[upgradeLevel].upgrades[pathIndex].upgradeCost;
        }
        return 0;
    }

    public string GetUpgradeName(int pathIndex)
    {
        if (upgradeLevel < paths.Length)
        {
            return paths[upgradeLevel].upgrades[pathIndex].upgradeName;
        }
        return "FULL";
            
    }
    public string GetUpgradeDescription(int pathIndex)
    {
        if (upgradeLevel < paths.Length)
        {
            return paths[upgradeLevel].upgrades[pathIndex].description;
        }
        return "FULL";
    }
}

[System.Serializable]
public class Path
{
    public SimpleUpgrade[] upgrades;
}
