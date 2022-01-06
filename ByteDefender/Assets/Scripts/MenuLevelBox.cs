using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuLevelBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highscore;
    [SerializeField] string sceneName;

    private void Start()
    {
        highscore.text = PlayerPrefs.GetInt(sceneName).ToString();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
