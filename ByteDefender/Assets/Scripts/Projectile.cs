using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float EmmitForce {get; set;}
    public int Damage {get; set;}
    public GameObject Target { get; set; }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Target)
        {
            transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, EmmitForce * Time.deltaTime);
            if (Vector2.Distance(transform.position, Target.transform.position) < 0.05f)
            {
                Target.GetComponent<Enemy>().TakeDamage(Damage);
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
