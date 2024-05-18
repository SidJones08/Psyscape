using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [SerializeField] private bool isTracking;
    [SerializeField] private bool isTransitioning;
    [SerializeField] private bool isDirectionalShake;
 
    [SerializeField] private bool isUpgrading;
    [SerializeField] private float zoomMax;
    [SerializeField] private float zoomDefault;
    [SerializeField] private float upgradeDuration = 3;
    [SerializeField] private float upGradeElapsed;
    [SerializeField] private float upgradeZoomRate = 1;
    [SerializeField] private float upgradeZoomRemainDuration = 1;
    [SerializeField] private float upgradeZoomResumeRate = 1;
    [SerializeField] private Vector3 lastScreenPos;

    [SerializeField] private float isTrackingRefresh = 0.1f;

    [SerializeField] private float cameraMoveSpeed = 1;
    [SerializeField] private LevelContainer levelContainerCurrent;

    [SerializeField] private Camera camera;
    [SerializeField] private Camera backgroundCamera;

    [SerializeField] private TutorialMananger tutorialMananger;

    private UpgradeStatusController upgradeStatusController;
    private TileMapManager tileMapManager;

    public static CameraController instance;

    public event Action OnEnterLevel;
    public event Action OnExitPreviousLevel;
    public event Action<CameraController, Vector3> OnPopUpOpen;

    private void Awake()
    {
        instance = this;
        upgradeStatusController = FindObjectOfType<UpgradeStatusController>();
        playerController = FindObjectOfType<PlayerController>();
        tutorialMananger = FindObjectOfType<TutorialMananger>();
    }

    private void Start()
    {
        tileMapManager = TileMapManager.instance;
        upgradeStatusController.UpgradeEnter += StartUpgradeRoutine;

        if (!isTracking)
            StartCoroutine(PlayerTrackingRoutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(levelContainerCurrent.SceneName);

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

    }

    private IEnumerator PlayerTrackingRoutine()
    {
        isTracking = true;

        while (isTracking)
        {
            lastScreenPos = transform.position;

            if (levelContainerCurrent != tileMapManager.GetClosestLevelContainer(playerController.transform.position))
            { 
                if (!isTransitioning)
                    StartCoroutine(TransitionRoutine(new Vector3(tileMapManager.GetClosestLevelContainer(playerController.transform.position).LevelPosition.x, tileMapManager.GetClosestLevelContainer(playerController.transform.position).LevelPosition.y, -10)));
            }

            yield return new WaitForSeconds(isTrackingRefresh);
        }
    }

    private IEnumerator TransitionRoutine(Vector3 endPos)
    {
        isTransitioning = true;

        levelContainerCurrent = tileMapManager.GetClosestLevelContainer(playerController.transform.position);

        if (OnEnterLevel != null)
            OnEnterLevel();

        while (transform.position != endPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, Time.deltaTime * cameraMoveSpeed);
            yield return null;
        }

        transform.position = endPos;
        isTransitioning = false;

        if (OnExitPreviousLevel != null)
            OnExitPreviousLevel();
    }

    public void StartUpgradeRoutine(UpgradeStatusController upgradeStatusController, Vector3 pos)
    {
        if (!isUpgrading)
            StartCoroutine("UpgradeRoutine");
    }

    IEnumerator UpgradeRoutine()
    {
        isUpgrading = true;

        StopTransitionRoutine();
        StopPlayerTrackingRoutine();

        Vector3 targetPos = new Vector3(playerController.transform.position.x, playerController.transform.position.y, -10);

        while (upGradeElapsed < upgradeDuration)
        {
            upGradeElapsed += Time.deltaTime;

            camera.orthographicSize = Mathf.Lerp(zoomDefault, zoomMax, (upGradeElapsed / upgradeDuration));
            backgroundCamera.orthographicSize = Mathf.Lerp(zoomDefault, zoomMax, (upGradeElapsed / upgradeDuration));          
            transform.position = Vector3.MoveTowards(transform.position, targetPos, (upGradeElapsed / upgradeDuration));
            yield return null;
        }

        camera.orthographicSize = zoomMax;
        backgroundCamera.orthographicSize = zoomMax;

        upGradeElapsed = 0;

        yield return new WaitForSeconds(upgradeZoomRemainDuration);

        while (upGradeElapsed < upgradeZoomResumeRate)
        {
            upGradeElapsed += Time.deltaTime;
            
            camera.orthographicSize = Mathf.Lerp(zoomMax, zoomDefault, (upGradeElapsed / upgradeZoomResumeRate));
            backgroundCamera.orthographicSize = Mathf.Lerp(zoomMax, zoomDefault, (upGradeElapsed / upgradeZoomResumeRate));

            transform.position = Vector3.MoveTowards(transform.position, lastScreenPos, (upGradeElapsed / upgradeZoomResumeRate));
            yield return null;
        }

        camera.orthographicSize = zoomDefault;
        backgroundCamera.orthographicSize = zoomDefault;

        upGradeElapsed = 0;
        isUpgrading = false;

        //Move These to TutorialManager

        tutorialMananger.EnableTutorial(upgradeStatusController.GetUpgradeIndex() - 1);
        
        if (OnPopUpOpen != null)
            OnPopUpOpen(this, transform.position);

        if (!isTracking)
            StartCoroutine(PlayerTrackingRoutine());
    }

    public void StopUpgradeRoutine()
    {
        StopCoroutine("UpgradeRoutine");
        isUpgrading = false;
        upGradeElapsed = 0;
    }

    public void StopTransitionRoutine()
    {
        StopCoroutine("TransitionRoutine");
        isTransitioning = false;
    }

    public void StopPlayerTrackingRoutine()
    {
        StopCoroutine("PlayerTrackingRoutine");
        isTracking = false;
    }

    public LevelContainer GetLevelContainerCurrent()
    {
        return levelContainerCurrent;
    }

}
