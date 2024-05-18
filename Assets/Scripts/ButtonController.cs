using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour
{
    public UnityEvent OnPrimaryAction;
    public UnityEvent OnSecondaryAction;

    private PolygonCollider2D polygonCollider2D;
    private Animator animator;
    private string currentState;

    protected Collider2D collider2D;
    protected bool isOn;

    [SerializeField] private Color colourLightActive;
    [SerializeField] private Color colourLightInActive;

    protected const string BUTTON_UP = "Animation_Button_Up_Idle";
    protected const string BUTTON_DOWN = "Animation_Button_Down_Idle";

    public virtual void Awake()
    {
        animator = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
    }

    public virtual void OnCollisionStay2D(Collision2D collision)
    {
    }

    public virtual void OnCollisionExit2D(Collision2D collision)
    {
    }

    public virtual void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
            return;

        animator.Play(newState);
        currentState = newState;
    }

    public virtual void SetOnAction() 
    {

    }

    public virtual void SetOffAction()
    {

    }

    public virtual bool GetIsOn()
    {
        return isOn;
    }
}
