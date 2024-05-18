using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PillAcquirePowerController : MonoBehaviour
{
    [SerializeField] private bool isRunning;
    [SerializeField] private Vector2 startSize;
    [SerializeField] private Vector2 minSize;
    [SerializeField] private Vector2 maxSize;
    [SerializeField] private float updateRate = 0.1f;
    [SerializeField] private float sizeChangeRate = 1;

    [SerializeField] private float rotationSpeed = 1;

    public event Action<PillAcquirePowerController, Vector3> OnPillAquired;

    private void Start()
    {
        startSize = transform.localScale;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<UpgradeStatusController>())
        {
            if (OnPillAquired != null)
                OnPillAquired(this, transform.position);

            collision.GetComponent<UpgradeStatusController>().UpgradePlayer();
            gameObject.SetActive(false);
        }
    }
}
