using UnityEngine;
/// <summary>
/// Simple Camera Shake for Game feel inGame.
/// Pretty Self Explanatory
/// </summary>
public class CameraShake : MonoBehaviour
{
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private float magnitude = 0.2f;

    private Vector3 originalPosition;
    private float shakeTimeRemaining;

    private void Awake()
    {
        originalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        GameEvents.OnGameOver += TriggerShake;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= TriggerShake;
    }

    private void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            Vector2 offset = Random.insideUnitCircle * magnitude;
            transform.localPosition = originalPosition + new Vector3(offset.x, offset.y, 0f);

            shakeTimeRemaining -= Time.unscaledDeltaTime; // since i'm pasuing the Game in UI manager

            if (shakeTimeRemaining <= 0f)
            {
                transform.localPosition = originalPosition;
            }
        }
    }

    private void TriggerShake()
    {
        shakeTimeRemaining = duration;
    }
}