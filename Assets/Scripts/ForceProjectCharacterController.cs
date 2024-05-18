using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ForceProjectCharacterController : MonoBehaviour
{
    [SerializeField] private float characterForceProject = 10;

    private PlayerController playerController;
    private ExpansiveForce expansiveForce;
    private Rigidbody2D body;

    [SerializeField] private bool forceJumpIsRunning;
    [SerializeField] private float forceJumpDuration = 1;
    [SerializeField] private float forceJumpElapsed = 0;

    [SerializeField] private ParticleSystemEffectController forceDirPrefab;

    public event Action<ForceProjectCharacterController, Vector3> OnForceJumpEnter;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        expansiveForce = GetComponent<ExpansiveForce>();
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (playerController.GetCanAct())
            {
                if (expansiveForce.GetIsExpanding())
                {
                    if (forceJumpIsRunning != true)
                        StartCoroutine(ForceProjectCharacterRoutine(playerController.GetForceDirection()));

                    if (OnForceJumpEnter != null)
                        OnForceJumpEnter(this, transform.position);
                }
            }
        }
    }

    IEnumerator ForceProjectCharacterRoutine(Vector2 dir)
    {
        forceJumpIsRunning = true;

        expansiveForce.ClearExpansiveForce();
        expansiveForce.SetCanExpand(false);
        ForceCharacterDirection(dir, characterForceProject);

        while(forceJumpElapsed < forceJumpDuration)
        {
            forceJumpElapsed += Time.deltaTime;
            yield return null;
        }

        while (!playerController.GetOnGround())
        {
            yield return null;
        }

        expansiveForce.SetCanExpand(true);

        forceJumpElapsed = 0;
        forceJumpIsRunning = false;
    }

    private void ForceCharacterDirection(Vector2 dir, float force)
    {
        if (dir != Vector2.zero)
        {
            body.velocity = (dir * force);
            Debug.Log("Jump: " + (dir * force));
        }
        else
        {
            body.velocity = (Vector2.up * force);
            Debug.Log("Jump: " + Vector2.up * force);
        }
        
        
        ParticleSystemEffectController psForceDir = ObjectPool.instance.GetObjectFromPool(forceDirPrefab.name).GetComponent<ParticleSystemEffectController>();

        if (dir != Vector2.zero)
            psForceDir.transform.rotation = Quaternion.FromToRotation(Vector2.up, -dir);
        else
            psForceDir.transform.rotation = Quaternion.FromToRotation(Vector2.up, -Vector2.up);

        psForceDir.gameObject.transform.position = new Vector2(transform.position.x, transform.position.y) + (dir * 2) ;
        psForceDir.gameObject.SetActive(true);
    }
}
