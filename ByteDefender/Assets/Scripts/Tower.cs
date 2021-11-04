using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Functionality")]
    [SerializeField] private LayerMask enemiesMask;
    [Header("Tower Information")]
    [SerializeField] private string towerName;
    [SerializeField][TextArea] private string description;
    [SerializeField] private int powerCost;

    [Header("Base Stats")]
    [SerializeField] private float range;
    [SerializeField] private float speed;
    [SerializeField] private Transform rangeCircle;

    Vector3 startingScale;
    private Collider2D[] enemiesInRange = new Collider2D[1];
    [SerializeField] private GameObject target;

    private enum Focus
    {
        First,
        Last,
        Strongest,
        All
    }

    [SerializeField] private Focus towerFocus = Focus.First;

    private void Awake()
    {
        startingScale = rangeCircle.transform.localScale;
        UpdateRangeIndicator();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectEnemies();
        target = FindTarget();
    }

    //Find all enemies in range
    private void DetectEnemies()
    {
        Collider2D[] checkSphere = Physics2D.OverlapCircleAll(transform.position, range, enemiesMask);
        enemiesInRange = checkSphere;
    }

    //Change the focus of this tower
    public void ChangeFocus(int focus)
    {
        towerFocus = (Focus)focus;
    }

    //Returns the target of this tower
    public GameObject GetTarget()
    {
        return target;
    }

    //Returns all targets in range
    public GameObject[] GetTargetsInRange()
    {
        List<GameObject> targets = new List<GameObject>();
        if(enemiesInRange.Length > 0)
        {
            foreach (Collider2D col in enemiesInRange)
            {
                if (col != null)
                {
                    targets.Add(col.gameObject);
                }
            }
        }
        return targets.ToArray();
    }

    //Look At : https://answers.unity.com/questions/585035/lookat-2d-equivalent-.html
    public void LookAtTarget()
    {
        if(target)
        {
            Vector3 diff = target.transform.position - transform.position;
            diff.Normalize();
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
    }

    //Find and return the First/Last/Strongest Enemy
    private GameObject FindTarget()
    {
        GameObject currentTarget = null;
        if(enemiesInRange.Length > 0)
        {
            GameObject first = null, last = null, strongest = null;
            float min = Mathf.Infinity;
            float max = Mathf.NegativeInfinity;
            int maxHitPoints = -1;
            for (int i = 0; i < enemiesInRange.Length; i++)
            {
                float distanceFromBase = Vector2.Distance(FindObjectOfType<Spawner>().transform.position, enemiesInRange[i].transform.position);
                if (distanceFromBase < min)
                {
                    min = distanceFromBase;
                    last = enemiesInRange[i].gameObject;
                }
                if (distanceFromBase > max)
                {
                    max = distanceFromBase;
                    first = enemiesInRange[i].gameObject;

                }
                if (maxHitPoints < enemiesInRange[i].GetComponent<Enemy>().HitPoints)
                {
                    maxHitPoints = enemiesInRange[i].GetComponent<Enemy>().HitPoints;
                    strongest = enemiesInRange[i].gameObject;
                }
            }
            switch (towerFocus)
            {
                case Focus.First:
                    currentTarget = first;
                    break;
                case Focus.Last:
                    currentTarget = last;
                    break;
                case Focus.Strongest:
                    currentTarget = strongest;
                    break;
                default:
                    break;
            }
        }
        return currentTarget;
    }

    //Update Towers Range Indicator
    private void UpdateRangeIndicator()
    {
        rangeCircle.transform.localScale = startingScale * range;
    }

    //Show/Hide Range Indicator
    public void ChangeRangeVisibility(bool isVisible)
    {
        UpdateRangeIndicator();
        rangeCircle.gameObject.SetActive(isVisible);
    }

    public bool IsEnabled()
    {
        return GetComponent<CircleCollider2D>().enabled;
    }

    public float Range
    {
        get { return range; }
        set { range = value; }
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public int PowerCost
    {
        get { return powerCost; }
        set { powerCost = value; }
    }

    public string TowerName
    {
        get { return towerName; }
        set { towerName = value; }
    }

    public string Description
    {
        get { return description; }
        set { description = value; }
    }


    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
