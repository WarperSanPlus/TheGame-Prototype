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
        SceneManager.LoadScene("BoatTestScene");

        this.menuCanvas.SetActive(false);
    }

    public void StartShootingRange()
    {
        SceneManager.LoadScene("CannonTestScene");

        this.menuCanvas.SetActive(false);
    }
}
