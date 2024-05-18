using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScientistAnimatorController : CharacterAnimatorController
{
    //Does not take into account "Pinned"
    const string CHARACTER_IDLE = "Animation_Character_Scientist_Idle";
    const string CHARACTER_RUN = "Animation_Character_Scientist_Running";
    const string CHARACTER_AIR = "Animation_Character_Scientist_Air";
    const string CHARACTER_CLIMBING = "Animation_Character_Scientist_Climbing";
    const string CHARACTER_STUNNED_AIR = "Animation_Character_Scientist_Dead_Air_Hands";
    const string CHARACTER_STUNNED_GROUND = "Animation_Character_Scientist_Stunned";
    const string CHARACTER_INFLUENCED_AIR = "Animation_Character_Scientist_InfluencedAir_Hands";
    const string CHARACTER_INFLUENCED_TARGETREACHED = "Animation_Character_Scientist_TargetReached";

    //const string CHARACTER_GETUP = "Animation_Character_Player_Climbing";

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
                            ChangeAnimationState(CHARACTER_INFLUENCED_TARGETREACHED);
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
