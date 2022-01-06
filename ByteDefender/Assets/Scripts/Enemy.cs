using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hitText;
    [SerializeField] private Sprite[] enemySprites;
    [SerializeField] GameObject destroyEffect;
    [SerializeField] float spawnSpread = 0.3f;
    [SerializeField] private int coinsOnDeath = 5;
    [SerializeField] AudioSource damageSource;

    [SerializeField] [Range(0f, 1f)] private float timeBetweenInits;

    private enum Type
    {
        Normal,
        Crypted,
        Zipped
    }

    Type enemyType = Type.Normal;
    private int hitPoints;
    private float speed;
    private float currentSpeed;
    private Transform[] path;

    private bool canMove = true;

    public int EnemyType
    {
        get { return (int)enemyType; }
        set { enemyType = (Type)value; }
    }

    public int HitPoints
    {
        get { return hitPoints; }
        set { hitPoints = value; }
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public Transform[] Path
    {
        get { return path; }
        set { path = value; }
    }


    public int dirIndex = 0;
    private int initHitPoints;
    private int orderInLayer = 2;

    // Start is called before the first frame update
    private void Start()
    {
        hitText.text = hitPoints.ToString();
        initHitPoints = hitPoints;
        currentSpeed = speed;
        FormatType();
    }

    //Update enemy stats
    public void UpdateStats()
    {
        hitText.text = hitPoints.ToString();
        initHitPoints = hitPoints;
        currentSpeed = speed;
        FormatType();
    }
    

    //Set enemy sorting layer
    public void SetSortingLayer(int order)
    {
        GetComponent<SpriteRenderer>().sortingOrder = order - 1;
        hitText.GetComponentInParent<Canvas>().sortingOrder = order;
    }

    //Prepares the enemy based on his type
    private void FormatType()
    {
        switch (enemyType)
        {
            case Type.Normal:
                GetComponent<SpriteRenderer>().sprite = enemySprites[0];
                return;
            case Type.Crypted:
                GetComponent<SpriteRenderer>().sprite = enemySprites[1];
                return;
            case Type.Zipped:
                GetComponent<SpriteRenderer>().sprite = enemySprites[2];
                return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    //Moves unit through the specified path
    private void Move()
    {
        if(canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, Path[dirIndex].position, currentSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, Path[dirIndex].position) <= 0.1f)
            {
                if (dirIndex < Path.Length - 1)
                {
                    dirIndex++;
                }
                else
                {
                    //If they reach last path point deal damage to base
                    Base playerBase = FindObjectOfType<Base>();
                    if(playerBase)
                    {
                        playerBase.TakeDamage(hitPoints);
                    }

                    Destroy(gameObject);
                }
            }
        }
    }

    //Enables movement
    public void StartMoving()
    {
        canMove = true;
    }

    //Take damage (called when a projectile hits the enemy)
    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
        FindObjectOfType<SpawnerV2>().GetComponent<AudioSource>().Play();
        hitText.text = hitPoints.ToString();
        if (hitPoints <= 0)
        {
            FindObjectOfType<TowerShopGUI>().AddCoins(coinsOnDeath);
            GameObject effect = Instantiate(destroyEffect, transform.position, transform.rotation);
            Destroy(effect, 0.5f);
            if(enemyType == Type.Zipped)
            {
                ZipBehaviour();
            }
            Destroy(gameObject);
        }
    }

    //Take Slow and play the effect for the specified duration
    IEnumerator TakeSlow(float slowRatio, float duration)
    {
        transform.Find("Slow_Effect").GetComponent<ParticleSystem>().Play();
        currentSpeed = speed - (speed * slowRatio);
        yield return new WaitForSeconds(duration);
        currentSpeed = speed;
        transform.Find("Slow_Effect").GetComponent<ParticleSystem>().Stop();
    }

    //Begin take slow coroutine
    public void SlowEnemy(float slowRatio, float duration)
    {
        StartCoroutine(TakeSlow(slowRatio, duration));
    }

    IEnumerator TakeStun(float stunDuration)
    {
        currentSpeed = 0f;
        yield return new WaitForSeconds(stunDuration);
        currentSpeed = speed;
    }

    public void StunEnemy(float duration)
    {
        StartCoroutine(TakeStun(duration));
    }

    //How zipped packages should behave on death
    private void ZipBehaviour()
    {
        //Create an instance of the prefab
        GameObject enemyPrefab = this.gameObject;
        //Create an empty enemies list
        List<GameObject> enemies = new List<GameObject>();
        //Calculate power and enemies to spawn
        int power = initHitPoints/2;
        int enemiesToSpawn = Random.Range(1, power);

        //Create and Initialize all enemies
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            //Calculate random spread and position
            float randomSpread = Random.Range(-1, 1f) * spawnSpread;
            int randomIndex = Random.Range(Mathf.Clamp(dirIndex-1, 0, path.Length-1), Mathf.Clamp(dirIndex + 1, 0, path.Length - 1));
            //Create a new enemy prefab in the random position
            Vector2 randomPos = path[randomIndex].position + new Vector3(randomSpread, randomSpread, 0f);
            GameObject enemy = Instantiate(enemyPrefab, randomPos, transform.rotation) as GameObject;
            //Initialize enemys stats
            enemy.GetComponent<Enemy>().dirIndex = dirIndex;
            enemy.GetComponent<Enemy>().SetSortingLayer(orderInLayer);
            orderInLayer -= 2;
            enemy.GetComponent<Enemy>().HitPoints++;
            enemy.GetComponent<Enemy>().Speed = speed;
            enemy.GetComponent<Enemy>().Path = path;
            enemy.GetComponent<Enemy>().UpdateStats();
            enemy.GetComponent<Enemy>().StartMoving();
        }
    }
}
