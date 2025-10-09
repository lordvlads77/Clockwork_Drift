using UnityEngine;
public class OilSpill : MonoBehaviour
{
    [SerializeField] private float slipDuration = 2f;
    [SerializeField] private float slipForce = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TopDownMovement car = collision.GetComponent<TopDownMovement>();
        if (car != null)
        {
            car.Slip(slipDuration, slipForce);
        }
    }
}

