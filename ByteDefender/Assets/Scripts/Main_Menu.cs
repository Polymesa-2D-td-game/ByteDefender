using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    SpawnerV2 spawner;
    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<SpawnerV2>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!spawner.IsWaveRunning())
        {
            spawner.SpawnNextWave();
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
