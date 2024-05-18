using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentDoorController : WiggleController
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void ForceEffectAction(Vector2 dir)
    {
        base.ForceEffectAction(dir);
    }

    public override bool GetIsInfluenced()
    {
        return base.GetIsInfluenced();
    }

    public override bool GetIsInfluenceImmunity()
    {
        return base.GetIsInfluenceImmunity();
    }

    public override bool GetIsPinned()
    {
        return base.GetIsPinned();
    }

    public override Rigidbody2D GetRigidbody2D()
    {
        return base.GetRigidbody2D();
    }

    public override void OnPinEnter()
    {
        base.OnPinEnter();
    }

    public override void OnPinExit()
    {
        base.OnPinExit();
    }

    public override void Release()
    {
        base.Release();
    }

    public override void SetIsInfluenced(bool value)
    {
        base.SetIsInfluenced(value);
    }

    public override void SetIsPinned(bool value)
    {
        base.SetIsPinned(value);
    }

    public override void Start()
    {
        base.Start();
    }

    public override void StartInfluenceImmunityRoutine()
    {
        base.StartInfluenceImmunityRoutine();
    }

    public override void UpdateInfluenceState()
    {
        base.UpdateInfluenceState();
    }

    public override void WiggleAction()
    {
        base.WiggleAction();
    }
}
