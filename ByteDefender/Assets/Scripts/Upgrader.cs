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
        if (leftIndex == paths.Length || rightIndex == paths.Length)
        {
            max = paths.Length - 1;
        }

        if (levelIndex < max)
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
            tower.InitializeStats();
            tower.UpdateRangeIndicator();
        }
        Debug.Log(levelIndex);
    }

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

    [System.Serializable]
    public class Path
    {
        public SimpleUpgrade[] upgrades;
    }
}