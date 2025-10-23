using UnityEngine;

public class CarSpriteController : MonoBehaviour
{
    [SerializeField] private Sprite[] carSprites;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TopDownMovement movementScript;

    private void Reset()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (movementScript == null)
            movementScript = GetComponent<TopDownMovement>();
    }

    private void Start()
    {
        
        if (carSprites != null && carSprites.Length > 0 && spriteRenderer != null)
        {
            spriteRenderer.sprite = carSprites[0]; 
        }
    }
    
    private void Update()
    {
        float angle = movementScript.CurrentAngle;
        UpdateCarSprite(angle);
    }

    private void UpdateCarSprite(float angle)
    {
        angle = (angle + 360) % 360;

      
        int index = 0;
        if (angle >= 330 || angle < 30) index = 0;       // Derecha (idle)
        else if (angle >= 30 && angle < 90) index = 1;   // Diagonal arriba-derecha
        else if (angle >= 90 && angle < 150) index = 2;  // Arriba
        else if (angle >= 150 && angle < 210) index = 3; // Diagonal arriba-izquierda
        else if (angle >= 210 && angle < 270) index = 4; // Izquierda
        else if (angle >= 270 && angle < 330) index = 5; // Diagonal abajo-derecha

        spriteRenderer.sprite = carSprites[index];
    }
}
