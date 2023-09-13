using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;
 

    private bool isPaused = false;

    private void Start()
    {
        // Hide the pause menu initially
        pauseMenuUI.SetActive(false);
        
        // Add click event listeners to the buttons
    }

    private void Update()
    {
        // Check for pause input (e.g., 'P' key)
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Pressed P");
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        Time.timeScale = 0f; // Pause the game
        pauseMenuUI.SetActive(true);
        isPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f; // Resume the game
        pauseMenuUI.SetActive(false);
        isPaused = false;
    }

    public void Quit()
    {
        // You can add code here to handle quitting the game
        // For example: Application.Quit();
    }
}