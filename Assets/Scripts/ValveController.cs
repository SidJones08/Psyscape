using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ValveController : ForceInteractableObjectController 
{
    [SerializeField] private float turnValue;
    [SerializeField] private float turnRate = 0.1f;

    [SerializeField] private float turnActivateValue = 90;
    
    public UnityEvent ActivatePrimary;
    public UnityEvent ActivateSecondary;

    public void ActivatePrimaryAction()
    {
        ActivatePrimary.Invoke();
    }

    public void ActivateSecondayAction()
    {
        ActivateSecondary.Invoke();
    }

    public override void ForceEffectAction(Vector2 dir)
    {
        if (dir.x != 0)
        {
            turnValue -= (turnRate * Time.deltaTime * dir.x);
            transform.eulerAngles = new Vector3(0, 0, turnValue);
        }

        if(turnValue > turnActivateValue)
        {
            ActivatePrimaryAction();
        }
        else if (turnValue < turnActivateValue)
        {
            ActivateSecondayAction();
        }
    }

    public override bool GetIsInfluenced()
    {
        return base.GetIsInfluenced();
    }

    public override Rigidbody2D GetRigidbody2D()
    {
        return GetComponent<Rigidbody2D>();
    }

    public override void SetIsInfluenced(bool value)
    {
        base.SetIsInfluenced(value);
    }

    public override void SetIsPinned(bool value)
    {
        base.SetIsPinned(value);
    }

    public override void UpdateInfluenceState()
    {
        base.UpdateInfluenceState();
    }
}
