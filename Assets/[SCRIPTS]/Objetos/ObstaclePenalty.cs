using UnityEngine;

[RequireComponent(typeof(ObstacleDetector))]
public class ObstaclePenalty : MonoBehaviour
{
    [SerializeField] private int penaltyPoints = 5;
    private ObstacleDetector detector;

    private void Awake()
    {
        detector = GetComponent<ObstacleDetector>();
    }

    private void OnEnable()
    {
        detector.OnPlayerHit += ApplyPenalty;
    }

    private void OnDisable()
    {
        detector.OnPlayerHit -= ApplyPenalty;
    }

    private void ApplyPenalty()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.SubtractScore(penaltyPoints);
            Debug.Log($"Jugador golpeó obstáculo: -{penaltyPoints} puntos");
        }
    }
}