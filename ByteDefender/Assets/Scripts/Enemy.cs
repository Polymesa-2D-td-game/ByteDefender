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

    private bool canMove = false;

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


    private int dirIndex = 0;
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

    public void UpdateStats()
    {
        hitText.text = hitPoints.ToString();
        initHitPoints = hitPoints;
        currentSpeed = speed;
        FormatType();
    }
    

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
                    FindObjectOfType<Base>().TakeDamage(hitPoints);
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

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
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

    IEnumerator TakeSlow(float slowRatio, float duration)
    {
        currentSpeed = speed - (speed * slowRatio);
        yield return new WaitForSeconds(duration);
        currentSpeed = speed;
    }

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

    private void ZipBehaviour()
    {
        GameObject enemyPrefab = this.gameObject;
        List<GameObject> enemies = new List<GameObject>();
        Vector2 dir = transform.position - path[dirIndex].position;
        dir = dir.normalized;
        int power = initHitPoints;
        int enemiesToSpawn = Random.Range(4, initHitPoints);
        power -= enemiesToSpawn;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            float randomSpread = Random.Range(-1f, 1f) * spawnSpread;
            Vector2 randomPos = transform.position + new Vector3(randomSpread, randomSpread, 0);
            GameObject enemy = Instantiate(enemyPrefab, randomPos, transform.rotation) as GameObject;
            enemy.GetComponent<Enemy>().SetSortingLayer(orderInLayer);
            orderInLayer -= 2;
            enemy.GetComponent<Enemy>().HitPoints++;
            enemy.GetComponent<Enemy>().Speed = speed;
            enemy.GetComponent<Enemy>().Path = path;
            enemy.GetComponent<Enemy>().UpdateStats();
            enemies.Add(enemy);
        }

        power -= enemiesToSpawn;

        while (power > 0)
        {
            int index = Random.Range(0, enemies.Count);
            enemies[index].GetComponent<Enemy>().HitPoints++;
            power -= 1;
        }

        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().StartMoving();
        }
    }

}
