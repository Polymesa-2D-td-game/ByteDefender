using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tower))]
public class CoilGun : MonoBehaviour
{ 
    private Tower tower;
    // Start is called before the first frame update
    private void Awake()
    {
        tower = GetComponent<Tower>();
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        while (true)
        {
            if(tower.GetTarget() && tower.IsEnabled())
            {
                tower.LookAtTarget();
                GameObject projectile = Instantiate(tower.SpawnObject, tower.SpawnPoint.position, tower.SpawnPoint.rotation) as GameObject;
                tower.PlaySoundEffect();
                projectile.GetComponent<CoilGunProjectile>().Damage = tower.CurrentIntPower;
                projectile.GetComponent<CoilGunProjectile>().EmmitForce = tower.EmmitForce;
                projectile.GetComponent<CoilGunProjectile>().Target = tower.GetTarget();
            }
            yield return new WaitForSeconds(1 / tower.CurrentSpeed);
        }
    }
}
