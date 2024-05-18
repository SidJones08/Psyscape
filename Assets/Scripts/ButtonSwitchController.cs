using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonSwitchController : ButtonController
{
    public event Action<ButtonSwitchController, Vector3> OnButtonSwitchOn;
    public event Action<ButtonSwitchController, Vector3> OnButtonSwitchOff;

    public override void Awake()
    {
        base.Awake();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isOn)
        SetOnAction();
    }

    public override void OnCollisionExit2D(Collision2D collision)
    {
    }

    public override void OnCollisionStay2D(Collision2D collision)
    {
    }

    public override void SetOffAction()
    {
        isOn = false;
        ChangeAnimationState(BUTTON_UP);

        if (OnButtonSwitchOff != null)
            OnButtonSwitchOff(this, transform.position);
    }

    public override void SetOnAction()
    {
        isOn = true;
        ChangeAnimationState(BUTTON_DOWN);
        OnPrimaryAction.Invoke();

        if (OnButtonSwitchOn != null)
            OnButtonSwitchOn(this, transform.position);
    }

    public override void ChangeAnimationState(string newState)
    {
        base.ChangeAnimationState(newState);
    }

    public override bool GetIsOn()
    {
        return base.GetIsOn();
    }
}
