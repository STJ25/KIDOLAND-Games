using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
/// <summary>
/// UI Monobehaviour Singleton that Handles UI logic and events.
/// Subscribes to events to update Score UI.
/// Uses Restart level to replay level using Scene Management.
/// </summary>
public class UIManager : MonoBehaviour
{
#region Inspector

    [Header("Game Over UI")]
    [SerializeField] private CanvasGroup gameOverPanel;

    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score;
#endregion

#region Unity Cycle and Events Subs
    private void Awake()
    {
        SetGameOverUI(false);
        UpdateScoreUI();
    }

    private void OnEnable()
    {
        GameEvents.OnGameOver += HandleGameOver;
        GameEvents.OnCollectiblePicked += HandleCollectible;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= HandleGameOver;
        GameEvents.OnCollectiblePicked -= HandleCollectible;
    }
#endregion

#region Private Events and Functions

    private void HandleGameOver()
    {
        SetGameOverUI(true);
        Time.timeScale = 0f; // Stop moving
    }

    private void HandleCollectible(int value)
    {
        score += value;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }

    private void SetGameOverUI(bool state)
    {
        gameOverPanel.alpha = state ? 1f : 0f;
        gameOverPanel.interactable = state;
        gameOverPanel.blocksRaycasts = state;
    }
#endregion

    // Set on the button to restart level.
    public void RestartLevel()
    {
        Time.timeScale = 1f; // set evrything to normal
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}