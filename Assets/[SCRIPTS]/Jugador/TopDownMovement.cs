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
    }

    void ApplyMovementForce()
    {
        Vector2 engineForceVector = transform.up * _accelaration * _accelarationF;
        
        _elrigido.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteerinng()
    {
        _rotationAngle -= _steering * _turn;
        
        _elrigido.MoveRotation(_rotationAngle);
    }

    void MurderOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(_elrigido.velocity, transform.up);
        
    }

    public void SetInputVector(Vector2 inputVector)
    {
        _steering = inputVector.x;
        _accelaration = inputVector.y;
    }
}
