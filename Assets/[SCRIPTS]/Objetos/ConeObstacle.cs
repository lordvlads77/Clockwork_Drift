using UnityEngine;

public class ConeObstacle : MonoBehaviour
{
    [Header("Configuración del obstáculo")]
    [SerializeField] private float knockbackForce = 4f; // fuerza de retroceso
    [SerializeField] private float speedReductionFactor = 0.5f; // reducción de velocidad (50%)
    [SerializeField] private float slowDuration = 2f; // duración de la penalización
    [SerializeField] private float disappearDelay = 1.5f; // tiempo antes de desaparecer el cono

    private bool hasBeenHit = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasBeenHit) return;

        if (other.CompareTag("Player"))
        {
            hasBeenHit = true;

            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            Movement playerMove = other.GetComponent<Movement>();

            if (playerRb != null)
            {
                // Empuje en dirección contraria al impacto
                Vector2 knockDir = (playerRb.position - (Vector2)transform.position).normalized;
                playerRb.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);
            }

            if (playerMove != null)
            {
                playerMove.ApplySpeedModifier(speedReductionFactor, slowDuration);
            }

            // Desaparecer el cono tras unos segundos
            StartCoroutine(DisappearAfterSeconds());
        }
    }

    private System.Collections.IEnumerator DisappearAfterSeconds()
    {
        yield return new WaitForSeconds(disappearDelay);
        gameObject.SetActive(false);
    }
}

