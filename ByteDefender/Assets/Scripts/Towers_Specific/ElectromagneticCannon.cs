using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectromagneticCannon : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject afterProjectilePrefab;
    [SerializeField] int damage;
    [SerializeField] float emmitForce;
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
                GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation) as GameObject;
                projectile.GetComponent<ElectromagneticCannonProjectile>().Damage = damage;
                projectile.GetComponent<ElectromagneticCannonProjectile>().EmmitForce = emmitForce;
                projectile.GetComponent<ElectromagneticCannonProjectile>().Target = tower.GetTarget();
                projectile.GetComponent<ElectromagneticCannonProjectile>().Range = afterRange;
                projectile.GetComponent<ElectromagneticCannonProjectile>().afterHitAttack = afterProjectilePrefab;
            }
            yield return new WaitForSeconds(1 / tower.Speed);
        }
    }
}
