using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VentEntranceController : MonoBehaviour
{
    [SerializeField] private Transform ventCoverTransform;
    [SerializeField] private Vector2 ventCoverStartingPosition;

    public event Action<VentEntranceController, Vector3> OnVentEntranceCollision;

    private void Start()
    {
        ventCoverStartingPosition = ventCoverTransform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if (collision.contacts.Length > 0)
        {
            if (OnVentEntranceCollision != null)
                OnVentEntranceCollision(this, transform.position);
        }
        */
    }
}
