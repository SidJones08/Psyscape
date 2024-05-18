using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VentController : MonoBehaviour
{
    [SerializeField] private Sprite spriteInterior;
    [SerializeField] private Sprite spriteExterior;

    [SerializeField] private bool isOccupied;

    public event Action OnVentEnter;
    public event Action OnVentExit;

    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            isOccupied = true;
            if (OnVentEnter != null)
                OnVentEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            isOccupied = false;
            if (OnVentExit != null)
                OnVentExit();
        }
    }

    public bool GetIsOccupied()
    {
        return isOccupied;
    }

    public void VentEnterUpdateVisual()
    {
        spriteRenderer.sprite = spriteInterior;
    }

    public void VentExitUpdateVisual()
    {
        spriteRenderer.sprite = spriteExterior;
    }

}
