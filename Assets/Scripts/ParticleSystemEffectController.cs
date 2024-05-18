using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemEffectController : MonoBehaviour
{
    private ParticleSystem particleSystem;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        particleSystem.Play();
    }
}
