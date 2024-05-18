using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BurstForceController : MonoBehaviour
{
    [SerializeField] private float burstForce = 10f;
    [SerializeField] private bool isBurstForce;
    [SerializeField] private float burstForceDuration = 0.1f;

    public event Action<BurstForceController, Vector3> OnForceBurstEnter;
    public event Action OnForceBurstExit;

    private ExpansiveForce expansiveForce;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        expansiveForce = GetComponent<ExpansiveForce>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            if (playerController.GetCanAct())
                if(expansiveForce.GetIsExpanding())
                BurstForce();
    }

    private void FixedUpdate()
    {
        if (isBurstForce)
        {
            if (expansiveForce.GetForceInteractableObjectControllers().Count > 0)
            {
                List<ForceInteractableObjectController> forceInteractables = new List<ForceInteractableObjectController>();
                forceInteractables.AddRange(expansiveForce.GetForceInteractableObjectControllers());

                for (int i = 0; i < forceInteractables.Count; i++)
                {
                    if (forceInteractables[i].GetIsPinned())
                        forceInteractables[i].OnPinExit();

                    if (!forceInteractables[i].GetIsInfluenceImmunity())
                    {
                        forceInteractables[i].StartInfluenceImmunityRoutine();
                        forceInteractables[i].SetIsInfluenced(true);
                        forceInteractables[i].UpdateInfluenceState();
                        forceInteractables[i].GetRigidbody2D().AddForce(expansiveForce.GetForceDirection().normalized * burstForce, ForceMode2D.Impulse);
                    }

                    //Debug.Log("Thrown: " + forceInteractables[i].GetRigidbody2D().velocity);
                }
            }
        }
    }

    public void BurstForce()
    {
        if (!isBurstForce)
            StartCoroutine(BurstForceRoutine());
    }

    IEnumerator BurstForceRoutine()
    {
        if (OnForceBurstEnter != null)
            OnForceBurstEnter(this, transform.position);

        isBurstForce = true;

        while (isBurstForce)
        {
            yield return new WaitForSeconds(burstForceDuration);
            isBurstForce = false;
        }

        if (OnForceBurstExit != null)
            OnForceBurstExit();
    }

    public bool GetIsBurstForce()
    {
        return isBurstForce;
    }
}
