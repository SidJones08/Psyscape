using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : ForceInteractableObjectController
{
    public override void Awake()
    {
        base.Awake();
    }

    public override bool GetIsInfluenced()
    {
        return base.GetIsInfluenced();
    }

    public override bool GetIsInfluenceImmunity()
    {
        return base.GetIsInfluenceImmunity();
    }

    public override bool GetIsPinned()
    {
        return base.GetIsPinned();
    }

    public override Rigidbody2D GetRigidbody2D()
    {
        return base.GetRigidbody2D();
    }

    public override void SetIsInfluenced(bool value)
    {
        base.SetIsInfluenced(value);
    }

    public override void SetIsPinned(bool value)
    {
        base.SetIsPinned(value);
    }

    public override void StartInfluenceImmunityRoutine()
    {
        base.StartInfluenceImmunityRoutine();
    }

    public override void UpdateInfluenceState()
    {
        base.UpdateInfluenceState();
    }

    Vector2 forceDirection;

    [SerializeField] private float turnSpeed = 1f;
    [SerializeField] private float missileSpeed = 10f;

    public override void ForceEffectAction(Vector2 dir)
    {
        if (dir != Vector2.zero)
        {
            float forceDirectionDegrees = Vector2.Angle(dir.normalized, Vector2.up);
            float angle = Vector2.Angle(dir.normalized, Vector2.left);

            if (angle > 90)
                forceDirectionDegrees = 360 - forceDirectionDegrees;

            Quaternion from = this.transform.rotation;
            Quaternion to = Quaternion.Euler(0, 0, forceDirectionDegrees);

            transform.rotation = Quaternion.Slerp(from, to, Time.deltaTime * turnSpeed);
        }
    }

    /*
    private void Update()
    {
        forceDirection.x = Input.GetAxisRaw("Horizontal");
        forceDirection.y = Input.GetAxisRaw("Vertical");

        if(forceDirection != Vector2.zero)
        {
            float forceDirectionDegrees = Vector2.Angle(forceDirection.normalized, Vector2.up);
            float angle = Vector2.Angle(forceDirection.normalized, Vector2.left);

            if (angle > 90)
                forceDirectionDegrees = 360 - forceDirectionDegrees;

            Quaternion from = this.transform.rotation;
            Quaternion to = Quaternion.Euler(0, 0, forceDirectionDegrees);

            transform.rotation = Quaternion.Slerp(from, to, Time.deltaTime * turnSpeed);
        }
    }
    */

    private void FixedUpdate()
    {
        GetRigidbody2D().velocity = transform.up * (Time.fixedDeltaTime * missileSpeed);
    }

    //Temp
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetRigidbody2D().angularVelocity = 0;
    }

    public override void OnPinEnter()
    {
        SetIsPinned(true);
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public override void OnPinExit()
    {
        SetIsPinned(false);
        rigidbody2D.constraints = RigidbodyConstraints2D.None;
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
