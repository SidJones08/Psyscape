using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DoorScannerController : DoorController
{
    [SerializeField] private ParticleSystemEffectController deniedEffectController;
    [SerializeField] private ParticleSystemEffectController acceptedEffectController;

    [SerializeField] DoorDetectionController detectionController;

    public override void Awake()
    {
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        detectionController = GetComponentInChildren<DoorDetectionController>();
    }

    public override void Start()
    {
        CloseDoor();

        detectionController.OnScanAccepted += DoorAccepted;
        detectionController.OnScanDenied += DoorDenied;

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

    public override void CloseDoorAnimationEnd()
    {
        Debug.Log("Close End");
        animator.enabled = false;
    }

    public override void OpenDoorAnimationEnd()
    {
        Debug.Log("Open End");
        animator.enabled = false;
    }

    public void DoorAccepted(DoorDetectionController doorDetectionController, Vector3 pos)
    {
        ParticleSystemEffectController psAccepted = ObjectPool.instance.GetObjectFromPool(acceptedEffectController.name).GetComponent<ParticleSystemEffectController>();
        psAccepted.transform.position = transform.position;
        psAccepted.gameObject.SetActive(true);
        detectionController.SetCanScan(false);
        OpenDoor();
    }

    public void DoorDenied(DoorDetectionController doorDetectionController, Vector3 pos)
    {
        ParticleSystemEffectController psDenied = ObjectPool.instance.GetObjectFromPool(deniedEffectController.name).GetComponent<ParticleSystemEffectController>();
        psDenied.transform.position = transform.position;
        psDenied.gameObject.SetActive(true);
    }

    public override void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
            return;

        animator.Play(newState);
        currentState = newState;
    }

    public override bool GetForcePassThrough()
    {
        return base.GetForcePassThrough();
    }

    public override void OpenDoor()
    {
        base.OpenDoor();
    }

    public override void CloseDoor()
    {
        base.CloseDoor();
    }
}
