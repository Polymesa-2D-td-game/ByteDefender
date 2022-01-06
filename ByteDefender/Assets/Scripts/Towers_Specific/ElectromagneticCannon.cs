using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectromagneticCannon : MonoBehaviour
{
    [SerializeField] GameObject afterProjectilePrefab;

    [SerializeField] float afterRange;

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
            if (tower.GetTarget() && tower.IsEnabled())
            {
                tower.LookAtTarget();
                GameObject projectile = Instantiate(tower.SpawnObject, tower.SpawnPoint.position, tower.SpawnPoint.rotation) as GameObject;
                tower.PlaySoundEffect();
                projectile.GetComponent<ElectromagneticCannonProjectile>().Damage = tower.CurrentIntPower;
                projectile.GetComponent<ElectromagneticCannonProjectile>().EmmitForce = tower.EmmitForce;
                projectile.GetComponent<ElectromagneticCannonProjectile>().Target = tower.GetTarget();
                //projectile.GetComponent<ElectromagneticCannonProjectile>().range = afterRange;
                projectile.GetComponent<ElectromagneticCannonProjectile>().afterHitAttack = afterProjectilePrefab;
            }
            yield return new WaitForSeconds(1 / tower.CurrentSpeed);
        }
    }
}
