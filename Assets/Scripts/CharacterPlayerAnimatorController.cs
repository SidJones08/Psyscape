using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterPlayerAnimatorController : CharacterAnimatorController
{
    const string CHARACTER_IDLE = "Animation_Character_Player_Idle";
    const string CHARACTER_RUN = "Animation_Character_Player_Running";
    const string CHARACTER_AIR = "Animation_Character_Player_Air";
    const string CHARACTER_CLIMBING = "Animation_Character_Player_Climbing";
    const string CHARACTER_CLIMBING_IDLE = "Animation_Character_Player_Climbing_Idle";
    const string CHARACTER_FORCEHORIZONTAL = "Animation_Character_Player_ForceHorizontal";
    const string CHARACTER_FORCEVERTICALUP = "Animation_Character_Player_ForceVerticalUp";
    const string CHARACTER_FORCEVERTICALDOWN = "Animation_Character_Player_ForceVerticalDown";
    const string CHARACTER_FORCEDIAGONAL = "Animation_Character_Player_ForceDiagonal";
    const string CHARACTER_UPGRADING = "Animation_Character_Player_Upgrading";

    //Animation_Character_Player_Climbing_Idle

    //const string CHARACTER_FORCEAIR = "Animation_Character_Player_Force_InAir";
    //const string CHARACTER_GETUP = "Character_GetUp";
    //const string CHARACTER_STUNNED = "Character_Stunned_Ground";
    //const string CHARACTER_AIR = "Character_Stunned_Ground";

    private PlayerController playerController;

    public event Action<CharacterPlayerAnimatorController, Vector3> OnFootPlacmentRight;
    public event Action<CharacterPlayerAnimatorController, Vector3> OnFootPlacmentLeft;

    public event Action<CharacterPlayerAnimatorController, Vector3> OnLeftFootLadder;
    public event Action<CharacterPlayerAnimatorController, Vector3> OnRightFootLadder;

    public override void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    public override void ChangeAnimationState(string newState)
    {
        base.ChangeAnimationState(newState);
    }

    private void Update()
    {
        if (!playerController.GetIsUpgrading())
        {
            if (playerController.GetIsForceExpanding())
            {
                if (playerController.GetForceDirection() != Vector2.zero)
                {
                    if (playerController.GetForceDirection().x != 0 && playerController.GetForceDirection().y != 0)
                    {
                        ChangeAnimationState(CHARACTER_FORCEDIAGONAL);
                    }
                    else if (playerController.GetForceDirection().x == 0 && playerController.GetForceDirection().y > 0)
                    {
                        ChangeAnimationState(CHARACTER_FORCEVERTICALUP);
                    }
                    else if (playerController.GetForceDirection().x == 0 && playerController.GetForceDirection().y < 0)
                    {
                        ChangeAnimationState(CHARACTER_FORCEVERTICALDOWN);
                    }
                    else if (playerController.GetForceDirection().x != 0 && playerController.GetForceDirection().y == 0)
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
                if (playerController.GetIsClimbing())
                {
                    if (playerController.GetDirection() != Vector2.zero)
                    {
                        ChangeAnimationState(CHARACTER_CLIMBING);
                    }
                    else
                    {
                        ChangeAnimationState(CHARACTER_CLIMBING_IDLE);
                    }
                }
                else
                {
                    if (playerController.GetOnGround())
                    {
                        if (playerController.GetDesiredVelocity() != Vector2.zero)
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
        }
        else
        {
            ChangeAnimationState(CHARACTER_UPGRADING);
        }
    }

    public void FootPlacementAnimationEventLeft()
    {
        if (OnFootPlacmentLeft != null)
            OnFootPlacmentLeft(this, transform.position);
    }

    public void FootPlacementAnimationEventRight()
    {
        if (OnFootPlacmentRight != null)
            OnFootPlacmentRight(this, transform.position);
    }

    public void CharacterLadderLeftFoot()
    {
        if (OnLeftFootLadder != null)
            OnLeftFootLadder(this, transform.position);
    }

    public void CharacterLadderRightFoot()
    {
        if (OnRightFootLadder != null)
            OnRightFootLadder(this, transform.position);
    }
}
