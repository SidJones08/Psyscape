using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorHazardController : CharacterAnimatorController
{
    const string CHARACTER_IDLE = "Animation_Character_Hazard_Idle";
    const string CHARACTER_RUN = "Animation_Character_Hazard_Running";
    const string CHARACTER_AIR = "Animation_Character_Hazard_Air";
    const string CHARACTER_CLIMBING = "Animation_Character_Hazard_Climbing";
    const string CHARACTER_STUNNED_AIR = "Animation_Character_Hazard_Dead_Air_Hands";
    const string CHARACTER_STUNNED_GROUND = "Animation_Character_Hazard_Stunned_Ground";
    const string CHARACTER_INFLUENCED_AIR = "Animation_Character_Hazard_Influenced_Air_Hands";
    const string CHARACTER_INFLUENCED_TARGETREACHED = "";

    public override void Awake()
    {
        base.Awake();
    }

    public override void ChangeAnimationState(string newState)
    {
        base.ChangeAnimationState(newState);
    }

    private void Update()
    {
        if (!movementController.GetIsKnocked())
        {
            if (!movementController.GetIsInfluenced())
            {
                if (movementController.GetIsClimbing())
                {
                    ChangeAnimationState(CHARACTER_CLIMBING);
                }
                else
                {
                    if (!movementController.GetHasReachedTarget())
                    {
                        if (movementController.GetOnGround())
                        {
                            if (movementController.GetDesiredVelocity() != Vector2.zero)
                            {
                                ChangeAnimationState(CHARACTER_RUN);
                            }
                            else
                            {
                                ChangeAnimationState(CHARACTER_IDLE);
                            }
                        }
                        else
                        {
                            ChangeAnimationState(CHARACTER_AIR);
                        }
                    }
                    else
                    {
                        if (!movementController.GetIsPinned())
                        {
                            ChangeAnimationState(CHARACTER_IDLE);
                        }
                    }
                }
            }
            else
            {
                ChangeAnimationState(CHARACTER_INFLUENCED_AIR);
            }
        }
        else
        {
            if (movementController.GetOnGround())
            {
                ChangeAnimationState(CHARACTER_STUNNED_GROUND);
            }
            else
            {
                ChangeAnimationState(CHARACTER_STUNNED_AIR);
            }
        }
    }
}
