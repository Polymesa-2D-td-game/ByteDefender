using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCoin : MonoBehaviour
{
    [SerializeField] private string powerCoinUITag = "PowerCoin_UI";
    [SerializeField] private float travelSpeed = 10f;

    public int Value { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0.3f, 0.3f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        //Move towards UI Coin Indicator
        transform.position = Vector3.Lerp(transform.position, GetPowerCoinUIPos(), travelSpeed * Time.deltaTime);
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 1f, 1f), travelSpeed * Time.deltaTime);
        //Destroy when reaches destination
        if (Vector3.Distance(transform.position, GetPowerCoinUIPos()) < 0.1f)
        {
            FindObjectOfType<TowerShopGUI>().AddCoins(Value);
            Destroy(gameObject);
        }
    }

    //Get UI Element position to world space
    private Vector3 GetPowerCoinUIPos()
    {
        GameObject powerCoinUI = GameObject.FindGameObjectWithTag(powerCoinUITag);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(powerCoinUI.transform.position);
        return worldPos;
    }
}
