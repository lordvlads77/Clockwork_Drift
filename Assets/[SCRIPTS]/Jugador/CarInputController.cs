using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputController : MonoBehaviour
{
    public static CarInputController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        Vector2 inputVector = Vector2.zero;
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");
        
        TopDownMovement.Instance.SetInputVector(inputVector);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TopDownMovement.Instance.Handbrake(true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            TopDownMovement.Instance.Handbrake(false);
        }
    }
}
