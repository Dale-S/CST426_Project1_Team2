using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        // Load the "Game" scene
        SceneManager.LoadScene("Playtest2");
    }

    public void QuitGame()
    {
        // Quit the application (only works in a build, not in the editor)
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}