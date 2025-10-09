using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Rigidbody2D))]

public class DynamicCameraOffset : MonoBehaviour
{
    [Header("Referencias")]
    public CinemachineVirtualCamera vcam;

    [Header("Configuración")]
    public float offsetDistance = 2f;     // Qué tan lejos se adelanta la cámara en dirección del movimiento
    public float smoothSpeed = 5f;        // Suavidad del movimiento

    private Rigidbody2D rb;
    private CinemachineFramingTransposer transposer;
    private Vector3 currentOffset;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (vcam != null)
            transposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();

        if (transposer != null)
            currentOffset = transposer.m_TrackedObjectOffset;
    }

    void LateUpdate()
    {
        if (rb == null || transposer == null)
            return;

        Vector2 velocity = rb.velocity;

        // Si el jugador se está moviendo
        if (velocity.sqrMagnitude > 0.01f)
        {
            // Calcula dirección de movimiento normalizada
            Vector2 moveDir = velocity.normalized;

            // Calcula offset deseado según dirección del movimiento
            Vector3 targetOffset = new Vector3(moveDir.x, moveDir.y, 0f) * offsetDistance;

            // Interpolación suave hacia el nuevo offset
            currentOffset = Vector3.Lerp(currentOffset, targetOffset, Time.deltaTime * smoothSpeed);
        }
        else
        {
            // Si no hay movimiento, regresa lentamente al centro
            currentOffset = Vector3.Lerp(currentOffset, Vector3.zero, Time.deltaTime * smoothSpeed);
        }

        // Aplica el offset al FramingTransposer
        transposer.m_TrackedObjectOffset = currentOffset;
    }
}
