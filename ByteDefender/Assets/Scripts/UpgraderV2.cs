using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgraderV2 : MonoBehaviour
{
    [SerializeField] Path[] paths;

    private int leftIndex = 0;
    private int rightIndex = 0;

    Tower tower;
    void Start()
    {
        tower = GetComponent<Tower>();
    }

    public void Upgrade(int pathIndex)
    {
        switch (pathIndex)
        {
            case 0:
                ApplyUpgrade(leftIndex, 0);
                leftIndex++;
                break;
            case 1:
                ApplyUpgrade(rightIndex, 1);
                rightIndex++;
                break;
            default:
                break;
        }
    }
    
    private void ApplyUpgrade(int levelIndex, int pathIndex)
    {
        if(levelIndex < paths.Length)
        {
            SimpleUpgrade upgrade = paths[levelIndex].upgrades[pathIndex];
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
            if(levelIndex == 1)
            {
                Debug.Log("<OGM");
                paths[paths.Length -1].upgrades[0].description = "";
                paths[paths.Length -1].upgrades[0].name = "";
                paths[paths.Length - 1].upgrades[0].upgradeCost = 0;
            }
            tower.InitializeStats();
            tower.UpdateRangeIndicator();
        }
    }

    public int UpgradeCost(int pathIndex)
    {
        if(pathIndex == 0)
        {
            if (leftIndex < paths.Length)
            {
                return paths[leftIndex].upgrades[pathIndex].upgradeCost;
            }
            return 0;
        }
        else
        {
            if (rightIndex < paths.Length)
            {
                return paths[rightIndex].upgrades[pathIndex].upgradeCost;
            }
            return 0;
        }
        
    }

    public string GetUpgradeName(int pathIndex)
    {
        if (pathIndex == 0)
        {
            if (leftIndex < paths.Length)
            {
                return paths[leftIndex].upgrades[pathIndex].upgradeName;
            }
            return "FULL";
        }
        else
        {
            if (rightIndex < paths.Length)
            {
                return paths[rightIndex].upgrades[pathIndex].upgradeName;
            }
            return "FULL";
        }
    }

    public string GetUpgradeDescription(int pathIndex)
    {
        if (pathIndex == 0)
        {
            if (leftIndex < paths.Length)
            {
                return paths[leftIndex].upgrades[pathIndex].description;
            }
            return "FULL";
        }
        else
        {
            if (rightIndex < paths.Length)
            {
                return paths[rightIndex].upgrades[pathIndex].description;
            }
            return "FULL";
        }
    }

    [System.Serializable]
    public class Path
    {
        public SimpleUpgrade[] upgrades;
    }
}
