using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;

public class TopDownMovement : MonoBehaviour
{
    public static TopDownMovement Instance { get; private set; }
    [FormerlySerializedAs("accelarationF")] [SerializeField] private float _accelarationF = default;
    [SerializeField] private float _turn = default;
    [FormerlySerializedAs("accelaration")] [SerializeField] private float _accelaration = default;
    [SerializeField] private float _steering = default;
    [SerializeField] private float _rotationAngle = default;
    [SerializeField] private Rigidbody2D _elrigido;
    [SerializeField] private float _driftFactor = default;
    [SerializeField] private float _minSpeedForTurning = default;
    [SerializeField] private float _maxSpeed = default;
    [SerializeField] private float _velocityRight = default; 
    
    private bool isSlipping = false;
    private float originalDriftFactor;
    private float originalDrag;
    
    private bool isHandbraking = false;
    private float normalDrift;
    private float normalDrag;

    public Animator animator;

    private void Awake()
    {
        Instance = this;
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        
        originalDriftFactor = _driftFactor;
        originalDrag = _elrigido.drag;
        
        animator.SetFloat("Rotation", _rotationAngle);
    }

    private void FixedUpdate()
    {
        if (isSlipping) return; 

        ApplyMovementForce();
        ApplySteerinng();
        MurderOrthogonalVelocity();
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.A))
            animator.SetBool("Left", true);
        if (Input.GetKeyUp(KeyCode.A))
            animator.SetBool("Left", false);

        if (Input.GetKeyDown(KeyCode.D))
            animator.SetBool("Right", true);
        if (Input.GetKeyUp(KeyCode.D))
            animator.SetBool("Right", false);

        if (animator.GetBool("Left") && animator.GetBool("Right"))
        {
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
        }
    }

    void ApplyMovementForce()
    {
        _velocityRight = Vector2.Dot(transform.right, _elrigido.velocity);
        if (_velocityRight > _maxSpeed && _accelaration > 0)
        {
            return;
        }

        if (_velocityRight < -_maxSpeed * 0.5f && _accelaration < 0)
        {
            return;
        }

        if (_elrigido.velocity.sqrMagnitude > _maxSpeed * _maxSpeed && _accelaration > 0)
        {
            return;
        }
        if (_accelaration == 0)
        {
            _elrigido.drag = Mathf.Lerp(_elrigido.drag, 3.0f, Time.fixedDeltaTime * 3);
        }
        else
        {
            _elrigido.drag = 0;
        }
        Vector2 engineForceVector = transform.right * _accelaration * _accelarationF;
        _elrigido.AddForce(engineForceVector, ForceMode2D.Force);
        animator.SetFloat("Move", _velocityRight);
    }

    void ApplySteerinng()
    {
        // Calcula la cantidad de velocidad (0 a 1)
        float speedFactor = Mathf.InverseLerp(0f, _maxSpeed, _elrigido.velocity.magnitude);
    
        // Suaviza la sensibilidad del giro (más estable)
        float steerSensitivity = Mathf.Lerp(0.4f, 1.0f, speedFactor);

        // Calcula el ángulo final con una respuesta más fluida
        _rotationAngle -= _steering * _turn * steerSensitivity;
        // Suaviza el cambio de rotación en lugar de aplicarlo directo
        float smoothRotation = Mathf.LerpAngle(_elrigido.rotation, _rotationAngle, Time.fixedDeltaTime * 8f);
        _elrigido.MoveRotation(smoothRotation);
    }

    void MurderOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.right * Vector2.Dot(_elrigido.velocity, transform.right);
        Vector2 transformUp = transform.up * Vector2.Dot(_elrigido.velocity, transform.up);
        _elrigido.velocity = forwardVelocity + transformUp * _driftFactor;
    }

    public void SetInputVector(Vector2 inputVector)
    {
        _steering = inputVector.x;
        _accelaration = inputVector.y;
    }
    
    public void Slip(float duration, float slipForce)
    {
        if (!isSlipping)
            StartCoroutine(SlipCoroutine(duration, slipForce));
    }

    private IEnumerator SlipCoroutine(float duration, float slipForce)
    {
        isSlipping = true;

        
        _driftFactor = 1f;             
        _elrigido.drag = 0.2f;         
        Vector2 slipDir = _elrigido.velocity.normalized;

        float timer = 0f;
        while (timer < duration)
        {
            _elrigido.AddForce(slipDir * slipForce, ForceMode2D.Force);
            timer += Time.deltaTime;
            yield return null;
        }
        
        _driftFactor = originalDriftFactor;
        _elrigido.drag = originalDrag;
        isSlipping = false;
    }
    
    public void HitCone(float impactForce, float recoveryTime)
    {
        if (!isSlipping)
            StartCoroutine(HitConeCoroutine(impactForce, recoveryTime));
    }

    private IEnumerator HitConeCoroutine(float impactForce, float recoveryTime)
    {
        
        float originalAccel = _accelarationF;
        float originalDrag = _elrigido.drag;
        
        _elrigido.AddForce(-transform.right * impactForce, ForceMode2D.Impulse);
        _elrigido.drag = 5f;
        _accelarationF = 0f;

        yield return new WaitForSeconds(recoveryTime);
        
        _elrigido.drag = originalDrag;
        _accelarationF = originalAccel;
    }
    
    public void Handbrake(bool active)
    {
        if (active && !isHandbraking)
        {
            isHandbraking = true;
            normalDrift = _driftFactor;
            normalDrag = _elrigido.drag;

            _driftFactor = 0.7f;   // Patina más
            _elrigido.drag = 0.9f;  // Menos fricción
        }
        else if (!active && isHandbraking)
        {
            isHandbraking = false;
            _driftFactor = normalDrift;
            _elrigido.drag = normalDrag;
        }
    }
}

