using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorial1;

    public void disableTutorial()
    {
        tutorial1.SetActive(false);
    }
    
    public void goToGame(string scene)
    {
        // Load the "Game" scene
        SceneManager.LoadScene($"{scene}");
    }
}
