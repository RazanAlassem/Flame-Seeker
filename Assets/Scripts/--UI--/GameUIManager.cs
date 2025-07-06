using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    // Singleton pattern
    public static GameUIManager Instance;

    // UI panels
    public GameObject gameOverPanel;
    public GameObject winPanel;

    // Buttons
    public Button gameOverRetryButton;
    public Button winRetryButton;


    void Awake()
    {
        // Set up singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Hide panels at start
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
    }

    // Show game over screen
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    // Show win screen
    public void ShowWin()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }

    // Restart the current level
    public void RestartGame()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
    // Load the next level
public void LoadNextLevel()
 {
     Time.timeScale = 1f;
     int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
     if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
     {
         SceneManager.LoadScene(nextSceneIndex);
     }
     else
     {
         Debug.Log("No more levels in build settings.");
     }
 }
 
  public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f; 
    }
}