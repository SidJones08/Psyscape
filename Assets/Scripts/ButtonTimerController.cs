using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonTimerController : ButtonController, LoopingSound
{
    [SerializeField] private float releaseDelay = 3f;
    [SerializeField] private float releaseElapsed;
    [SerializeField] private bool isRunning;

    public event Action<LoopingSound, ButtonTimerController, Vector3> OnTimerStart;
    public event Action<ButtonTimerController, Vector3> OnTimerUpdate;
    public event Action<LoopingSound, ButtonTimerController, Vector3> OnTimerEnd;

    public event Action<ButtonTimerController, Vector3> OnButtonTimerOn;
    public event Action<ButtonTimerController, Vector3> OnButtonTimerOff;

    public override void Awake()
    {
        base.Awake();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isRunning)
            StartCoroutine("ButtonTimedReleaseRoutine");
    }

    public override void OnCollisionExit2D(Collision2D collision)
    {
    }

    public override void OnCollisionStay2D(Collision2D collision)
    {
    }

    IEnumerator ButtonTimedReleaseRoutine()
    {
        isRunning = true;

        SetOnAction();

        if (OnTimerStart != null)
            OnTimerStart(this, this, transform.position);

        while (releaseElapsed < releaseDelay)
        {
            releaseElapsed += Time.deltaTime;

            if (OnTimerUpdate != null)
                OnTimerUpdate(this, transform.position);

            yield return null;
        }

        if (OnTimerEnd != null)
            OnTimerEnd(this, this, transform.position);

        SetOffAction();
        isRunning = false;
        releaseElapsed = 0;
    }

    public override void SetOffAction()
    {
        isOn = false;
        ChangeAnimationState(BUTTON_UP);
        OnSecondaryAction.Invoke();

        if (OnButtonTimerOff != null)
            OnButtonTimerOff(this, transform.position);
    }

    public override void SetOnAction()
    {
        isOn = true;
        ChangeAnimationState(BUTTON_DOWN);
        OnPrimaryAction.Invoke();

        if (OnButtonTimerOn != null)
            OnButtonTimerOn(this, transform.position);
    }

    public override void ChangeAnimationState(string newState)
    {
        base.ChangeAnimationState(newState);
    }

    public override bool GetIsOn()
    {
        return base.GetIsOn();
    }

    public float GetReleaseDelay()
    {
        return releaseDelay;
    }

    public float GetReleaseElapsed()
    {
        return releaseElapsed;
    }
}
