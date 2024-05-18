using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WiggleController : ForceInteractableObjectController, LoopingSound
{
    //In this instance Wiggle Controller is also Vent Entrance Controller

    [SerializeField] private float wiggleDistance = 1;
    [SerializeField] private float wiggleSpeed = 1;
    [SerializeField] private Vector2 wiggleDir;
    [SerializeField] private Vector2 wiggleReleaseDir;

    [SerializeField] private bool isReleased;
    [SerializeField] private bool biDirectional;
    [SerializeField] private bool resetOnRelease;
    [SerializeField] private bool alignReleaseToTransform;
    [SerializeField] private bool isWiggling;

    [SerializeField] private float releaseProgress = 1;
    [SerializeField] private float releaseProgressElapsed;
    [SerializeField] private float releaseProgreeRate = 0.01f;

    private Vector2 startPos;

    public event Action<LoopingSound, WiggleController, Vector3> OnWiggleUpdateEnter;
    public event Action<LoopingSound, WiggleController, Vector3> OnWiggleUpdateExit;
    public event Action<WiggleController, Vector3> OnWiggleRelease;
    public event Action<WiggleController, Vector3> OnWiggleCollision;

    public virtual void Start()
    {
        startPos = transform.position;

        if (alignReleaseToTransform)
        {
            wiggleDir = transform.up;
            wiggleReleaseDir = transform.up;
        }
    }

    public virtual void WiggleAction()
    {
        if (!isWiggling)
            if (OnWiggleUpdateEnter != null)
                OnWiggleUpdateEnter(this, this, transform.position);

        isWiggling = true;

        float xPosition = Mathf.Sin(Time.time * wiggleSpeed) * (wiggleDir.normalized.x * wiggleDistance);
        float yPosition = Mathf.Sin(Time.time * wiggleSpeed) * (wiggleDir.normalized.y * wiggleDistance);
        transform.position = new Vector3(startPos.x + xPosition, startPos.y + yPosition);

        if (!isReleased)
        {
            if (releaseProgressElapsed < releaseProgress)
            {
                releaseProgressElapsed += releaseProgreeRate * Time.deltaTime;
            }
            else
            {
                Release();
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (OnWiggleCollision != null)
            OnWiggleCollision(this, transform.position);
    }

    public virtual void Release()
    {
        isReleased = true;
        GetRigidbody2D().isKinematic = false;

        if (OnWiggleRelease != null)
            OnWiggleRelease(this, transform.position);

        isWiggling = false;

        if (OnWiggleUpdateExit != null)
            OnWiggleUpdateExit(this, this, transform.position);
    }

    public override void ForceEffectAction(Vector2 dir)
    {
        if (!isReleased)
        {
            if (biDirectional)
            {
                if (dir == wiggleDir || dir == -wiggleDir)
                {

                    WiggleAction();
                }
                else
                {
                    isWiggling = false;

                    if (OnWiggleUpdateExit != null)
                        OnWiggleUpdateExit(this, this, transform.position);
                }
            }
            else
            {
                if (dir == wiggleDir)
                {
                    WiggleAction();
                }
                else
                {
                    isWiggling = false;

                    if (OnWiggleUpdateExit != null)
                        OnWiggleUpdateExit(this, this, transform.position);
                }
            }
        }
    }

    public override void UpdateInfluenceState()
    {
        base.UpdateInfluenceState();
    }

    public override void SetIsInfluenced(bool value)
    {
        if (!isReleased)
        {
            base.SetIsInfluenced(value);

            //Only works when releasing space
            if (!GetIsInfluenced())
            {
                transform.position = startPos;
            }

            if (resetOnRelease)
                releaseProgressElapsed = 0;
        }
    }

    public override Rigidbody2D GetRigidbody2D()
    {
        return base.GetRigidbody2D();
    }

    public override bool GetIsInfluenced()
    {
        return base.GetIsInfluenced();
    }
}
