using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PinForceController : MonoBehaviour
{
    private ExpansiveForce expansiveForce;
    private PlayerController playerController;

    public event Action<PinForceController, Vector3> OnPinActionEnter;

    private void Awake()
    {
        expansiveForce = GetComponent<ExpansiveForce>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            if(playerController.GetCanAct())
            PinAllForceInteractables();
    }

    public void PinAllForceInteractables()
    {
        if (GetNumberOfPinableObjects(expansiveForce) > 0)
            if (OnPinActionEnter != null)
                OnPinActionEnter(this, transform.position);

        if (expansiveForce.GetForceInteractableObjectControllers().Count > 0)
            for (int i = 0; i < expansiveForce.GetForceInteractableObjectControllers().Count; i++)
                if (!expansiveForce.GetForceInteractableObjectControllers()[i].GetIsPinned())
                    expansiveForce.GetForceInteractableObjectControllers()[i].OnPinEnter();
    }

    public int GetNumberOfPinableObjects(ExpansiveForce expansiveForce)
    {
        int num = 0;

        for (int i = 0; i < expansiveForce.GetForceInteractableObjectControllers().Count; i++)
            if (!expansiveForce.GetForceInteractableObjectControllers()[i].GetIsPinned())
                num++;

        return num;
    }
}
