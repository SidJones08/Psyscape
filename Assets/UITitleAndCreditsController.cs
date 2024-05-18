using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITitleAndCreditsController : MonoBehaviour
{
    [SerializeField] private Gradient fadeGradient;
    [SerializeField] private float fadeOutTime = 3;
    [SerializeField] private float fadeOutTimeElapsed;

    [SerializeField] private bool isRunning;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text titleCreatedBy;
    [SerializeField] private TMP_Text titleCredits;

    private void Start()
    {
        titleText.overrideColorTags = true;
        titleCreatedBy.overrideColorTags = true;
        titleCredits.overrideColorTags = true;

        Invoke("StartFadeTitleAndCreditsDelayRoutine", 3);
    }

    private void StartFadeTitleAndCreditsDelayRoutine()
    {
        if (!isRunning)
            StartCoroutine("FadeTitleAndCreditsDelayRoutine");
    }

    IEnumerator FadeTitleAndCreditsDelayRoutine()
    {
        isRunning = true;

        while (fadeOutTimeElapsed < fadeOutTime)
        {
            UpdateTitleAndCredits();
            fadeOutTimeElapsed += Time.deltaTime;
            yield return null;
        }

        isRunning = false;

        EndTitleAndCredits();
    }

    public void UpdateTitleAndCredits()
    {
        float progress = fadeOutTimeElapsed / fadeOutTime;

        titleText.color = fadeGradient.Evaluate(progress);
        titleCreatedBy.color = fadeGradient.Evaluate(progress);
        titleCredits.color = fadeGradient.Evaluate(progress);
    }

    private void EndTitleAndCredits()
    {
        gameObject.SetActive(false);
    }
}
