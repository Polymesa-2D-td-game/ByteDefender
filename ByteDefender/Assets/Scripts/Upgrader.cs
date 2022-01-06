using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrader : MonoBehaviour
{
    [SerializeField] Path[] paths;

    private int leftIndex = 0;
    private int rightIndex = 0;

    private int max;

    Tower tower;
    void Start()
    {
        tower = GetComponent<Tower>();
        max = paths.Length;
    }

    //Upgrade (left - right path)
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
        //Check if max
        if (leftIndex == paths.Length || rightIndex == paths.Length)
        {
            max = paths.Length - 1;
        }

        //If level index is not max
        if (levelIndex < max)
        {
            //Perform selected Upgrade
            SimpleUpgrade upgrade = paths[levelIndex].upgrades[pathIndex];
            tower.Power += upgrade.powerUpgrade;
            tower.Speed += upgrade.speedUpgrade;
            tower.Range += upgrade.rangeUpgrade;
            tower.EmmitForce += upgrade.emmitForceUpgrade;
            //If upgrade has a new sprite update it
            if (upgrade.spriteUpgrade)
            {
                tower.GetComponent<SpriteRenderer>().sprite = upgrade.spriteUpgrade;
            }
            //If upgrade has a new projectile update it
            if (upgrade.spawnObjectUpgrade)
            {
                tower.SpawnObject = upgrade.spawnObjectUpgrade;
            }
            //if one path is reached max. Reduce max
            if(levelIndex == max-1)
            {
                max = max - 1;
            }
            //Initialize stats and update range indicator
            tower.InitializeStats();
            tower.UpdateRangeIndicator();
        }
        Debug.Log(levelIndex);
    }

    //Return the cost of the selected upgrade
    public int UpgradeCost(int pathIndex)
    {
        
        if (pathIndex == 0)
        {
            if (leftIndex < max)
            {
                return paths[leftIndex].upgrades[pathIndex].upgradeCost;
            }
            return 0;
        }
        else
        {
            if (rightIndex < max)
            {
                return paths[rightIndex].upgrades[pathIndex].upgradeCost;
            }
            return 0;
        }
    }

    //Return the name of the selected upgrade
    public string GetUpgradeName(int pathIndex)
    {
        if (pathIndex == 0)
        {
            if (leftIndex < max)
            {
                return paths[leftIndex].upgrades[pathIndex].upgradeName + " (" + (int)(leftIndex + 1) + ")";
            }
            return "FULL";
        }
        else
        {
            if (rightIndex < max)
            {
                return paths[rightIndex].upgrades[pathIndex].upgradeName + " (" + (int)(rightIndex + 1) + ")";
            }
            return "FULL";
        }
    }

    //Return the descruption of the selected upgrade
    public string GetUpgradeDescription(int pathIndex)
    {
        if (pathIndex == 0)
        {
            if (leftIndex < max)
            {
                return paths[leftIndex].upgrades[pathIndex].description;
            }
            return "FULL";
        }
        else
        {
            if (rightIndex < max)
            {
                return paths[rightIndex].upgrades[pathIndex].description;
            }
            return "FULL";
        }
    }

    //A class that stores each upgrade for all paths
    [System.Serializable]
    public class Path
    {
        public SimpleUpgrade[] upgrades;
    }
}