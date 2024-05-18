using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTitleManager : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private bool isRunning;
    [SerializeField] private float titleRemainDuration = 2.5f;
    [SerializeField] private float titleFadeInTime = 0.5f;
    [SerializeField] private float titleFadeInOut = 0.5f;
    [SerializeField] private float titleElapsed;

    CameraController cameraController;

    private void Start()
    {
        cameraController = CameraController.instance;
        
        cameraController.OnEnterLevel += EnterLevel;
        cameraController.OnExitPreviousLevel += ExitPreviousLevel;
    }

    public void StartLevelTitle()
    {
        if (!isRunning)
            StartCoroutine("TitleRoutine");
    }

    public void CancelTitleRoutine()
    {
        StopCoroutine("TitleRoutine");
        title.color = new Color(title.color.r, title.color.g, title.color.b, 0);
        isRunning = false;
        title.gameObject.SetActive(false);
        titleElapsed = 0;
    }

    public void EnterLevel()
    {
        //Debug.Log("Enter Level");
        CancelInvoke();
    }

    public void ExitPreviousLevel()
    {
        StartLevelTitle();
        //Debug.Log("Exit Level");
    }

    private IEnumerator TitleRoutine()
    {
        isRunning = true;

        title.gameObject.SetActive(true);
        title.text = cameraController.GetLevelContainerCurrent().LevelName;

        while (titleElapsed < titleFadeInTime)
        {
            titleElapsed += Time.deltaTime;
            title.color = new Color(title.color.r, title.color.g, title.color.b, titleElapsed / titleFadeInTime);
            yield return null;
        }

        titleElapsed = 0;

        yield return new WaitForSeconds(titleRemainDuration);

        while (titleElapsed < titleFadeInOut)
        {
            titleElapsed += Time.deltaTime;
            title.color = new Color(title.color.r, title.color.g, title.color.b, 1 - (titleElapsed / titleFadeInOut));
            yield return null;
        }

        titleElapsed = 0;

        title.gameObject.SetActive(false);
        isRunning = false;
    }
}
