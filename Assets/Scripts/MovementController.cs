using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public virtual void OnClimbEnter()
    {
    }

    public virtual void OnClimbExit()
    {
    }

    public virtual void SetOnGround(bool value)
    {
    }

    public virtual void SetCharacterMaterial(Material material)
    {
    }

    public virtual void SetCanMove(bool value)
    {
    }

    public virtual void SetCanAct(bool value)
    {
    }

    public virtual bool GetCanMove()
    {
        return true;
    }

    public virtual bool GetCanAct()
    {
        return true;
    }

    public virtual bool GetIsPinned()
    {
        return false;
    }

    public virtual bool GetHasReachedTarget()
    {
        return false;
    }

    public virtual bool GetOnGround()
    {
        return false;
    }

    public virtual bool GetIsClimbing()
    {
        return false;
    }

    public virtual bool GetIsKnocked()
    {
        return false;
    }

    public virtual bool GetIsInfluenced()
    {
        return false;
    }

    public virtual bool GetIsForceExpanding()
    {
        return false;
    }

    public virtual Vector2 GetDesiredVelocity()
    {
        return Vector2.zero;
    }

    public virtual Vector2 GetVelocity()
    {
        return Vector2.zero;
    }

    public virtual Vector2 GetForceDirection() 
    {
        return Vector2.zero;
    }

    public virtual Material GetDefaultMaterial()
    {
        return null;
    }
}
