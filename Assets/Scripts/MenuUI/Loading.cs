using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] float simulatedTimeToLoad = 3.0f;
    Slider loadingBar;
    float initTime;

    void Start()
    {
        Time.timeScale = 1;
        this.loadingBar = this.GetComponentInChildren<Slider>();
        this.initTime = Time.time;

        var levelToLoad = PlayerPrefs.GetString("LevelToLoad", "MainMenuScreen");

        _ = this.StartCoroutine(this.Load(levelToLoad));
    }

    void Update()
    {
        this.loadingBar.value = (Time.time - this.initTime) / this.simulatedTimeToLoad;
    }

    private IEnumerator Load(string levelToLoad)
    {
        var loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        loadOperation.allowSceneActivation = false;
        
        // Fake the loading
        yield return new WaitForSeconds(this.simulatedTimeToLoad);

        // Wait for 85% of the scene to be loaded
        yield return new WaitUntil(() => loadOperation.progress > 0.85f);

        // Load the scene
        loadOperation.allowSceneActivation = true;
    }
}
