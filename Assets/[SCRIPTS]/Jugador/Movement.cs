using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movimiento")]
    public float ForceMove;

    private Vector2 move;

    // I commented this just in the odd chance that we will need it in the future. doubtful but still
    /*[Header("Verificar Suelo")]
    public Vector3 checkforFloorPos;
    public float checkforFloorRadio;
    public LayerMask checkFloorMask;
    public bool isGround;*/
    
    private Rigidbody2D rigi;

    [Header("FlipPlayer")]
    [SerializeField] private KeyCode _horizonL = default;
    [SerializeField] private KeyCode _horizonR = default;

    //This is only here as a reference for when we do Animations and they will be done in a separate script if possible
    //private int walksie = Animator.StringToHash("xSpeed");

    // Start is called before the first frame update
    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");
        //isGround = Physics2D.OverlapCircle(transform.position + checkforFloorPos, checkforFloorRadio, checkFloorMask);
        rigi.AddForce(move * ForceMove);
        //Same here just as reference.
        //_animator.SetFloat(walksie, Math.Abs(move.x));
        Flipeando(move.x);
        UpDown(move.y);
    }


    void Update()
    {
        if (Input.GetKeyDown(_horizonL))
        {
            transform.Rotate(0f, 180f, 0f);
        }
        if (Input.GetKeyDown(_horizonR))
        {
            transform.Rotate(0f, -180f,0f);
        }
    }
    
    /*private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + checkforFloorPos, checkforFloorRadio);
    }*/

    private void Flipeando(float _dirX)
    {
        Vector3 localScale = transform.localScale;
        if (_dirX > 0)
        {
            localScale.x = 1f;
        }
        else if (_dirX < 0f)
        {
            localScale.x = -1f;
        }

        transform.localScale = localScale;
    }

    private void UpDown(float _dirY)
    {
        Vector3 localScale = transform.localScale;
        if (_dirY > 0)
        {
            localScale.y = 1f;
        }
        else if (_dirY < 0f)
        {
            localScale.y = -1f;
        }
        
        transform.localScale = localScale;
    }
}
