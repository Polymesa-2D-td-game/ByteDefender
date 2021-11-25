using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tower))]
public class MotorGun : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform emmiter;
    [SerializeField] int damage;
    [SerializeField] float emmitForce;

    private Tower tower;
    private Spawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        tower = GetComponent<Tower>();
        spawner = FindObjectOfType<Spawner>();
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        LookAtMouse();
    }

    public void LookAtMouse()
    {
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    IEnumerator Attack()
    {
        while (true)
        {
            if (tower.IsEnabled() && spawner.IsWaveRunning())
            {
                GameObject projectile = Instantiate(projectilePrefab, emmiter.position, emmiter.rotation) as GameObject;
                projectile.GetComponent<MotorGunProjectile>().Damage = damage;
                projectile.GetComponent<MotorGunProjectile>().EmmitForce = emmitForce;
            }
            yield return new WaitForSeconds(1 / tower.Speed);
        }
    }

}
