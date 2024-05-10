using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        this.canvas = this.GetComponent<Canvas>();
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        this.canvas.enabled = false;
    }
    public void Pause()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
        this.canvas.enabled = true;
    }
}

