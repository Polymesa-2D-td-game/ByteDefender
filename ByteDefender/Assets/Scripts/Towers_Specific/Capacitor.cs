using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tower))]
public class Capacitor : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int coinValue;

    private Tower tower;
    // Start is called before the first frame update

    private void Awake()
    {
        tower = GetComponent<Tower>();
        StartCoroutine(SpawnCoins());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnCoins()
    {
        while(true)
        {
            if(FindObjectOfType<Spawner>().IsWaveRunning() && tower.IsEnabled())
            {
                CreateCoin();
            }
            yield return new WaitForSeconds(1/tower.Speed);
        }
    }

    private void CreateCoin()
    {
        float range = tower.Range;
        float distance = range / 2f;
        float randomX = UnityEngine.Random.Range(-distance, distance);
        float randomY = UnityEngine.Random.Range(-distance, distance);
        Vector2 spawnSpot = new Vector2(transform.position.x + randomX, transform.position.y + randomY);
        GameObject coin = Instantiate(coinPrefab, spawnSpot, transform.rotation) as GameObject;
        coin.GetComponent<PowerCoin>().Value = coinValue;
    }
}
