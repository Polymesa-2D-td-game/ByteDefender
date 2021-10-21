using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerShopGUI : MonoBehaviour
{

    //Variables shown in the inspector
    [Header("Towers")]
    [SerializeField] private GameObject[] towers;
    [Header("Adjustments")]
    [Range(0.5f,0.95f)] [SerializeField] float sizeReductionOnPickUp = 0.8f;

    //Private Variables
    private GameObject target = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SnapTowerToMouse(target);
        if(Input.GetMouseButton(0))
        {
            ReleaseTower();
        }
    }

    public void CreateTower(int towerIndex)
    {
        GameObject newTower = Instantiate(towers[towerIndex]);
        ChangeTowerSize(newTower, sizeReductionOnPickUp);
        target = newTower;
    }

    private Vector2 GetMousePosition()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 mousePosToWorld = Camera.main.ScreenToWorldPoint(mousePos);
        Debug.Log(mousePosToWorld);
        return mousePosToWorld;
    }

    private void SnapTowerToMouse(GameObject tower)
    {
        if(tower)
        {
            tower.transform.position = GetMousePosition();
        }
    }

    private void ReleaseTower()
    {
        ChangeTowerSize(target, 1);
        target = null;
    }

    private void ChangeTowerSize(GameObject tower, float size)
    {
        if(tower)
        {
            tower.transform.localScale = new Vector2(size, size);
        }
    } 
}
