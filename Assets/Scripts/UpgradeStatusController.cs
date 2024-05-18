using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UpgradeStatusController : MonoBehaviour
{
    [SerializeField] private int currentUpgradeIndex;
    [SerializeField] private TutorialMananger tutorialMananger;
    [SerializeField] private bool canUpgrade = true;
    [SerializeField] private float canUpgradeDelay = 0.5f;

    public event Action<UpgradeStatusController, Vector3> UpgradeEnter;
    public event Action UpgradeExit;

    private VisualUpgradeController visualUpgradeController;
    private PlayerController playerController;
    private ExpansiveForce expansiveForce;
    private BurstForceController burstForceController;
    private PinForceController pinForceController;
    private ForceProjectCharacterController forceProjectCharacter;

    public static UpgradeStatusController instance;

    private void Awake()
    {
        instance = this;

        visualUpgradeController = GetComponent<VisualUpgradeController>();
        playerController = GetComponent<PlayerController>();
        expansiveForce = GetComponent<ExpansiveForce>();
        burstForceController = GetComponent<BurstForceController>();
        pinForceController = GetComponent<PinForceController>();
        forceProjectCharacter = GetComponent<ForceProjectCharacterController>();

        tutorialMananger = FindObjectOfType<TutorialMananger>();
    }

    private void Start()
    {
        tutorialMananger.OnTutorialComplete += UpgradePlayerComplete;
    }

    public void UpgradePlayer()
    {
        if (canUpgrade)
        {
            StartCoroutine("UpgradeDelayRoutine");
            UpgradePlayerStart();
            currentUpgradeIndex++;
            DisablePlayer();
        }
    }

    //Quick Temporary Fix (Upgrade Player being called more than once when eating pill)
    IEnumerator UpgradeDelayRoutine()
    {
        canUpgrade = false;

        while (!canUpgrade)
        {
            yield return new WaitForSeconds(canUpgradeDelay);
            canUpgrade = true;
        }
    }

    public void UpgradePlayerStart()
    {
        if (UpgradeEnter != null)
            UpgradeEnter(this, transform.position);
    }

    public void UpgradePlayerComplete(TutorialMananger tutorialMananger, Vector3 pos)
    {
        UpdatePlayerAbilities();
        EnablePlayer();
    }

    public void DisablePlayer()
    {
        playerController.SetCanAct(false);
        playerController.SetCanMove(false);
        playerController.SetDesiredVelocity(Vector2.zero);
    }

    public void EnablePlayer()
    {
        playerController.SetCanAct(true);
        playerController.SetCanMove(true);
    }

    public void UpdatePlayerAbilities()
    {
        switch (currentUpgradeIndex)
        {
            case 0:
                //Starting Level Ignore
                break;
            case 1:
                expansiveForce.enabled = true;
                break;
            case 2:
                burstForceController.enabled = true;
                break;
            case 3:
                pinForceController.enabled = true;
                break;
            case 4:
                forceProjectCharacter.enabled = true;
                break;
            default:
                break;
        }
    }

    public int GetUpgradeIndex()
    {
        return currentUpgradeIndex;
    }
}
