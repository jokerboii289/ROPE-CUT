using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    bool win;
    bool loose;

    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject failPanel;
    [SerializeField] GameObject lvlPanel;
    public static PauseMenu instance;
    // Start is called before the first frame update
    void Start()
    {
        win = false;
        loose = false;
        lvlPanel.SetActive(true);
        winPanel.SetActive(false);
        failPanel.SetActive(false);
        instance = this;

        if (PlayerPrefs.GetInt("Index") == SceneManager.sceneCountInBuildSettings)
        {
            levelText.text =  "LEVEL "+(PlayerPrefs.GetInt("IndexNo")).ToString();
        }

        BossDeathState.gameOver += GameOver;
        BossStateManager.playerDead += Loose;
        Vibration.Init();
    }

    public void Reload()//relod
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLvl()
    {
        if (PlayerPrefs.GetInt("Index") == SceneManager.sceneCountInBuildSettings)
        {
            PlayerPrefs.SetInt("IndexNo", (PlayerPrefs.GetInt("IndexNo") + 1));//save index no for looping;
            SceneManager.LoadScene("FirstScene");    
        }
        if (PlayerPrefs.GetInt("Index") < SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1;
            //save level index
            PlayerPrefs.SetInt("Index", (SceneManager.GetActiveScene().buildIndex) + 1);
            PlayerPrefs.SetInt("IndexNo", SceneManager.GetActiveScene().buildIndex);//save index no for looping;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void GameOver() //win
    {
        if (!win && !loose)
        {
            print("over");
            StartCoroutine(DelayWin());
            win = true;
        }
    }
    IEnumerator DelayWin()
    {      
        lvlPanel.SetActive(false);
        yield return new WaitForSeconds(1f);
        AudioManager.instance.Play("win");
        winPanel.SetActive(true);
    }
    public void Loose()
    {
        if (!loose && !win)
        {
            print("loose");
            StartCoroutine(DelayFail());
            loose = true;
        }
    }
    IEnumerator DelayFail()
    {
        lvlPanel.SetActive(false);
        yield return new WaitForSeconds(1);
        AudioManager.instance.Play("loose");
        failPanel.SetActive(true);
    }

    private void OnDestroy()
    {
        BossDeathState.gameOver -= GameOver;
        BossStateManager.playerDead -= Loose;
    }
}
