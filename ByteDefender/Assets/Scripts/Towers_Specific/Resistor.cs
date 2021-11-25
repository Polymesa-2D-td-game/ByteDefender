using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tower))]
public class Resistor : MonoBehaviour
{

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
                    enemy.GetComponent<Enemy>().SlowEnemy(tower.CurrentPower, tower.EmmitForce);
                }
            }
            yield return new WaitForSeconds(1/tower.CurrentSpeed);
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
                    enemy.GetComponent<Enemy>().StunEnemy(tower.EmmitForce);
                }
            }  
            yield return new WaitForSeconds(1 / tower.CurrentSpeed);
        }
    }
}
