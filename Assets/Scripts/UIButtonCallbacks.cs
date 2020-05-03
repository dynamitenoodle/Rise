using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonCallbacks : MonoBehaviour
{
    public bool isPaused = false;

    public GameObject mainmenuui;
    public GameObject creditsui;

    public GameObject gameoverui;
    public GameObject gameui;
    public GameObject pauseui;

    public GameObject titleText;

    public void CloseGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("LevelGenTest");
        Time.timeScale = 1.0f;
    }

    public void ShowCredits()
    {
        mainmenuui.SetActive(false);
        titleText.SetActive(false);
        creditsui.SetActive(true);
    }

    public void ShowMainMenu()
    {
        mainmenuui.SetActive(true);
        titleText.SetActive(true);
        creditsui.SetActive(false);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
