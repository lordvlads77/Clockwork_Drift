using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashEffect : MonoBehaviour
{
    [SerializeField] private string tagObjetivo = "Wall";
    public ParticleSystem crashParticle;

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag(tagObjetivo))
        {
            Debug.Log("El objeto con la tag " + tagObjetivo + " ha entrado en el trigger!");
            crearParticula();
        }
    }

    void crearParticula ()
    {
        crashParticle.Play();
    }
}
