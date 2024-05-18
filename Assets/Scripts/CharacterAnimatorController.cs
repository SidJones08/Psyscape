using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorController : MonoBehaviour
{
    protected Animator animator;
    protected string currentState;

    const string CHARACTER_IDLE = "Character_Idle";
    const string CHARACTER_RUN = "Character_Run";
    const string CHARACTER_AIR = "Character_Air";
    const string CHARACTER_GETUP = "Character_GetUp";
    const string CHARACTER_STUNNED = "Character_Stunned_Ground";
    const string CHARACTER_CLIMBING = "Character_Climbing";
    const string CHARACTER_FORCEHORIZONTAL = "Character_ForceHorizontal";
    const string CHARACTER_FORCEVERTICALUP = "Character_ForceVerticalUp";
    const string CHARACTER_FORCEVERTICALDOWN = "Character_ForceVerticalDown";
    const string CHARACTER_FORCEDIAGONAL = "Character_ForceDiagonal";
    const string CHARACTER_INFLUENCEDAIR = "Character_InfluencedAir_Hands";
    const string CHARACTER_DEADAIR = "Character_Dead_Air_Hands";

    protected MovementController movementController;

    public virtual void Awake()
    {
        movementController = GetComponent<MovementController>();
        animator = GetComponent<Animator>();
    }

    public virtual void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
            return;

        animator.Play(newState);
        currentState = newState;
    }

    /*
    private void Update()
    {
        if (!movementController.GetIsKnocked())
        {
            if (movementController.GetIsForceExpanding())
            {
                if (movementController.GetForceDirection() != Vector2.zero)
                {
                    if (movementController.GetForceDirection().x != 0 && movementController.GetForceDirection().y != 0)
                    {
                        ChangeAnimationState(CHARACTER_FORCEDIAGONAL);
                    }
                    else if (movementController.GetForceDirection().x == 0 && movementController.GetForceDirection().y > 0)
                    {
                        ChangeAnimationState(CHARACTER_FORCEVERTICALUP);
                    }
                    else if (movementController.GetForceDirection().x == 0 && movementController.GetForceDirection().y < 0)
                    {
                        ChangeAnimationState(CHARACTER_FORCEVERTICALDOWN);
                    }
                    else if (movementController.GetForceDirection().x != 0 && movementController.GetForceDirection().y == 0)
                    {
                        ChangeAnimationState(CHARACTER_FORCEHORIZONTAL);
                    }
                }
                else
                {
                    ChangeAnimationState(CHARACTER_FORCEHORIZONTAL);
                }
            }
            else
            {
                if (!movementController.GetIsInfluenced())
                {
                    if (movementController.GetIsClimbing())
                    {
                        ChangeAnimationState(CHARACTER_CLIMBING);
                    }
                    else
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
                }
                else
                {
                    ChangeAnimationState(CHARACTER_INFLUENCEDAIR);
                }
            }
        }
        else
        {
            if (movementController.GetOnGround())
            {
                ChangeAnimationState(CHARACTER_STUNNED);
            }
            else
            {
                ChangeAnimationState(CHARACTER_DEADAIR);
            }
        }
    }

    const string CHARACTER_IDLE = "Character_Idle";
    const string CHARACTER_RUN = "Character_Run";
    const string CHARACTER_AIR = "Character_Air";
    const string CHARACTER_GETUP = "Character_GetUp";
    const string CHARACTER_STUNNED = "Character_Stunned_Ground";
    const string CHARACTER_CLIMBING = "Character_Climbing";
    const string CHARACTER_FORCEHORIZONTAL = "Character_ForceHorizontal";
    const string CHARACTER_FORCEVERTICALUP = "Character_ForceVerticalUp";
    const string CHARACTER_FORCEVERTICALDOWN = "Character_ForceVerticalDown";
    const string CHARACTER_FORCEDIAGONAL = "Character_ForceDiagonal";
    const string CHARACTER_INFLUENCEDAIR = "Character_InfluencedAir_Hands";
    const string CHARACTER_DEADAIR = "Character_Dead_Air_Hands";


    */


}
