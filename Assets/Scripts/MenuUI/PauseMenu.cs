using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        this.canvas = this.GetComponent<Canvas>();
        this.canvas.enabled = false;
    }

    public void Resume()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Time.timeScale = 1;
        this.canvas.enabled = false;
    }
    public void Pause()
    {
        if (!this.canvas.enabled)
        {
            Cursor.lockState = CursorLockMode.None;
            //Time.timeScale = 0;
            this.canvas.enabled = true;
        }
        else
        {
            this.Resume();
        }
    }
    public void StartMainMenu()
    {
        PlayerPrefs.SetString("LevelToLoad", "MainMenuScreen");
        PlayerPrefs.Save();

        SceneManager.LoadScene("LoadingScreen");
        // SceneManager.LoadScene("MainMenu");

    }
    public void StartShootingRange()
    {
        PlayerPrefs.SetString("LevelToLoad", "CannonTestScene");
        PlayerPrefs.Save();

        SceneManager.LoadScene("LoadingScreen");

        //SceneManager.LoadScene("CannonTestScene");
    }
    public void StartGame()
    {
        PlayerPrefs.SetString("LevelToLoad", "BoatTestScene");
        PlayerPrefs.Save();

        SceneManager.LoadScene("LoadingScreen");
    }
    public void Exit()
    {
        Application.Quit();
    }
}

