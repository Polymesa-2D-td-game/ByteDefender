using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoder : MonoBehaviour
{
    [SerializeField] private bool canDecrypt;
    [SerializeField] private bool canUnzip;
    private Tower tower;
    private Spawner spawner;
    // Start is called before the first frame update
    void Start()
    {
        tower = GetComponent<Tower>();
    }

    public Tower[] GetNearbyTargets()
    {
        List<Tower> nearbyTowers = new List<Tower>();
        tower = GetComponent<Tower>();
        Tower[] towers = FindObjectsOfType<Tower>();
        foreach (Tower target in towers)
        {
            if (Vector2.Distance(target.transform.position, transform.position) < tower.CurrentRange)
            {
                nearbyTowers.Add(target);
            }
        }

        return nearbyTowers.ToArray();
    }

    public void BuffTowers()
    {
        Tower[] towersToBuff = GetNearbyTargets();
        if(towersToBuff.Length > 0)
        {
            for(int i = 0; i < towersToBuff.Length; i++)
            {
                if(canDecrypt)
                {
                    towersToBuff[i].canDecript = true;
                }
                if(canUnzip)
                {
                    towersToBuff[i].canUnzip = true;
                }
                
            }
        }
    }

    
}
