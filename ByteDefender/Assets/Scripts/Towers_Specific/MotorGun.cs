using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tower))]
public class MotorGun : MonoBehaviour
{
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
                GameObject projectile = Instantiate(tower.SpawnObject, tower.SpawnPoint.position, tower.SpawnPoint.rotation) as GameObject;
                tower.PlaySoundEffect();
                projectile.GetComponent<MotorGunProjectile>().Damage = tower.CurrentIntPower;
                projectile.GetComponent<MotorGunProjectile>().EmmitForce = tower.EmmitForce;
            }
            yield return new WaitForSeconds(1 / tower.CurrentSpeed);
        }
    }

}
