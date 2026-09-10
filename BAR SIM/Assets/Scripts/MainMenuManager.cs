using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string gameSceneName = "BarScene";

    // Starts the research session
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Closes the application
    public void QuitGame()
    {
        Application.Quit();
    }
}