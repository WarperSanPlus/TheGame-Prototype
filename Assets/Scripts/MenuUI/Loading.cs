using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] float simulatedTimeToLoad = 2.0f;
    Slider loadingBar;
    float initTime;
    List<float> pauseTimes;

    void Start()
    {
        Time.timeScale = 1;
        this.loadingBar = this.GetComponentInChildren<Slider>();
        this.initTime = Time.time;

        var levelToLoad = PlayerPrefs.GetString("LevelToLoad", "MainMenuScreen");

        this.pauseTimes = new List<float>();
        for (int i = 0; i < 2; i++)
        {
            this.pauseTimes.Add(Random.Range(0, this.simulatedTimeToLoad));
        }
        this.pauseTimes.Sort();

        _ = this.StartCoroutine(this.Load(levelToLoad));
    }

    private IEnumerator Load(string levelToLoad)
    {
        var loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        loadOperation.allowSceneActivation = false;

        float elapsedTime = 0.0f;
        int pauseIndex = 0;

        while (elapsedTime < this.simulatedTimeToLoad)
        {
            if (pauseIndex < this.pauseTimes.Count && elapsedTime >= this.pauseTimes[pauseIndex])
            {
                pauseIndex++;
                yield return new WaitForSeconds(1.0f);
            }
            else
            {
                elapsedTime += Time.deltaTime;
                this.loadingBar.value = Mathf.Clamp01(elapsedTime / this.simulatedTimeToLoad);
                yield return null;
            }
        }

        // Making loading bar full after simulated loadin time
        this.loadingBar.value = 1.0f;

        // Wait for 85% of the scene to be loaded
        yield return new WaitUntil(() => loadOperation.progress >= 0.85f);

        // Load the scene
        loadOperation.allowSceneActivation = true;
    }
}
