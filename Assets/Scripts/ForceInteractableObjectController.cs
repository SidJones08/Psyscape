using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ForceInteractableObjectController : MonoBehaviour
{
    [SerializeField] private ParticleSystemEffectController psPinned;
    [SerializeField] private ParticleSystemEffectController psUnpinned;

    [SerializeField] private Material materialDefault;
    [SerializeField] private Material materialOutline;
    [SerializeField] private Material materialPinned;

    [SerializeField] private bool notPhysicsObjectOnEnable;

    [SerializeField] private bool solidOnPin;

    [SerializeField] private float magnitudeForCollisionSound = 5f; 

    protected bool isInfluenceImmunity;
    protected private float influenceImmunityDuration = 1f;
    protected private float influenceImmunityElapsed = 0f;

    protected private bool isInfluenced;
    protected private bool isPinned;
    protected private bool immunePinned;

    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigidbody2D;

    public event Action OnInfluenceChange;
    public event Action OnPinChange;

    public event Action<ForceInteractableObjectController, Vector3> OnCollisionSound;

    public virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void OnEnable()
    {
        if(notPhysicsObjectOnEnable)
        gameObject.layer = 3;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (OnCollisionSound != null)
        {
            OnCollisionSound(this, transform.position);
        }
    }

    public virtual void UpdateInfluenceState()
    {
        if (!immunePinned)
        {
            if (isPinned)
            {
                spriteRenderer.material = materialPinned;
            }
            else
            {
                if (isInfluenced)
                    spriteRenderer.material = materialOutline;
                else
                    spriteRenderer.material = materialDefault;
            }
        }
        else
        {
            if (isInfluenced)
                spriteRenderer.material = materialOutline;
            else
                spriteRenderer.material = materialDefault;
        }
    }

    public virtual void StartInfluenceImmunityRoutine()
    {
        if (!isInfluenceImmunity)
            StartCoroutine("InfluenceImmunityRoutine");
    }

    IEnumerator InfluenceImmunityRoutine()
    {
        isInfluenceImmunity = true;

        while(influenceImmunityElapsed < influenceImmunityDuration)
        {
            influenceImmunityElapsed += Time.deltaTime;
            yield return null;
        }

        influenceImmunityElapsed = 0;
        isInfluenceImmunity = false;
    }

    public virtual void SetIsInfluenced(bool value)
    {
        isInfluenced = value;

        if (OnInfluenceChange != null)
            OnInfluenceChange();
    }

    public virtual void SetIsPinned(bool value)
    {
        isPinned = value;

        if(isPinned)
            SpawnPinnedEffect();
        else
            SpawnUnpinnedEffect();

        if (OnPinChange != null)
            OnPinChange();
    }

    public virtual Rigidbody2D GetRigidbody2D()
    {
        return rigidbody2D;
    }

    public virtual bool GetIsInfluenced()
    {
        return isInfluenced;
    }

    public virtual bool GetIsPinned()
    {
        return isPinned;
    }

    public virtual bool GetIsInfluenceImmunity()
    {
        return isInfluenceImmunity;
    }

    public virtual void ForceEffectAction(Vector2 dir)
    {
        if (dir != Vector2.zero)
        {

        }
        else
        {
        }
    }

    private void SpawnPinnedEffect()
    {
        ParticleSystemEffectController particleSystemPinned = ObjectPool.instance.GetObjectFromPool(psPinned.name).GetComponent<ParticleSystemEffectController>();
        particleSystemPinned.transform.position = transform.position;
        particleSystemPinned.gameObject.SetActive(true);
    }

    private void SpawnUnpinnedEffect()
    {
        ParticleSystemEffectController particleSystemPinned = ObjectPool.instance.GetObjectFromPool(psUnpinned.name).GetComponent<ParticleSystemEffectController>();
        particleSystemPinned.transform.position = transform.position;
        particleSystemPinned.gameObject.SetActive(true);
    }

    public virtual void OnPinEnter()
    {
        SetIsPinned(true);
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;

        if (solidOnPin)
            gameObject.layer = 9;
    }

    public virtual void OnPinExit()
    {
        SetIsPinned(false);
        rigidbody2D.constraints = RigidbodyConstraints2D.None;
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        if(solidOnPin)
                gameObject.layer = 3;
    }
}
