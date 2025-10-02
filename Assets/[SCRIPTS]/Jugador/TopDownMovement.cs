using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;

public class TopDownMovement : MonoBehaviour
{
    [FormerlySerializedAs("accelarationF")] [SerializeField] private float _accelarationF = default;
    [SerializeField] private float _turn = default;
    
    [FormerlySerializedAs("accelaration")] [SerializeField] private float _accelaration = default;
    [SerializeField] private float _steering = default;

    private Rigidbody2D _elrigido;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
