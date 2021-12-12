using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Base : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private int startingHealth;

    private int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        healthText.text = currentHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(healthText)
        {
            healthText.text = currentHealth.ToString();
        }
        
        if(currentHealth <= 0)
        {
            Debug.Log("You Loose");
            Time.timeScale = 1f;
            SceneManager.LoadScene("Game_Over");
        }
    }
}
