using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] float simulatedTimeToLoad = 3.0f;
    string levelToLoad = "MainMenu";
    Slider loadingBar;
    float initTime;

    void Start()
    {
        this.loadingBar = this.GetComponentInChildren<Slider>();
        this.initTime = Time.time;

        if (PlayerPrefs.HasKey("LevelToLoad"))
            this.levelToLoad = PlayerPrefs.GetString("LevelToLoad");

        this.StartCoroutine(this.Load());
    }

    void Update()
    {
        this.loadingBar.value = (Time.time - this.initTime) / this.simulatedTimeToLoad;
    }
    IEnumerator Load()
    {
        yield return new WaitForSeconds(this.simulatedTimeToLoad);
        SceneManager.LoadScene(this.levelToLoad);
    }
}
