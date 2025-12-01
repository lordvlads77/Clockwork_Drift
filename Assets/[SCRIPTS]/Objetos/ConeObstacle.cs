using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class ConeObstacle : MonoBehaviour
{
    [Header("Configuración del obstáculo")]
    [SerializeField] private float recoilForce = 4f;
    [SerializeField] private float conePushForce = 3f;
    [SerializeField] private float speedReductionFactor = 0.5f;
    [SerializeField] private float slowDuration = 2f;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip ImpactClip;
    [SerializeField] private float disappearDelay = 1.5f;

    private bool hasBeenHit = false;
    private Rigidbody2D rb;

  
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        rb.isKinematic = false;
        rb.gravityScale = 0;

        // Guardamos la posición original
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;

      
        GameManager.OnLapCompleted += ResetCone;

        enabled = GameStateManager.Instance.CurrentGameState == GameState.Gameplay;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        GameManager.OnLapCompleted -= ResetCone;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasBeenHit) return;

        if (other.CompareTag("Player"))
        {
            _audioSource.clip = ImpactClip;
            _audioSource.Play();
            hasBeenHit = true;

            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            Vector2 knockDir = (playerRb.position - (Vector2)transform.position).normalized;

            if (playerRb != null)
                playerRb.AddForce(knockDir * recoilForce, ForceMode2D.Impulse);

            rb.AddForce(-knockDir * conePushForce, ForceMode2D.Impulse);

            StartCoroutine(DisappearAfterSeconds());
        }
    }

    private IEnumerator DisappearAfterSeconds()
    {
        yield return new WaitForSeconds(disappearDelay);
        gameObject.SetActive(false);
    }

    
    private void ResetCone(int lap)
    {
        hasBeenHit = false;

        // Restaurar transform original
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Resetear física
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Reactivar
        gameObject.SetActive(true);
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}
