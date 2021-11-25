using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DataInjector : MonoBehaviour
{
    private Tower tower;
    private Spawner spawner;
    [SerializeField] float randomSpawnEffector = 0.4f;

    public List<Transform> waypoints = new List<Transform>();
    private Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        tower = GetComponent<Tower>();
        spawner = FindObjectOfType<Spawner>();
        StartCoroutine(Attack());
        StartCoroutine(GetWayPoints());
        

    }

    private void GetClosestWaypoints()
    {
        Collider2D[] checkSphere = Physics2D.OverlapCircleAll(transform.position, tower.Range);
        foreach(Collider2D col in checkSphere)
        {
            if(col.CompareTag("Path"))
            {
                waypoints.Add(col.GetComponent<Transform>());
                Debug.Log(col.name);
            }
        }

    }

    private void Update()
    {
        LookAtTarget(target);
    }

    public void LookAtTarget(Vector3 pos)
    {
        Vector3 diff = pos - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, rot_z - 90), 12f * Time.deltaTime);
    }

    IEnumerator Attack()
    {
        yield return StartCoroutine(GetWayPoints());
        while (true)
        {
            if (tower.IsEnabled() && spawner.IsWaveRunning())
            {
                int i = Random.Range(0, waypoints.Count - 1);
                float randomSeedX = Random.Range(-randomSpawnEffector, randomSpawnEffector);
                float randomSeedY = Random.Range(-randomSpawnEffector, randomSpawnEffector);
                Vector3 position = waypoints[i].position + new Vector3(randomSeedX, randomSeedY);
                target = position;
                GameObject projectile = Instantiate(tower.SpawnObject, tower.SpawnPoint.position , transform.rotation) as GameObject;
                projectile.GetComponent<DataInjectorProjectile>().Target = position;
                projectile.GetComponent<DataInjectorProjectile>().EmmitForce = 8f;
                projectile.GetComponent<DataInjectorProjectile>().Damage = tower.CurrentIntPower;
            }
            yield return new WaitForSeconds(1 / tower.CurrentSpeed);
        }
    }

    IEnumerator GetWayPoints()
    {
        while(!tower.IsEnabled())
        {
            yield return new WaitForSeconds(1f);
        }
        GetClosestWaypoints();
    }
}
