using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonPressureController : ButtonController
{
    private List<Collider2D> collider2Ds = new List<Collider2D>();
    
    [SerializeField] private float releaseDelay = 0.1f;
    [SerializeField] private float releaseElapsed;
    [SerializeField] private bool isRunning;
    
    protected const string BUTTON_UP_PRESSURE = "Animation_Button_Pressure_Up_Idle";
    protected const string BUTTON_DOWN_PRESSURE = "Animation_Button_Pressure_Down_Idle";

    public event Action<ButtonPressureController, Vector3> OnButtonPressureSwitchOn;
    public event Action<ButtonPressureController, Vector3> OnButtonPressureSwitchOff;

    public override void Awake()
    {
        base.Awake();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        CancelButtonRelease();
    }

    public override void OnCollisionExit2D(Collision2D collision)
    {
        if (collider2Ds.Contains(collision.collider))
            collider2Ds.Remove(collision.collider);

        if (collider2Ds.Count > 0)
        {
        }
        else
        {
            if (!isRunning)
                StartCoroutine("ButtonReleaseRoutine");
        }
    }

    public override void OnCollisionStay2D(Collision2D collision)
    {
        CancelButtonRelease();

        if (!collider2Ds.Contains(collision.collider))
            collider2Ds.Add(collision.collider);

        SetOnAction();
    }

    IEnumerator ButtonReleaseRoutine()
    {
        isRunning = true;

        while(releaseElapsed < releaseDelay)
        {
            releaseElapsed += Time.deltaTime;
            yield return null;
        }

        SetOffAction();
        isRunning = false;
        releaseElapsed = 0;
    }

    private void CancelButtonRelease()
    {
        StopAllCoroutines();
        isRunning = false;
        releaseElapsed = 0;
    }

    public override void SetOffAction()
    {
        isOn = false;
        ChangeAnimationState(BUTTON_UP_PRESSURE);
        OnSecondaryAction.Invoke();

        if (OnButtonPressureSwitchOff != null)
            OnButtonPressureSwitchOff(this, transform.position);
    }

    public override void SetOnAction()
    {
        if (!isOn)
        {
            if (OnButtonPressureSwitchOn != null)
                OnButtonPressureSwitchOn(this, transform.position);
        }

        isOn = true;
        ChangeAnimationState(BUTTON_DOWN_PRESSURE);
        OnPrimaryAction.Invoke();
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
