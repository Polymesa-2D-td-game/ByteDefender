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

    //Damages base (called when enemy reaches last path point)
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(healthText)
        {
            GetComponent<AudioSource>().Play();
            healthText.text = currentHealth.ToString();
        }
        
        //If health is less than 0, show gameover
        if(currentHealth <= 0)
        {
            SaveScore();
            Debug.Log("You Loose");
            Time.timeScale = 1f;
            SceneManager.LoadScene("Game_Over");
        }
    }

    //Saves score when player looses
    private void SaveScore()
    {
        int wave = FindObjectOfType<SpawnerV2>().GetWaveCount();
        string sceneName = SceneManager.GetActiveScene().name;
        //Saves a variable localy that stores the highscore of the current scene
        if(wave > PlayerPrefs.GetInt(sceneName))
            PlayerPrefs.SetInt(sceneName, wave);
    }
}
