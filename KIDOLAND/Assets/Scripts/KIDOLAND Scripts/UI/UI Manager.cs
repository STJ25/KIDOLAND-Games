using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Game Over UI")]
    [SerializeField] private CanvasGroup gameOverPanel;

    private void Awake()
    {
        SetGameOverUI(false);
    }

    private void OnEnable()
    {
        GameEvents.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= HandleGameOver;
    }

    private void HandleGameOver()
    {
        SetGameOverUI(true);
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