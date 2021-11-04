using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tower))]
public class Resistor : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] private float slowRatio = 0.2f;
    [SerializeField] private float debuffTime = 0.5f;

    private Tower tower;
    // Start is called before the first frame update
    private void Awake()
    {
        tower = GetComponent<Tower>();
        StartCoroutine(SlowAttack());
    }

    // Update is called once per frame
    IEnumerator SlowAttack()
    {
        while(true)
        {
            if(tower.IsEnabled())
            {
                foreach (GameObject enemy in tower.GetTargetsInRange())
                {
                    enemy.GetComponent<Enemy>().SlowEnemy(slowRatio, debuffTime);
                }
            }
            yield return new WaitForSeconds(1/tower.Speed);
        }
    }

    IEnumerator StunAttack()
    {
        while (true)
        {
            if (tower.IsEnabled())
            {
                foreach (GameObject enemy in tower.GetTargetsInRange())
                {
                    enemy.GetComponent<Enemy>().StunEnemy(debuffTime);
                }
            }  
            yield return new WaitForSeconds(1 / tower.Speed);
        }
    }
}
