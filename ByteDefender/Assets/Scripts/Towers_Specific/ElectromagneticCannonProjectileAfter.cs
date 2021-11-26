using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectromagneticCannonProjectileAfter : MonoBehaviour
{
    public float Range { get; set; }
    public int Damage { get; set; }
    

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(Range, Range, Range);
        DamageEnemiesInRange();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DamageEnemiesInRange()
    {
        Collider2D[] checkSphere = Physics2D.OverlapCircleAll(transform.position, Range);
        foreach(Collider2D col in checkSphere)
        {
            if(col.CompareTag("Enemy"))
            {
                col.GetComponent<Enemy>().TakeDamage(Damage);
            }
        }
        Destroy(gameObject, 1f);
    }


    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
