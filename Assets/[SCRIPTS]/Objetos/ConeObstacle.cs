using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class ConeObstacle : MonoBehaviour
{
    [FormerlySerializedAs("RecoilForce")]
    [Header("Configuración del obstáculo")]
    [SerializeField] private float recoilForce = 4f; // fuerza de retroceso al jugador
    [SerializeField] private float conePushForce = 3f; // fuerza con la que el cono es empujado
    [SerializeField] private float speedReductionFactor = 0.5f; // reducción de velocidad (50%)
    [SerializeField] private float slowDuration = 2f; // duración de la penalización
    [SerializeField] private float disappearDelay = 1.5f; // tiempo antes de desaparecer el cono

    private bool hasBeenHit = false;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
        rb.gravityScale = 0;    
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasBeenHit) return;

        if (other.CompareTag("Player"))
        {
            hasBeenHit = true;

            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            Movement playerMove = other.GetComponent<Movement>();
            
            Vector2 knockDir = (playerRb.position - (Vector2)transform.position).normalized;
            
            if (playerRb != null)
            {
                playerRb.AddForce(knockDir * recoilForce, ForceMode2D.Impulse);
            }
            
            if (playerMove != null)
            {
                playerMove.ApplySpeedModifier(speedReductionFactor, slowDuration);
            }
            
            rb.AddForce(-knockDir * conePushForce, ForceMode2D.Impulse);
            
            StartCoroutine(DisappearAfterSeconds());
        }
    }

    private IEnumerator DisappearAfterSeconds()
    {
        yield return new WaitForSeconds(disappearDelay);
        gameObject.SetActive(false);
    }
}
