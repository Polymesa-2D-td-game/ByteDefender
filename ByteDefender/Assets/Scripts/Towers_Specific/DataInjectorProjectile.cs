using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataInjectorProjectile : MonoBehaviour
{
    public float EmmitForce { get; set; }
    public int Damage { get; set; }
    public Vector3 Target { get; set; }

    public GameObject afterHitAttack { get; set; }
    public float Range { get; set; }

    private void Update()
    {
        if (Target != Vector3.zero)
        {
            if (Vector2.Distance(transform.position, Target) > 0.01f)
            {
                transform.position = Vector2.MoveTowards(transform.position, Target, EmmitForce * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            int enemyHitPoints = collision.GetComponent<Enemy>().HitPoints;
            if(enemyHitPoints >= Damage)
            {
                collision.GetComponent<Enemy>().TakeDamage(Damage);
                Destroy(gameObject);
            }
            else
            {
                collision.GetComponent<Enemy>().TakeDamage(enemyHitPoints);
                Damage -= enemyHitPoints;
            }
        }
    }
}
