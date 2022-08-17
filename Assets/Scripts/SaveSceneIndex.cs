using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSceneIndex : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Index") == SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(RandomIndexNo());
        }
        else if (PlayerPrefs.GetInt("Index") <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + PlayerPrefs.GetInt("Index"));
    }

    int RandomIndexNo()
    {
        var rand = Random.Range(3, SceneManager.sceneCountInBuildSettings);
        //PlayerPrefs.SetInt("Index", rand);
        return rand;
    }
}