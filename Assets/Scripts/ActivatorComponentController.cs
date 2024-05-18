using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivatorComponentController : MonoBehaviour
{
    public UnityEvent ActivatePrimary;
    public UnityEvent ActivateSecondary;

    public virtual void ActivatePrimaryAction()
    {
        ActivatePrimary.Invoke();
    }

    public virtual void ActivateSecondayAction()
    {
        ActivateSecondary.Invoke();
    }
}
