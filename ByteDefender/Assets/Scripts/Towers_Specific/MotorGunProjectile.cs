using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorGunProjectile : MonoBehaviour
{
    public float EmmitForce { get; set; }
    public int Damage { get; set; }

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * EmmitForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}
