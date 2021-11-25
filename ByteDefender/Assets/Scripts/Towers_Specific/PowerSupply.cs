using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSupply : MonoBehaviour
{
    [SerializeField] float speedBuff;
    [SerializeField] float damageBuff;
    [SerializeField] float rangeBuff;

    List<Buffable> nearbyTowers = new List<Buffable>();

    private Tower tower;
    private Spawner spawner;
    // Start is called before the first frame update
    void Start()
    {
        tower = GetComponent<Tower>();
        StartCoroutine(Attack());
    }

    public void GetNearbyTargets()
    {
        tower = GetComponent<Tower>();
        Buffable[] buffableTowers = FindObjectsOfType<Buffable>();
        nearbyTowers.Clear();
        foreach(Buffable buffable in buffableTowers)
        {
            if(Vector2.Distance(buffable.transform.position, transform.position) < tower.CurrentRange)
            {
                nearbyTowers.Add(buffable);
            }
        }
    }
    
    public void BuffNearbyTowers()
    {
        GetNearbyTargets();
        foreach (Buffable nearby in nearbyTowers)
        {
            if(nearby)
            {
                nearby.Buff(rangeBuff, speedBuff, damageBuff);
            }
        }
    }

    IEnumerator Attack()
    {
        while (true)
        {
            foreach (Buffable nearby in nearbyTowers)
            {
                if (nearby)
                {
                    nearby.BuffAnimation();
                }
            }
            yield return new WaitForSeconds(1 / tower.CurrentSpeed);
        }
    }

    private void OnDestroy()
    {
        foreach (Buffable nearby in nearbyTowers)
        {
            if (nearby)
            {
                nearby.Debuff();
            }
        }
    }

}


