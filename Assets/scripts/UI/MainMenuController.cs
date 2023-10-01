using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame(string scene)
    {
        // Load the "Game" scene
        SceneManager.LoadScene($"{scene}");
    }

    public void QuitGame()
    {
        // Quit the application (only works in a build, not in the editor)
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}