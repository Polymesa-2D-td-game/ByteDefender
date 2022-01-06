using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerShopGUI : MonoBehaviour
{

    //Variables shown in the inspector
    [Header("Towers")]
    [SerializeField] private GameObject[] towers;
    [SerializeField] private LayerMask towerLayer;
    [SerializeField] private GameObject eventSystem;
    [Header("Adjustments")]
    [Range(0.5f,0.95f)] [SerializeField] float sizeReductionOnPickUp = 0.8f;
    [Header("Energy")]
    [SerializeField] private TextMeshProUGUI powerCoinsTxt;
    [SerializeField] private TextMeshProUGUI waveTxt;
    [SerializeField] private int startingPowerCoins = 50;
    [Header("Tower Info Panel")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Image towerImage;
    [SerializeField] private TextMeshProUGUI towerName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI pathName1;
    [SerializeField] private TextMeshProUGUI pathCost1;
    [SerializeField] private TextMeshProUGUI pathDesc1;
    [SerializeField] private TextMeshProUGUI pathName2;
    [SerializeField] private TextMeshProUGUI pathCost2;
    [SerializeField] private TextMeshProUGUI pathDesc2;
    [Header("Info Show Panel")]
    [SerializeField] private Image infoTowerImage;
    [SerializeField] private TextMeshProUGUI infoTowerName;
    [SerializeField] private TextMeshProUGUI infoDescription;
    [SerializeField] private GameObject infoPref;
    [Header("Options")]
    [SerializeField] private GameObject optionsPanel;


    //Private Variables
    private GameObject target = null;
    [SerializeField] private GameObject infoTarget = null;
    private int currentPowerCoins = 0;
    private float currentTimeScale = 1f;

    // Start is called before the first frame update
    void Start()
    {
        currentPowerCoins = startingPowerCoins;
        UpdateCoinsText();
    }

    // Update is called once per frame
    void Update()
    {
        SelectInfoTarget();
        SnapTowerToMouse(target);
        if(Input.GetMouseButton(0) && target)
        {
            target.GetComponent<Tower>().ChangeRangeVisibility(false);
            ReleaseTower();
        }
    }

    //Opens or closes options panel
    public void ToggleOptions()
    {
        if(optionsPanel.active)
        {
            optionsPanel.SetActive(false);
            Time.timeScale = currentTimeScale;
        }
        else
        {
            optionsPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void UpdateWaveText(int wave)
    {
        waveTxt.text = "Wave: " + wave.ToString();
    }

    //Updates the Information In Tower Info Panel
    private void UpdateInfoPanel()
    {
        if(infoTarget && !target)
        {
            towerName.text = infoTarget.GetComponent<Tower>().TowerName;
            description.text = infoTarget.GetComponent<Tower>().Description;
            towerImage.sprite = infoTarget.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void DisplayInfo(Tower tower)
    {
        infoPref.SetActive(true);
        infoDescription.text = tower.Description;
        infoTowerName.text = tower.TowerName;
        infoTowerImage.sprite = tower.GetComponent<SpriteRenderer>().sprite;
    }

    //Called when a tower is moved on top of the trash can
    public void DestroyTowerBeforePlaced()
    {
        if(target)
        {
            AddCoins(target.GetComponent<Tower>().PowerCost);
            Destroy(target);
            target = null;
        }
    }
    //Called when a tower is selected and the trash can button is pressed
    public void DestroyTower()
    {
        if (infoTarget)
        {
            AddCoins(infoTarget.GetComponent<Tower>().PowerCost /2);
            Destroy(infoTarget);
            InfoPanelSetActive(false);
            infoTarget = null;
        }
    }

    //Hide info panel
    public void HideInfo()
    {
        infoPref.SetActive(false);
    }

    //Changes The Focus 0:First 1:Last 2:Strongest
    public void SetFocus(int focus)
    {
        infoTarget.GetComponent<Tower>().ChangeFocus(focus);
    }

    //Selects a Tower with Mouse Click
    private void SelectInfoTarget()
    {
        RaycastHit2D hit = Physics2D.Raycast(GetMousePosition(), Vector3.forward, towerLayer);
        if(Input.GetMouseButtonDown(0))
        {
            //If there is a tower under mouse
            if (hit)
            {
                if(hit.collider.CompareTag("Tower"))
                {
                    //If you already have an info target
                    if (infoTarget && !target)
                    {
                        infoTarget.GetComponent<Tower>().ChangeRangeVisibility(false);
                        InfoPanelSetActive(false);
                        infoTarget = null;
                    }
                    //Update Info Box
                    if (!target)
                    {
                        infoTarget = hit.collider.gameObject;
                        InfoPanelSetActive(true);
                        UpdateInfoPanel();
                        infoTarget.GetComponent<Tower>().ChangeRangeVisibility(true);
                    }
                }
                else
                {
                    //If there is no UI Element under mouse
                    if (!IsPointerOverUIElement())
                    {
                        //If there is an info target
                        if (infoTarget && !target)
                        {
                            infoTarget.GetComponent<Tower>().ChangeRangeVisibility(false);
                        }
                        //Dismiss info target
                        InfoPanelSetActive(false);
                        infoTarget = null;
                    }
                }
            }
            //If there is no tower under mouse
            else
            {
                //If there is no UI Element under mouse
                if(!IsPointerOverUIElement())
                {
                    //If there is an info target
                    if(infoTarget && !target)
                    {
                        infoTarget.GetComponent<Tower>().ChangeRangeVisibility(false);
                    }
                    //Dismiss info target
                    InfoPanelSetActive(false);
                    infoTarget = null;
                } 
            }
        }
        
    }

    //Checks if mouse is over UI Element https://docs.unity3d.com/2018.1/Documentation/ScriptReference/UI.GraphicRaycaster.Raycast.html
    private bool IsPointerOverUIElement()
    {
        EventSystem m_EventSystem = eventSystem.GetComponent<EventSystem>();
        PointerEventData m_PointerEventData;
        GraphicRaycaster m_Raycaster = GetComponent<GraphicRaycaster>();
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        return results.Count > 0;
    }

    //Change Tower Information Panel Visibility
    private void InfoPanelSetActive(bool status)
    {
        if (status)
        {
            DisplayUpgradeCosts();
        }
        infoPanel.SetActive(status);
    }

    //Checks If there is enough powerCoins to make a transaction
    private bool CanBuy(int currentCost)
    {
        if(currentCost <= currentPowerCoins)
        {
            currentPowerCoins -= currentCost;
            UpdateCoinsText();
            return true;
        }
        return false;
    }

    public void AddCoins(int value)
    {
        currentPowerCoins += value;
        UpdateCoinsText();
    }

    //Updates the GUI Power Coin Text
    private void UpdateCoinsText()
    {
        powerCoinsTxt.text = currentPowerCoins.ToString();
    }
    
    //Creates a specified tower
    public void CreateTower(int towerIndex)
    {
        if(CanBuy(towers[towerIndex].GetComponent<Tower>().PowerCost))
        {
            GameObject newTower = Instantiate(towers[towerIndex]);
            ChangeTowerSize(newTower, sizeReductionOnPickUp);
            target = newTower;
            target.GetComponent<Tower>().ChangeRangeVisibility(true);
            target.GetComponent<CircleCollider2D>().enabled = false;
            UpdateInfoPanel();
        }
    }


    //Returns the Mouse Position in World Space
    private Vector2 GetMousePosition()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 mousePosToWorld = Camera.main.ScreenToWorldPoint(mousePos);
        return mousePosToWorld;
    }

    //Moves the selected tower with Mouse
    private void SnapTowerToMouse(GameObject tower)
    {
        if(tower)
        {
            tower.transform.position = GetMousePosition();
        }
    }

    //Place Tower
    private void ReleaseTower()
    {
        //Cast a ray
        RaycastHit2D hit = Physics2D.Raycast(GetMousePosition(), Vector3.forward); 
        //if ray hits a collider and its not over a ui element
        if(!hit.collider && !IsPointerOverUIElement())
        {
            //Place tower
            target.GetComponent<CircleCollider2D>().enabled = true;
            target.GetComponent<Tower>().DebuffAll();
            ChangeTowerSize(target, 1);
            infoTarget = null;
            target = null;
        }
    }

    //Changes the Tower Size (Used when picked up/down)
    private void ChangeTowerSize(GameObject tower, float size)
    {
        if(tower)
        {
            tower.transform.localScale = new Vector2(size, size);
        }
    } 

    //Change game speed
    public void ToggleSpeed()
    {
        if(Time.timeScale == 1f)
        {
            Time.timeScale = 2f;
            currentTimeScale = 2f;
            return;
        }
        Time.timeScale = 1f;
        currentTimeScale = 1f;
    }

    //Trigger Button Animation
    public void ButtonAnimation(GameObject button)
    {
        ToggleSpeed();
        if(Time.timeScale == 1)
        {
            button.GetComponent<Animator>().Play("Pressed");
            return;
        }
        button.GetComponent<Animator>().Play("Selected");
    }

    //Upgrade tower (path index left - right upgrade)
    public void Upgrade(int pathIndex)
    {
        if (infoTarget)
        {
            Upgrader upgrader = infoTarget.GetComponent<Upgrader>();
            if(CanBuy(upgrader.UpgradeCost(pathIndex)))
            {
                upgrader.Upgrade(pathIndex);
            }
        }
        DisplayUpgradeCosts();
    }

    //Update upgrade panel info
    public void DisplayUpgradeCosts()
    {
        if (infoTarget)
        {
            Upgrader upgrader = infoTarget.GetComponent<Upgrader>();
            pathName1.text = upgrader.GetUpgradeName(0);
            pathName2.text = upgrader.GetUpgradeName(1);
            pathCost1.text = upgrader.UpgradeCost(0).ToString();
            pathCost2.text = upgrader.UpgradeCost(1).ToString();
            pathDesc1.text = upgrader.GetUpgradeDescription(0).ToString();
            pathDesc2.text = upgrader.GetUpgradeDescription(1).ToString();
        }
    }

}
