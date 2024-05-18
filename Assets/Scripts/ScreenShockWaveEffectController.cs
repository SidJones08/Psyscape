using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShockWaveEffectController : MonoBehaviour
{
    [SerializeField] private bool isRunning;
    [SerializeField] private float duration = 1;
    [SerializeField] private float elapsed = 0;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnEnable()
    {
        StartRoutine();
    }

    public void StartRoutine()
    {
        if (!isRunning)
            StartCoroutine("ShockWaveRoutine");
    }

    IEnumerator ShockWaveRoutine()
    {
        isRunning = true;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            spriteRenderer.material.SetFloat("_Progression", elapsed / duration);
            yield return null;
        }

        elapsed = 0;
        isRunning = false;

        gameObject.SetActive(false);
    }

    public void OnDisable()
    {
        StopAllCoroutines();
        elapsed = 0;
        isRunning = false;
    }
}
