using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private SoundClipController soundClipControllerPrefab;
    [SerializeField] private LoopingAudioController loopingAudioControllerPrefab;

    public static SoundManager instance;

    [Header("Audio Character")]
    [SerializeField] private AudioClip audioClipActionCharacterJump; 
    [SerializeField] private AudioClip audioClipActionCharacterLanded; //Done
    [SerializeField] private AudioClip audioClipActionCharacterLeftFoot; //Done
    [SerializeField] private AudioClip audioClipActionCharacterRightFoot; //Done

    [SerializeField] private AudioClip audioClipActionCharacterLeftFootLadder; //Needs Feedback
    [SerializeField] private AudioClip audioClipActionCharacterRightFootLadder; //Needs Feedback

    [Header("Audio Enemy")]
    [SerializeField] private AudioClip audioClipActionCharacterKnockedOut; //Done
    [SerializeField] private AudioClip audioClipActionCharacterImpact; 

    //Looping
    [Header("Audio Powers")]
    [SerializeField] private AudioClip audioClipActionExpansiveForceLoop; //Done
    
    [SerializeField] private AudioClip audioClipActionLevelUpEnter;
    [SerializeField] private AudioClip audioClipActionLevelUpBurstEffect; //Done
    
    [SerializeField] private AudioClip audioClipActionLevelUpPopUpEnter; //Done
    [SerializeField] private AudioClip audioClipActionLevelUpPopUpExit; //Done
    
    [SerializeField] private AudioClip audioClipActionForceBurstEnter; //Done
    [SerializeField] private AudioClip audioClipActionForceJumpEnter; //Done
    [SerializeField] private AudioClip audioClipActionForcePinEnter; //Done

    [Header("Audio Buttons")]
    [SerializeField] private AudioClip audioClipActionButtonSwitchIn; //Done
    [SerializeField] private AudioClip audioClipActionButtonSwitchOut; //Done

    [SerializeField] private AudioClip audioClipActionButtonPressureIn; //Done
    [SerializeField] private AudioClip audioClipActionButtonPressureOut; //Done
    
    //Looping
    [SerializeField] private AudioClip audioClipActionButtonTimerIn; //Done
    [SerializeField] private AudioClip audioClipActionButtonTimerOut; //Done

    //Looping
    [SerializeField] private AudioClip audioClipActionButtonTimerLoop; //Done

    [Header("Audio Doors")]
    [SerializeField] private AudioClip audioClipActionDoorOpenStart; //Done
    [SerializeField] private AudioClip audioClipActionDoorCloseStart; //Done

    [SerializeField] private AudioClip audioClipActionDoorOpenEnd; //Done
    [SerializeField] private AudioClip audioClipActionDoorCloseEnd; //Done

    [SerializeField] private AudioClip audioClipActionDoorSecurityAccepeted; //Done
    [SerializeField] private AudioClip audioClipActionDoorSecurityDenied; //Done
    
    //Looping
    [SerializeField] private AudioClip audioClipActionDoorSecurityScanLoop; //Done

    //Looping
    [Header("Audio Vents")]
    [SerializeField] private AudioClip audioClipActionVentDoorPullLoop; //Done
    [SerializeField] private AudioClip audioClipActionVentDoorPop; //Done
    
    [SerializeField] private AudioClip audioClipActionVentDoorCollide; //Done

    [Header("Audio Boxes")]
    [SerializeField] private AudioClip audioClipBoxCollision; //Done

    [Header("Audio Pills")]
    [SerializeField] private AudioClip audioClipAquirePill; //Done


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //Single
        JumpController jumpController = FindObjectOfType<JumpController>(); // 1
        PlayerController playerController = FindObjectOfType<PlayerController>();
        ExpansiveForce expansiveForce = FindObjectOfType<ExpansiveForce>(); //2
        UpgradeStatusController upgradeStatusController = FindObjectOfType<UpgradeStatusController>(); //1
        VisualUpgradeController visualUpgradeController = FindObjectOfType<VisualUpgradeController>(); //1
        CameraController cameraController = FindObjectOfType<CameraController>(); //1
        TutorialMananger tutorialMananger = FindObjectOfType<TutorialMananger>(); //1
        BurstForceController burstForceController = FindObjectOfType<BurstForceController>(); //1
        ForceProjectCharacterController forceProjectCharacterController = FindObjectOfType<ForceProjectCharacterController>(); //1
        PinForceController pinForceController = FindObjectOfType<PinForceController>(); //1
        CharacterPlayerAnimatorController characterPlayerAnimator = FindObjectOfType<CharacterPlayerAnimatorController>();

        //Multiple

        List<EnemyController> enemyControllers = new List<EnemyController>();
        enemyControllers.AddRange(FindObjectsOfType<EnemyController>());

        List<ButtonSwitchController> buttonSwitchControllers = new List<ButtonSwitchController>(); //2
        buttonSwitchControllers.AddRange(FindObjectsOfType<ButtonSwitchController>());

        List<ButtonPressureController> buttonPressureControllers = new List<ButtonPressureController>(); //2
        buttonPressureControllers.AddRange(FindObjectsOfType<ButtonPressureController>());

        List<ButtonTimerController> buttonTimerControllers = new List<ButtonTimerController>(); //3
        buttonTimerControllers.AddRange(FindObjectsOfType<ButtonTimerController>());

        List<DoorController> doorControllers = new List<DoorController>(); //2
        doorControllers.AddRange(FindObjectsOfType<DoorController>());

        List<DoorDetectionController> doorDetectionControllers = new List<DoorDetectionController>(); //4
        doorDetectionControllers.AddRange(FindObjectsOfType<DoorDetectionController>());

        List<WiggleController> wiggleControllers = new List<WiggleController>(); //3
        wiggleControllers.AddRange(FindObjectsOfType<WiggleController>());

        List<PillAcquirePowerController> pillAcquirePowerControllers = new List<PillAcquirePowerController>(); //1
        pillAcquirePowerControllers.AddRange(FindObjectsOfType<PillAcquirePowerController>());

        //Warning contains both characters and boxes
        List<ForceInteractableObjectController> forceInteractableObjectControllers = new List<ForceInteractableObjectController>();
        forceInteractableObjectControllers.AddRange(FindObjectsOfType<ForceInteractableObjectController>());

        //Single

        jumpController.OnJumpAction += OnAudioActionCharacterJump;
        playerController.OnLanded += OnAudioActionCharacterLanded;

        characterPlayerAnimator.OnFootPlacmentLeft += OnAudioActionCharacterLeftFoot;
        characterPlayerAnimator.OnFootPlacmentRight += OnAudioActionCharacterRightFoot;

        characterPlayerAnimator.OnLeftFootLadder += OnAudioActionCharacterLeftFootLadder;
        characterPlayerAnimator.OnRightFootLadder += OnAudioActionCharacterRightFootLadder;


        expansiveForce.OnExpandEnter += OnAudioActionExpansiveForceEnter;
        expansiveForce.OnExpandExit += OnAudioActionExpansiveForceExit;

        upgradeStatusController.UpgradeEnter += OnAudioActionLevelUpEnter;
        visualUpgradeController.OnForceBurst += OnAudioActionLevelUpBurstEffect;
        cameraController.OnPopUpOpen += OnAudioActionLevelUpPopUpEnter;
        tutorialMananger.OnTutorialComplete += OnAudioActionLevelUpPopUpExit;
        burstForceController.OnForceBurstEnter += OnAudioActionForceBurstEnter;
        forceProjectCharacterController.OnForceJumpEnter += OnAudioActionForceJumpEnter;
        pinForceController.OnPinActionEnter += OnAudioActionForcePinEnter;

        for (int i = 0; i < enemyControllers.Count; i++)
        {
            enemyControllers[i].OnEnemyKnockedOut += OnAudioActionEnemyKnockedOut;
        }

        if(buttonSwitchControllers.Count > 0)
        {
            for (int i = 0; i < buttonSwitchControllers.Count; i++)
            {
                buttonSwitchControllers[i].OnButtonSwitchOn += OnAudioActionButtonSwitchIn;
                buttonSwitchControllers[i].OnButtonSwitchOff += OnAudioActionButtonSwitchOut;
            }
        }

        if(buttonPressureControllers.Count > 0)
        {
            for (int i = 0; i < buttonPressureControllers.Count; i++)
            {
                buttonPressureControllers[i].OnButtonPressureSwitchOn += OnAudioActionButtonPressureIn;
                buttonPressureControllers[i].OnButtonPressureSwitchOff += OnAudioActionButtonPressureOut;
            }
        }

        if(buttonTimerControllers.Count > 0)
        {
            for (int i = 0; i < buttonTimerControllers.Count; i++)
            {
                buttonTimerControllers[i].OnButtonTimerOn += OnAudioActionButtonTimerIn;
                buttonTimerControllers[i].OnButtonTimerOff += OnAudioActionButtonTimerOut;
                
                buttonTimerControllers[i].OnTimerStart += OnAudioActionButtonTimerEnter;
                buttonTimerControllers[i].OnTimerEnd += OnAudioActionButtonTimerExit;
            }
        }

        if(doorControllers.Count > 0)
        {
            for (int i = 0; i < doorControllers.Count; i++)
            {
                doorControllers[i].OnDoorOpenStart += OnAudioActionDoorOpenStart;
                doorControllers[i].OnDoorCloseStart += OnAudioActionDoorCloseStart;

                doorControllers[i].OnDoorOpenEnd += OnAudioActionDoorOpenEnd;
                doorControllers[i].OnDoorCloseEnd += OnAudioActionDoorCloseEnd;
            }
        }

        if(doorDetectionControllers.Count > 0)
        {
            for (int i = 0; i < doorDetectionControllers.Count; i++)
            {
                doorDetectionControllers[i].OnScanAccepted += OnAudioActionDoorSecurityAccepeted;
                doorDetectionControllers[i].OnScanDenied += OnAudioActionDoorSecurityDenied;
                doorDetectionControllers[i].OnScanStart += OnAudioActionDoorSecurityScanStart;
                doorDetectionControllers[i].OnScanEnd += OnAudioActionDoorSecurityScanEnd;
            }
        }

        if (wiggleControllers.Count > 0) 
        {
            for (int i = 0; i < wiggleControllers.Count; i++)
            {
                wiggleControllers[i].OnWiggleUpdateEnter += OnAudioActionVentDoorPullEnter;
                wiggleControllers[i].OnWiggleUpdateExit += OnAudioActionVentDoorPullExit;
                wiggleControllers[i].OnWiggleRelease += OnAudioActionVentDoorPop;
                wiggleControllers[i].OnWiggleCollision += OnAudioActionVentDoorCollide;
            }
        }


        if(forceInteractableObjectControllers.Count > 0)
        {
            for (int i = 0; i < forceInteractableObjectControllers.Count; i++)
            {
                if(!forceInteractableObjectControllers[i].GetComponent<EnemyController>())
                forceInteractableObjectControllers[i].OnCollisionSound += OnAudioBoxCollision;
            }
        }

        if(pillAcquirePowerControllers.Count > 0)
        {
            for (int i = 0; i < pillAcquirePowerControllers.Count; i++)
            {
                pillAcquirePowerControllers[i].OnPillAquired += OnAudioActionAquirePill;
            }
        }
    }

    //Single Objects

    public void OnAudioActionCharacterJump(JumpController jumpController, Vector3 pos)
    {
        //PlayEventSoundOneShot(audioClipTestSound, pos);
    }

    public void OnAudioActionCharacterLanded(PlayerController playerController, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionCharacterLanded, pos);
    }

    public void OnAudioActionCharacterLeftFoot(CharacterPlayerAnimatorController characterPlayer, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionCharacterLeftFoot, pos);
    }

    public void OnAudioActionCharacterRightFoot(CharacterPlayerAnimatorController characterPlayer, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionCharacterRightFoot, pos);
    }

    public void OnAudioActionCharacterLeftFootLadder(CharacterPlayerAnimatorController characterPlayer, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionCharacterLeftFootLadder, pos);
    }

    public void OnAudioActionCharacterRightFootLadder(CharacterPlayerAnimatorController characterPlayer, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionCharacterRightFootLadder, pos);
    }

    public void OnAudioActionEnemyKnockedOut(EnemyController enemyController, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionCharacterKnockedOut, pos);
    }

    public void OnAudioActionExpansiveForceEnter(LoopingSound loopingSound, ExpansiveForce expansiveForce, Vector3 pos)
    {
        PlayEventSoundLoop(loopingSound, audioClipActionExpansiveForceLoop, pos);
    }

    public void OnAudioActionExpansiveForceExit(LoopingSound loopingSound, ExpansiveForce expansiveForce, Vector3 pos)
    {
        StopAndRemoveLoopingSound(loopingSound);
    }

    public void OnAudioActionLevelUpEnter(UpgradeStatusController upgradeStatusController, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionLevelUpEnter, pos);
    }

    public void OnAudioActionLevelUpBurstEffect(VisualUpgradeController visualUpgradeController, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionLevelUpBurstEffect, pos);
    }

    public void OnAudioActionLevelUpPopUpEnter(CameraController cameraController, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionLevelUpPopUpEnter, pos);
    }

    public void OnAudioActionLevelUpPopUpExit(TutorialMananger tutorialMananger, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionLevelUpPopUpExit, pos);
    }

    public void OnAudioActionForceBurstEnter(BurstForceController burstForceController, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionForceBurstEnter, pos);
    }

    public void OnAudioActionForceJumpEnter(ForceProjectCharacterController forceProjectCharacterController, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionForceJumpEnter, pos);
    }

    public void OnAudioActionForcePinEnter(PinForceController pinForceController, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionForcePinEnter, pos);
    }

    //Multiple Objects

    public void OnAudioActionButtonSwitchIn(ButtonSwitchController buttonSwitchController, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionButtonSwitchIn, pos);
    }

    public void OnAudioActionButtonSwitchOut(ButtonSwitchController buttonSwitchController, Vector3 pos)
    {
        //Cant test as button does not allow switch out
        //PlayEventSoundOneShot(audioClipTestSound, pos);
    }

    public void OnAudioActionButtonPressureIn(ButtonPressureController buttonPressureController, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionButtonPressureIn, pos);
    }

    public void OnAudioActionButtonPressureOut(ButtonPressureController buttonPressureController, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionButtonPressureOut, pos);
    }

    public void OnAudioActionButtonTimerIn(ButtonTimerController buttonTimerController, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionButtonTimerIn, pos);
    }

    public void OnAudioActionButtonTimerOut(ButtonTimerController buttonTimerController, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionButtonTimerOut, pos);
    }

    public void OnAudioActionButtonTimerEnter(LoopingSound loopingSound, ButtonTimerController buttonTimerController, Vector3 pos)
    {
        PlayEventSoundLoop(loopingSound, audioClipActionButtonTimerLoop, pos);
    }

    public void OnAudioActionButtonTimerExit(LoopingSound loopingSound, ButtonTimerController buttonTimerController, Vector3 pos)
    {
        StopAndRemoveLoopingSound(loopingSound);
    }

    public void OnAudioActionDoorOpenStart(DoorController doorController, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionDoorOpenStart, pos);
    }

    public void OnAudioActionDoorCloseStart(DoorController doorController, Vector3 pos)
    {
        //Debug.Log("OnAudioActionDoorClose Called");
        PlayEventSoundOneShot(audioClipActionDoorCloseStart, pos);
    }

    public void OnAudioActionDoorOpenEnd(DoorController doorController, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionDoorOpenEnd, pos);
    }

    public void OnAudioActionDoorCloseEnd(DoorController doorController, Vector3 pos)
    {
        //Debug.Log("OnAudioActionDoorClose Called");
        PlayEventSoundOneShot(audioClipActionDoorCloseEnd, pos);
    }

    public void OnAudioActionDoorSecurityAccepeted(DoorDetectionController doorDetectionController, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionDoorSecurityAccepeted, pos);
    }

    public void OnAudioActionDoorSecurityDenied(DoorDetectionController doorDetectionController, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionDoorSecurityDenied, pos);
    }

    public void OnAudioActionDoorSecurityScanStart(LoopingSound loopingSound, DoorDetectionController doorDetectionController, Vector3 pos)
    {
        PlayEventSoundLoop(loopingSound, audioClipActionDoorSecurityScanLoop, pos);
    }

    public void OnAudioActionDoorSecurityScanEnd(LoopingSound loopingSound, DoorDetectionController doorDetectionController, Vector3 pos)
    {
        StopAndRemoveLoopingSound(loopingSound);
    }

    public void OnAudioActionVentDoorPullEnter(LoopingSound loopingSound, WiggleController wiggleController, Vector3 pos)
    {
        PlayEventSoundLoop(loopingSound, audioClipActionVentDoorPullLoop, pos);
    }

    public void OnAudioActionVentDoorPullExit(LoopingSound loopingSound, WiggleController wiggleController, Vector3 pos)
    {
        StopAndRemoveLoopingSound(loopingSound);
    }

    public void OnAudioActionVentDoorPop(WiggleController wiggleController, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionVentDoorPop, pos);
    }

    public void OnAudioActionVentDoorCollide(WiggleController wiggleController, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipActionVentDoorCollide, pos);
    }

    public void OnAudioActionAquirePill(PillAcquirePowerController pillAcquire, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipAquirePill, pos);
    }

    public void OnAudioBoxCollision(ForceInteractableObjectController forceInteractable, Vector3 pos)
    {
        PlayEventSoundOneShot(audioClipBoxCollision, pos);
    }
   
    public void PlayEventSoundOneShot(AudioClip audioClip, Vector3 pos)
    {
        //Test Locational Sound
        SoundClipController soundClipController = ObjectPool.instance.GetObjectFromPool(soundClipControllerPrefab.name).GetComponent<SoundClipController>();
        soundClipController.transform.position = pos;
        soundClipController.GetAudioSource().clip = audioClip;
        soundClipController.gameObject.SetActive(true);
        soundClipController.PlayAudioClip();
    }

    public void PlayEventSoundLoop(LoopingSound looping, AudioClip audioClip, Vector3 pos)
    {
        if(GetLoopingAudioController1FromLoopingSound(looping) != null)
        {

        }
        else
        {
            CreateNewLoopingSound(looping, audioClip, pos);
        }
    }

    private void CreateNewLoopingSound(LoopingSound looping, AudioClip audioClip, Vector3 pos)
    {
        SoundClipController soundClipController = ObjectPool.instance.GetObjectFromPool(soundClipControllerPrefab.name).GetComponent<SoundClipController>();
        LoopingAudioController loopingAudioController = ObjectPool.instance.GetObjectFromPool(loopingAudioControllerPrefab.name).GetComponent<LoopingAudioController>();

        loopingAudioController.gameObject.SetActive(true);
        loopingAudioController.SetLoopingSound(looping);
        loopingAudioController.SetSoundClipController(soundClipController);
        loopingAudioControllers.Add(loopingAudioController);

        soundClipController.transform.position = pos;
        soundClipController.GetAudioSource().clip = audioClip;
        soundClipController.gameObject.SetActive(true);
        soundClipController.GetAudioSource().loop = true;
        soundClipController.PlayAudioClip();
    }

    [SerializeField] private List<LoopingAudioController> loopingAudioControllers = new List<LoopingAudioController>();

    private void StopAndRemoveLoopingSound(LoopingSound loopingSound)
    {
        if (GetLoopingAudioController1FromLoopingSound(loopingSound) != null)
        {
            LoopingAudioController loopingAudio = GetLoopingAudioController1FromLoopingSound(loopingSound);
            SoundClipController soundClipController = loopingAudio.GetSoundClipController();

            soundClipController.StopPlayAudioClipRoutine();
            soundClipController.PlayAudioOver();

            loopingAudioControllers.Remove(loopingAudio);
            loopingAudio.gameObject.SetActive(false);
        }
    }

    public LoopingAudioController GetLoopingAudioController1FromLoopingSound(LoopingSound loopingSound)
    {
        LoopingAudioController loopingAudio = null;

        for (int i = 0; i < loopingAudioControllers.Count; i++)
        {
            if(loopingAudioControllers[i].GetLoopingSound() == loopingSound) 
            {
                loopingAudio = loopingAudioControllers[i];
                break;
            }
        }

        return loopingAudio;
    }

}
