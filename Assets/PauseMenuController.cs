using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject controlUI;

    //Adding reference to gameplayUI so it can be disabled on pause
    public GameObject gameplayUI;

    public MinigameManager MM;
 

    private bool isPaused = false;
    private bool canPause = true;

    private void Start()
    {
        // Hide the pause menu initially
        pauseMenuUI.SetActive(false);
        controlUI.SetActive(false);
        // Add click event listeners to the buttons
    }

    private void Update()
    {
        // Check for pause input (e.g., 'P' key)
        if (Input.GetKeyDown(KeyCode.P) &&  canPause)
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
        gameplayUI.SetActive(false);
        isPaused = true;
    }


    public void OpenControls()
    {
        pauseMenuUI.SetActive(false);
        controlUI.SetActive(true);
        canPause = false;
    }

    public void CloseControls()
    {
        controlUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        canPause = true;
    }
    
    public void Resume()
    {
        Time.timeScale = 1f; // Resume the game
        pauseMenuUI.SetActive(false);
        gameplayUI.SetActive(true);
        isPaused = false;
        MM.unPause();
    }

    public void Quit()
    {
        // You can add code here to handle quitting the game
        // For example: Application.Quit();
        Application.Quit();
    }
}