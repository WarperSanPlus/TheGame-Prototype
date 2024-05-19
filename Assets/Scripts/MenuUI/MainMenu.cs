using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menuCanvas;

    private void Start()
    {
        this.menuCanvas.SetActive(true);
    }

    public void StartGame()
    {
        PlayerPrefs.SetString("LevelToLoad", "GameScene");
        PlayerPrefs.Save();
        SceneManager.LoadScene("LoadingScreen");
        
        this.menuCanvas.SetActive(false);
    }

    public void StartShootingRange()
    {
        PlayerPrefs.SetString("LevelToLoad", "CannonTestScene");
        PlayerPrefs.Save();
        SceneManager.LoadScene("LoadingScreen");

        this.menuCanvas.SetActive(false);
    }
}
