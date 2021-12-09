using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerV2 : MonoBehaviour
{
    [Header("Enemies & Path Configuration")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform[] wayPoints;
    [SerializeField] float enemySpeed;

    [Header("Basic Spawner Configuration")]
    [SerializeField] int coinsIncrement;
    [SerializeField] float initTimeBetweenSpawns;
    [SerializeField] float timeBetweenSpawsIncr;
    [SerializeField] int maxWaves;

    [Header("Power Configuration")]
    [SerializeField] float initialPowerRatio;
    [SerializeField] float powerRatioIncr;

    [Header("Spawning Configuration")]
    [SerializeField] float constantEnemiesPerWave;
    [SerializeField] float minSpawnsRatio;
    [SerializeField] float maxSpawnsRatio;

    [Header("Crypted Bits Configuration")]
    [SerializeField] float initChanceToSpawnCrypted;
    [SerializeField] float chanceToSpawnCryptedIncr;
    [SerializeField] int cryptedApealWave;

    [Header("Zipped Bits Configuration")]
    [SerializeField] float initChanceToSpawnZipped;
    [SerializeField] float chanceToSpawnZippedIncr;
    [SerializeField] int zippedApealWave;

    private int waveCount = 1;

    private float wavePowerRatio = 0;

    private float timeBetweenSpawns = 0;

    private float chanceToSpawnCrypted = 0f;
    private float chanceToSpawnZipped = 0f;

    public int GetWaveCount()
    {
        return waveCount;
    }

    private void Start()
    {
        wavePowerRatio = initialPowerRatio;
        timeBetweenSpawns = initTimeBetweenSpawns;
        chanceToSpawnCrypted = initChanceToSpawnCrypted;
        chanceToSpawnZipped = initChanceToSpawnZipped;
    }

    //Check If a Wave is Active and Running
    public bool IsWaveRunning()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        return enemies.Length > 0;
    }

    IEnumerator RewardPlayer()
    {
        while(IsWaveRunning())
        {
            yield return new WaitForSeconds(1f);
        }
        FindObjectOfType<TowerShopGUI>().AddCoins(coinsIncrement * waveCount);
        FindObjectOfType<TowerShopGUI>().UpdateWaveText(waveCount);
    }

    //Spawn and Upgrade Next Wave
    public void SpawnNextWave()
    {
        if (!IsWaveRunning())
        {
            wavePowerRatio += powerRatioIncr;

            int enemiesToSpawn = CalculateEnemiesToSpawn();
            Enemy[] enemies = PrepareEnemies(enemiesToSpawn);
            StartCoroutine(SpawnWave(enemies));
            StartCoroutine(RewardPlayer());
            WaveIncrements();
        }
    }

    //Upgrade Next Wave
    private void WaveIncrements()
    {
        waveCount++;
        timeBetweenSpawns += timeBetweenSpawsIncr;
        if(waveCount >= cryptedApealWave)
        {
            chanceToSpawnCrypted += chanceToSpawnCryptedIncr;
        }
        if(waveCount >= zippedApealWave)
        {
            chanceToSpawnZipped += chanceToSpawnZippedIncr;
        }
    }

    private int CalculateEnemiesToSpawn()
    {
        //Calculate Lower and Upper Bountry
        int min = (int)((waveCount * minSpawnsRatio) + constantEnemiesPerWave);
        int max = (int)((waveCount * maxSpawnsRatio) + constantEnemiesPerWave);

        //Calculate a random number between those bountries
        int enemiesToSpawn = Random.Range(min, max);

        return enemiesToSpawn;
    }

    //Create Instancies Of Enemies With Random Spreaded Hitpoints
    private Enemy[] PrepareEnemies(int enemiesToSpawn)
    {
        //Define Max HitPoints To Spread
        int currentPower = (int)(wavePowerRatio * waveCount);
        Enemy[] enemies = new Enemy[enemiesToSpawn];

        //Initialize Enemies With 1 HitPoint
        for(int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = new Enemy();
            enemies[i].HitPoints = 1;
            enemies[i].Speed = enemySpeed;
            enemies[i].Path = wayPoints;
            currentPower--;
        }

        //Calculate Crypted and Zipped Chances
        for (int i = 0; i < enemies.Length; i++)
        {
            float cryptedChance = Random.Range(0f, 1f);
            if(chanceToSpawnCrypted > cryptedChance)
            {
                enemies[i].EnemyType = 1;
            }

            float zippedChance = Random.Range(0f, 1f);
            if (chanceToSpawnZipped > zippedChance)
            {
                enemies[i].EnemyType = 2;
            }
        }

        //Spread The remaining Hitpoints
        while (currentPower > 0)
        {
            int randomIndex = Random.Range(0, enemies.Length);

            enemies[randomIndex].HitPoints++;
            currentPower--;
        }

        return enemies;
    }

    IEnumerator SpawnWave(Enemy[] enemies)
    {
        PrepareForNextWave();
        foreach(Enemy enemy in enemies)
        {
            GameObject enemyObject = Instantiate(enemyPrefab, transform);
            Enemy enemyComponent = enemyObject.GetComponent<Enemy>();
            enemyComponent.HitPoints = enemy.HitPoints;
            enemyComponent.Speed = enemy.Speed;
            enemyComponent.Path = enemy.Path;
            enemyComponent.EnemyType = enemy.EnemyType;
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private void PrepareForNextWave()
    {
        FindObjectOfType<DestroyBullets>().DeleteAll();
        DebuffAll();
    }

    private static void DebuffAll()
    {
        Buffable[] buffableTowers = FindObjectsOfType<Buffable>();
        foreach (Buffable buffable in buffableTowers)
        {
            buffable.Debuff();
        }

        PowerSupply[] powerSuplies = FindObjectsOfType<PowerSupply>();
        foreach (PowerSupply powerSuply in powerSuplies)
        {
            powerSuply.BuffNearbyTowers();
        }

        Tower[] towers = FindObjectsOfType<Tower>();
        foreach (Tower target in towers)
        {
            target.canDecript = false;
            target.canUnzip = false;
        }
        Decoder[] decoders = FindObjectsOfType<Decoder>();
        if (decoders.Length > 0)
        {
            foreach (Decoder decoder in decoders)
            {
                decoder.BuffTowers();
            }
        }
    }
}
