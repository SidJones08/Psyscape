using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DoorController : TileEffectObject
{
    protected TileType.TileCategories tileCategoryModified;
    protected bool isClosed;

    [SerializeField] private bool forcePassThrough = true;

    protected Animator animator;
    protected string currentState;
    protected Collider2D collider;
    
    protected const string DOOR_CLOSE = "Door_Close";
    protected const string DOOR_CLOSE_IDLE = "Door_Close_Idle";
    protected const string DOOR_OPEN = "Door_Open";
    protected const string DOOR_OPEN_IDLE = "Door_Open_Idle";

    public event Action<DoorController, Vector3> OnDoorOpenStart;
    public event Action<DoorController, Vector3> OnDoorCloseStart;

    public event Action<DoorController, Vector3> OnDoorOpenEnd;
    public event Action<DoorController, Vector3> OnDoorCloseEnd;

    public virtual void Awake()
    {
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    public virtual void Start()
    {
        CloseDoor();
        ChangeAnimationState(DOOR_CLOSE_IDLE);
        isClosed = true;
        animator.enabled = false;
    }

    public override TileType.TileCategories GetTileCategory()
    {
        return tileCategoryModified;
    }

    public override void UpdateTileStatus(TileEffectObject tileEffectObject)
    {
        TileMapManager.instance.TileStatusUpdated(tileEffectObject);
    }

    public virtual void OpenDoor()
    {
        animator.enabled = true;

        tileCategoryModified = TileType.TileCategories.DoorOpen;
        collider.isTrigger = true;
        ChangeAnimationState(DOOR_OPEN);
        forcePassThrough = true;
        UpdateTileStatus(this);
        isClosed = false;

    }

    public virtual void CloseDoor()
    {
        animator.enabled = true;

        tileCategoryModified = TileType.TileCategories.DoorClose;
        collider.isTrigger = false;
        ChangeAnimationState(DOOR_CLOSE);
        forcePassThrough = false;
        UpdateTileStatus(this);
        isClosed = true;
    }

    public virtual void CloseDoorAnimationEnd() 
    {
        //Debug.Log("Close End");
        animator.enabled = false;
    }

    public virtual void CloseDoorAnimationEventStart()
    {
        //Debug.Log("CloseDoorAnimationEventStart");

        if (OnDoorCloseStart != null)
            OnDoorCloseStart(this, transform.position);
    }

    public virtual void OpenDoorAnimationEventStart()
    {
        if (OnDoorOpenStart != null)
            OnDoorOpenStart(this, transform.position);
    }

    public virtual void CloseDoorAnimationEventEnd()
    {
        if (OnDoorCloseEnd != null)
            OnDoorCloseEnd(this, transform.position);
    }

    public virtual void OpenDoorAnimationEventEnd()
    {
        if (OnDoorOpenEnd != null)
            OnDoorOpenEnd(this, transform.position);
    }

    public virtual void OpenDoorAnimationEnd()
    {
        //Debug.Log("Open End");
        animator.enabled = false;
    }

    public virtual void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
            return;

        animator.Play(newState);
        currentState = newState;
    }

    public override bool GetForcePassThrough()
    {
        return forcePassThrough;
    }
}
