using UnityEngine;
using UnityEngine;

public class OilSpill : MonoBehaviour
{
    [SerializeField] private float slipDuration = 4f; // duración del efecto
    [SerializeField] private float slipForce = 5f;    // fuerza del deslizamiento

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Movement car = collision.GetComponent<Movement>();
        if (car != null)
        {
            car.Slip(slipDuration, slipForce);
        }
    }
}
