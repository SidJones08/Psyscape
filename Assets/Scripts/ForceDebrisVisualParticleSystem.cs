using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceDebrisVisualParticleSystem : MonoBehaviour
{
    private ExpansiveForce expansiveForce;
    private ParticleSystem particleSystem;
    private BurstForceController burstForceController;

    [SerializeField] private float expansiveForceDefaultSpeed = 0.1f;
    [SerializeField] private float expansiveForceMaxSpeed = 1;

    private void Awake()
    {
        burstForceController = FindObjectOfType<BurstForceController>();
        particleSystem = GetComponent<ParticleSystem>();
    }

    public void Start()
    {
        expansiveForce = ExpansiveForce.instance;

        if (expansiveForce)
        {
            expansiveForce.OnExpandEnter += ForceDebrisVisualEnter;
            expansiveForce.OnExpandStay += ForceDebrisVisualStay;
            expansiveForce.OnExpandExit += ForceDebrisVisualExit;

            burstForceController.OnForceBurstEnter += ChangeParticleSpeedMax;
            burstForceController.OnForceBurstExit += ChangeParticleSpeedDefault;
        }
    }

    public void ChangeParticleSpeedDefault()
    {
        var vel = particleSystem.velocityOverLifetime;
        vel.speedModifier = expansiveForceDefaultSpeed;
    }

    public void ChangeParticleSpeedMax(BurstForceController burstForceController, Vector3 pos)
    {
        var vel = particleSystem.velocityOverLifetime;
        vel.speedModifier = expansiveForceMaxSpeed;
    }

    public void ForceDebrisVisualEnter(LoopingSound loopingSound, ExpansiveForce expansiveForce, Vector3 pos)
    {
        particleSystem.Play();
    }

    public void ForceDebrisVisualStay(LoopingSound loopingSound, ExpansiveForce expansiveForce, Vector3 pos)
    {
        var vel = particleSystem.velocityOverLifetime;
        vel.x = expansiveForce.GetForceDirection().normalized.x;
        vel.y = expansiveForce.GetForceDirection().normalized.y;
    }

    public void ForceDebrisVisualExit(LoopingSound loopingSound, ExpansiveForce expansiveForce, Vector3 pos)
    {
        particleSystem.Stop();
    }
}
