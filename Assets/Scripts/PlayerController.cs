using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MovementController
{
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;

    [SerializeField] private float friction;
    [SerializeField] private Vector2 direction;
    [SerializeField] private Vector2 desiredVelocity;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private Vector2 forceDirection;

    [SerializeField] private bool onGround;
    [SerializeField] private bool isClimbing;
    [SerializeField] private bool isExpandingForce;
    [SerializeField] private bool isUpgrading;

    [SerializeField] private bool canMove;
    [SerializeField] private bool canAct;

    [SerializeField] private float dragDefault = 0;
    [SerializeField] private float dragMax = 5;

    [SerializeField] private Vector2 lastGroundedPosition;

    [SerializeField] private Material defaultMaterial;

    public event Action<PlayerController, Vector3> OnLanded;
    
    private float maxSpeedChange;
    private float acceleration;

    ExpansiveForce expansiveForce;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        expansiveForce = GetComponent<ExpansiveForce>();
    }

    private void Start()
    {
        defaultMaterial = spriteRenderer.material;

        expansiveForce.OnExpandEnter += UpdateIsForceExpanding;
        expansiveForce.OnExpandExit += UpdateIsForceExpanding;
        expansiveForce.OnExpandEnter += ChangeRigidbodyDragToMax;
        expansiveForce.OnExpandExit += ChangeRigidbodyDragToDefault;
    }

    private void Update()
    {
        forceDirection = expansiveForce.GetForceDirection();

        if (!expansiveForce.GetIsExpanding())
        {
            direction.x = Input.GetAxisRaw("Horizontal");
            direction.y = Input.GetAxisRaw("Vertical");

            if(canMove)
            desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - GetFriction(), 0);

            if (desiredVelocity.x > 0)
                spriteRenderer.flipX = false;
            else if (desiredVelocity.x < 0)
                spriteRenderer.flipX = transform;
        }
        else
        {
            if (forceDirection.x > 0)
                spriteRenderer.flipX = false;
            else if (forceDirection.x < 0)
                spriteRenderer.flipX = transform;

            desiredVelocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        velocity = body.velocity;
        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        body.velocity = velocity;

        if (isClimbing)
        {
            body.velocity = new Vector2(body.velocity.x, direction.y * maxSpeed);
        }
    }

    public override void OnClimbEnter()
    {
        isClimbing = true;
        body.gravityScale = 0;
    }

    public override void OnClimbExit()
    {
        isClimbing = false;
        body.gravityScale = 1;
    }

    private void UpdateIsForceExpanding(LoopingSound loopingSound, ExpansiveForce expansiveForce, Vector3 pos)
    {
        isExpandingForce = expansiveForce.GetIsExpanding();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (onGround)
        {
            if (OnLanded != null)
            {
                OnLanded(this, transform.position);
            }
        }

        EvaluateCollisionEnter(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollisionStay(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(velocity.y == 0)
        lastGroundedPosition = transform.position;
        
        onGround = false;
        friction = 0;
    }

    private void EvaluateCollisionEnter(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;
            onGround |= normal.y >= 0.9f;
        }
    }


    private void EvaluateCollisionStay(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;
            onGround |= normal.y >= 0.9f;
        }
    }

    private void ChangeRigidbodyDragToDefault(LoopingSound loopingSound, ExpansiveForce expansiveForce, Vector3 pos)
    {
        body.drag = dragDefault;
    }

    private void ChangeRigidbodyDragToMax(LoopingSound loopingSound, ExpansiveForce expansiveForce, Vector3 pos)
    {
        body.drag = dragMax;
    }

    public void SetIsUpgrading(bool value)
    {
        isUpgrading = value;
    }

    public void SetDesiredVelocity(Vector2 value)
    {
        desiredVelocity = value;
    }

    public bool GetIsUpgrading()
    {
        return isUpgrading;
    }

    public override bool GetOnGround()
    {
        return onGround;
    }

    public float GetFriction()
    {
        return friction;
    }

    public Rigidbody2D GetRigidbody()
    {
        return body;
    }

    public Vector2 GetDirection()
    {
        return velocity;
    }

    public override bool GetIsClimbing()
    {
        return isClimbing;
    }

    public override Vector2 GetDesiredVelocity()
    {
        return desiredVelocity;
    }

    public override Vector2 GetVelocity()
    {
        return velocity;
    }

    public override bool GetIsForceExpanding()
    {
        return isExpandingForce;
    }

    public override Vector2 GetForceDirection()
    {
        return forceDirection;
    }

    public Vector2 GetLastGroundedPosition()
    {
        return lastGroundedPosition;
    }

    public override void SetCharacterMaterial(Material material)
    {
        spriteRenderer.material = material;
    }

    public override Material GetDefaultMaterial()
    {
        return defaultMaterial;
    }

    public override void SetCanMove(bool value)
    {
        canMove = value;
    }

    public override void SetCanAct(bool value)
    {
        canAct = value;
    }

    public override bool GetCanMove()
    {
        return canMove;
    }

    public override bool GetCanAct()
    {
        return canAct;
    }
}
