using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectromagneticCannonProjectile : MonoBehaviour
{
    
    public float EmmitForce { get; set; }
    public int Damage { get; set; }
    public GameObject Target { get; set; }

    public GameObject afterHitAttack { get; set; }
    public float Range { get; set; }

    private Vector2 dir;
    private void Start()
    {
        if(Target)
        {
            GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * EmmitForce, ForceMode2D.Impulse);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(Damage);
            GameObject afterHit = Instantiate(afterHitAttack, transform.position, transform.rotation);
            afterHit.GetComponent<ElectromagneticCannonProjectileAfter>().Damage = Damage;
            afterHit.GetComponent<ElectromagneticCannonProjectileAfter>().Range = Range;
            Destroy(gameObject);
        }
    }
}
