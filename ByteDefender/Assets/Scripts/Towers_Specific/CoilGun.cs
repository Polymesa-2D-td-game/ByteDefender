using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tower))]
public class CoilGun : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] int damage;
    [SerializeField] float emmitForce;

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
                GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation) as GameObject;
                projectile.GetComponent<Projectile>().Damage = damage;
                projectile.GetComponent<Projectile>().EmmitForce = emmitForce;
                projectile.GetComponent<Projectile>().Target = tower.GetTarget();
            }
            yield return new WaitForSeconds(1 / tower.Speed);
        }
    }
}
