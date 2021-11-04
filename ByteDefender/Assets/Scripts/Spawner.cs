using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Enemies and Path")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform[] wayPoints;

    [Header("Spawning Settings")]
    [SerializeField] [Range(0f, 1f)] private float minTimeBetweenInits;
    [SerializeField] [Range(0.1f, 5f)] private float maxTimeBetweenInits;
    [SerializeField] [Range(1f, 10f)] private float speed;
    [SerializeField] private int startingPower;

    [Header("Crypted Bits Chances")]
    [SerializeField] [Range(1,20)] private int waveToStartSpawningCrypted;
    [SerializeField] [Range(0f, 1f)] private float chanceToSpawnCryptedIncrement;

    [Header("Zipped Bits Chances")]
    [SerializeField] [Range(1, 20)] private int waveToStartSpawningZipped;
    [SerializeField] [Range(0f, 1f)] private float chanceToSpawnZippedIncrement;


    private int waveCount = 1;
    //Wave Power equals to enemies * hitPoints
    private int wavePower = 0;
    private int orderInLayer = 0;

    private float chanceToSpawnCrypted = 0;
    private float chanceToSpawnZipped = 0;

    private void Start()
    {
        //SpawnNextWave();
    }

    public bool IsWaveRunning()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        return enemies.Length > 0;
    }

    //Spawns The Next Wave
    public void SpawnNextWave()
    {
        if(!IsWaveRunning())
        {
            wavePower += startingPower;
            (int currentPower, int enemiesToSpawn) = WaveIncrements(wavePower);
            List<GameObject> enemies = EnemyIncrements(currentPower, enemiesToSpawn);
            StartCoroutine(EnableWave(enemies));
            waveCount++;
        }
    }


    //Prepares The Spawning (Spawn Chances and Enemies To Spawn)
    private (int,int) WaveIncrements(int currentPower)
    {
        int enemiesToSpawn = UnityEngine.Random.Range(waveCount, currentPower/waveCount);
        if (waveCount >= waveToStartSpawningCrypted)
        {
            chanceToSpawnCrypted += chanceToSpawnCryptedIncrement;
        }
        if (waveCount >= waveToStartSpawningZipped)
        {
            chanceToSpawnZipped += chanceToSpawnZippedIncrement;
        }
        currentPower -= enemiesToSpawn;
        return (currentPower, enemiesToSpawn);
    }


    //Spawn Enemies and Give them stats based on progress
    private List<GameObject> EnemyIncrements(int currentPower, int enemiesToSpawn)
    {
        List<GameObject> enemies = new List<GameObject>();
        //Spawns a specified number of enemies and gives them base stats
        for(int i =0; i < enemiesToSpawn; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position, transform.rotation) as GameObject;
            //This is used to prevent block graphics stack on each other. This way the first to spawn is on top
            enemy.GetComponent<Enemy>().SetSortingLayer(orderInLayer);
            orderInLayer -= 2;
            enemy.GetComponent<Enemy>().HitPoints++;
            enemy.GetComponent<Enemy>().Speed = speed;
            enemy.GetComponent<Enemy>().Path = wayPoints;
            if (chanceToSpawnCrypted >= UnityEngine.Random.Range(0f, 1f))
            {
                enemy.GetComponent<Enemy>().EnemyType = 1;
            }

            if (chanceToSpawnZipped >= UnityEngine.Random.Range(0f, 1f))
            {
                enemy.GetComponent<Enemy>().EnemyType = 2;
            }
            enemy.GetComponent<Enemy>().UpdateStats();
            enemies.Add(enemy);
        }
        //Remove 1 power for each enemy spawned (because each enemy starts with 1 life)
        currentPower -= enemiesToSpawn;
        //Split remaining power randomly on enemies
        while (currentPower > 0)
        {
            int index = UnityEngine.Random.Range(0, enemies.Count);
            enemies[index].GetComponent<Enemy>().HitPoints++;

            if (enemies[index].GetComponent<Enemy>().EnemyType == 2)
            {
                currentPower -= 2;
            }
            else
            {
                currentPower -= 1;
            }
        }
        return enemies;
    }
    //Begins the flow of incoming enemies
    IEnumerator EnableWave(List<GameObject> enemies)
    {
        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().StartMoving();
            yield return new WaitForSeconds(UnityEngine.Random.Range(minTimeBetweenInits, maxTimeBetweenInits));
        }
    }
}
