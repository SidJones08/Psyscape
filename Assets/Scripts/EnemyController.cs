using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : MovementController
{
    [Header("Movement")]
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    //Higher more responsive, Lower more heavy
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;

    [Header("Friction")]
    [SerializeField] private float friction;

    [Header("Direction")]
    [SerializeField] private Vector2 direction;
    [SerializeField] private Vector2 desiredVelocity;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private Rigidbody2D body;

    [Header("Status")]
    [SerializeField] private bool isFollowingPlayer = true;
    [SerializeField] private bool onGround;
    [SerializeField] private bool isClimbing;
    [SerializeField] private bool isInfluenced;

    [SerializeField] private bool knockPermenant;
    [SerializeField] private bool isKnocked;
    [SerializeField] private float isKnockedDuration = 3;
    [SerializeField] private float knockedOutThreshold = 1;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private bool hasReachedTarget;
    [SerializeField] private float hasReachedTargetRefreshRate = 0.1f;
    [SerializeField] private float reachTargetDistance = 2f;

    [Header("Effect")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private ParticleSystemEffectController ImpactEffectController;

    [Header("AI Movement")]
    [SerializeField] private int moveTargetIndex;
    [SerializeField] private float characterForceImpuse = 3;

    public event Action<EnemyController, Vector3> OnEnemyKnockedOut;

    private ForceInteractableObjectController forceInteractableObject;
    private ActorNavigationController actorNavigationController;

    private float maxSpeedChange;
    private float acceleration;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        actorNavigationController = GetComponent<ActorNavigationController>();
        forceInteractableObject = GetComponent<ForceInteractableObjectController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        defaultMaterial = spriteRenderer.material;
        forceInteractableObject.OnInfluenceChange += UpdateIsInfluenced;
        actorNavigationController.OnPathUpdate += PathUpdate;
    }

    public void PathUpdate()
    {
        if (isFollowingPlayer)
        {
            if (!isKnocked)
            {
                if (Vector2.Distance(transform.position, playerController.transform.position) > reachTargetDistance)
                {
                    if (desiredVelocity.x > 0)
                        spriteRenderer.flipX = false;
                    else if (desiredVelocity.x < 0)
                        spriteRenderer.flipX = transform;

                    if (!isInfluenced)
                    {
                        if (direction != Vector2.zero)
                            desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - GetFriction());
                        else
                            desiredVelocity = Vector2.zero;

                        if (actorNavigationController.GetPath().Count > 0)
                        {
                            if (moveTargetIndex < actorNavigationController.GetPath().Count)
                            {
                                if (Vector2.Distance(transform.position, actorNavigationController.GetPath()[moveTargetIndex]) > actorNavigationController.GetMaxTargetDistanceCurrent())
                                {
                                    Vector2 dir = actorNavigationController.GetPath()[moveTargetIndex] - transform.position;
                                    direction = dir.normalized;
                                }
                                else
                                {
                                    moveTargetIndex++;
                                }
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            moveTargetIndex = 0;
                            direction = Vector2.zero;
                        }
                    }
                }
                else
                {
                    if (!hasReachedTarget)
                        StartCoroutine("ReachTargetRoutine");

                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isKnocked)
        {
            if (isClimbing)
            {
                body.velocity = new Vector2(body.velocity.x, direction.y * maxSpeed);
                actorNavigationController.SetMaxTargetDistanceCurrent(actorNavigationController.GetMaxTargetDistanceLadder());
            }
            else
            {
                actorNavigationController.SetMaxTargetDistanceCurrent(actorNavigationController.GetMaxTargetDistanceDefault());
            }

            velocity = body.velocity;
            acceleration = onGround ? maxAcceleration : maxAirAcceleration;
            maxSpeedChange = acceleration * Time.deltaTime;
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
            body.velocity = velocity;
        }
    }

    public void KnockedOut()
    {
        if (!isKnocked)
        {
            if (OnEnemyKnockedOut != null)
                OnEnemyKnockedOut(this, transform.position);

            if (knockPermenant)
            {
                isKnocked = true;
                actorNavigationController.StopPathfindingRoutine();
            }
            else
            {
                StartCoroutine(KnockedOutRoutine());
            }
        }
    }


    IEnumerator KnockedOutRoutine()
    {
        isKnocked = true;

        actorNavigationController.StopPathfindingRoutine();

        while (isKnocked)
        {
            yield return new WaitForSeconds(isKnockedDuration);
            isKnocked = false;
        }

        actorNavigationController.StartPathfindingRoutine();
    }

    public override bool GetHasReachedTarget()
    {
        return hasReachedTarget;
    }

    IEnumerator ReachTargetRoutine()
    {
        hasReachedTarget = true;
        actorNavigationController.StopPathfindingRoutine();
        desiredVelocity = Vector2.zero;

        while (hasReachedTarget)
        {
            if (Vector2.Distance(transform.position, playerController.transform.position) > reachTargetDistance)
            {
                hasReachedTarget = false;
                actorNavigationController.StartPathfindingRoutine();
            }

            yield return new WaitForSeconds(hasReachedTargetRefreshRate);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);

        if (collision.transform.GetComponent<ButtonController>())
            if (onGround && !isClimbing && !isKnocked)
                ForceCharacterDirection(Vector2.up);

        if (collision.transform.GetComponent<Rigidbody2D>())
            if (collision.transform.GetComponent<Rigidbody2D>().velocity.magnitude > knockedOutThreshold)
                KnockedOut();

        if (velocity.magnitude > knockedOutThreshold)
        {
            ParticleSystemEffectController impactPS = ObjectPool.instance.GetObjectFromPool(ImpactEffectController.name).GetComponent<ParticleSystemEffectController>();
            impactPS.transform.rotation = Quaternion.FromToRotation(Vector2.up, collision.GetContact(0).normal);
            impactPS.transform.position = collision.GetContact(0).point;
            impactPS.gameObject.SetActive(true);

            KnockedOut();
        }
    }

    private void ForceCharacterDirection(Vector2 dir)
    {
        body.velocity = (dir * characterForceImpuse);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onGround = false;
        friction = 0;
    }

    private void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;
            onGround |= normal.y >= 0.9f;
        }
    }

    public void UpdateIsInfluenced()
    {
        isInfluenced = forceInteractableObject.GetIsInfluenced();
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

    public override bool GetIsKnocked()
    {
        return isKnocked;
    }

    public override bool GetIsPinned()
    {
        return forceInteractableObject.GetIsPinned();
    }

    public override bool GetIsInfluenced()
    {
        return isInfluenced;
    }

    public override Vector2 GetDesiredVelocity()
    {
        return desiredVelocity;
    }

    public override Vector2 GetVelocity()
    {
        return velocity;
    }

    public override void SetOnGround(bool value)
    {
        onGround = value;
    }

    public override void SetCharacterMaterial(Material material)
    {
        spriteRenderer.material = material;
    }

    public override Material GetDefaultMaterial()
    {
        return defaultMaterial;
    }

    private void OnDrawGizmos()
    {
        if (actorNavigationController)
        {
            if (actorNavigationController.GetPath().Count > 0)
            {
                Gizmos.color = Color.green;
                if (moveTargetIndex < actorNavigationController.GetPath().Count - 1)
                    Gizmos.DrawWireCube(actorNavigationController.GetPath()[moveTargetIndex], Vector2.one);

                for (int i = 0; i < actorNavigationController.GetPath().Count - 1; i++)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(actorNavigationController.GetPath()[i], actorNavigationController.GetPath()[i + 1]);
                }
            }
        }
    }
}
