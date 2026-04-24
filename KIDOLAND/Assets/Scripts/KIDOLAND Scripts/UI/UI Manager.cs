using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Game Over UI")]
    [SerializeField] private CanvasGroup gameOverPanel;

    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score;

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

    private void HandleGameOver()
    {
        SetGameOverUI(true);
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

    public void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}