using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JumpController : MonoBehaviour
{
    [SerializeField] private float characterForceJump = 5f;

    private PlayerController playerController;
    private ExpansiveForce expansiveForce;

    private Rigidbody2D body;

    public event Action<JumpController, Vector3> OnJumpAction;

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
                if (playerController.GetOnGround() && !playerController.GetIsClimbing())
                {
                    if (!expansiveForce.GetIsExpanding())
                        JumpAction();
                }
            }
        }
    }

    private void JumpAction()
    {
        body.velocity = (Vector2.up * characterForceJump);

        if (OnJumpAction != null)
            OnJumpAction(this, transform.position);
    }
}
