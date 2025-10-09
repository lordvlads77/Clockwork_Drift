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
    


    private void Awake()
    {
        Instance = this;
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        ApplyMovementForce();
        ApplySteerinng();
        MurderOrthogonalVelocity();
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
    }

    void ApplySteerinng()
    {
        _minSpeedForTurning = _elrigido.velocity.magnitude / 8;
        _minSpeedForTurning = Mathf.Clamp01(_minSpeedForTurning);
        _rotationAngle -= _steering * _turn * _minSpeedForTurning;
        _elrigido.MoveRotation(_rotationAngle);
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
}
