using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tower))]
public class Capacitor : MonoBehaviour
{
    private Tower tower;

    private void Awake()
    {
        tower = GetComponent<Tower>();
        StartCoroutine(SpawnCoins());
    }

    //Spawn coins frequently based on speed
    IEnumerator SpawnCoins()
    {
        while(true)
        {
            if(FindObjectOfType<Spawner>().IsWaveRunning() && tower.IsEnabled())
            {
                CreateCoin();
                tower.PlaySoundEffect();
            }
            yield return new WaitForSeconds(1/tower.CurrentSpeed);
        }
    }

    //Create coin at a random position close to its range
    private void CreateCoin()
    {
        float range = tower.CurrentRange;
        float distance = range / 2f;
        float randomX = UnityEngine.Random.Range(-distance, distance);
        float randomY = UnityEngine.Random.Range(-distance, distance);
        Vector2 spawnSpot = new Vector2(transform.position.x + randomX, transform.position.y + randomY);
        GameObject coin = Instantiate(tower.SpawnObject, spawnSpot, transform.rotation) as GameObject;
        coin.GetComponent<PowerCoin>().Value = tower.CurrentIntPower;
    }
}
