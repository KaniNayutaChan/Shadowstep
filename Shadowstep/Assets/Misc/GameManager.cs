using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject pauseCanvas;
    bool isPaused = false;
    float currentTimeScale = 1;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                currentTimeScale = Time.timeScale;
                isPaused = true;
                Time.timeScale = 0;
                pauseCanvas.SetActive(true);
            }
            else
            {
                isPaused = false;
                Time.timeScale = currentTimeScale;
                pauseCanvas.SetActive(false);
            }
        }
    }
}
