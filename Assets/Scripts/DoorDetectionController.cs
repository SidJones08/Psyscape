using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DoorDetectionController : MonoBehaviour, LoopingSound
{
    public event Action<DoorDetectionController, Vector3> OnScanAccepted;
    public event Action<DoorDetectionController, Vector3> OnScanDenied;

    public event Action<LoopingSound, DoorDetectionController, Vector3> OnScanStart;
    public event Action<LoopingSound, DoorDetectionController, Vector3> OnScanEnd;

    [SerializeField] private bool canScan = true;
    [SerializeField] private bool isScanning;
    [SerializeField] private float elapsedTime = 0;
    [SerializeField] private float scanningDuration = 2f;

    [SerializeField] private LineRenderer scanLine;
    [SerializeField] private Material materialScan;

    [SerializeField] private Transform doorTransform; 

    [SerializeField] private List<MovementController> movementControllers = new List<MovementController>();

    private void Start()
    {
        scanLine.gameObject.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (canScan)
        {
            if (collision.GetComponent<MovementController>())
                if (!movementControllers.Contains(collision.GetComponent<MovementController>()))
                    movementControllers.Add(collision.GetComponent<MovementController>());

            if (movementControllers.Count > 0)
            {
                if (!isScanning)
                {
                    ScanEnter(movementControllers[0]);
                    StartCoroutine(ScanningRoutine(movementControllers[0]));

                    if (OnScanStart != null)
                        OnScanStart(this, this, transform.position);
                }
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (canScan)
        {
            if (collision.GetComponent<MovementController>())
            {
                if (movementControllers.Contains(collision.GetComponent<MovementController>()))
                {
                    collision.GetComponent<MovementController>().SetCharacterMaterial(collision.GetComponent<MovementController>().GetDefaultMaterial());
                    movementControllers.Remove(collision.GetComponent<MovementController>());
                }

                if (movementControllers.Count > 0)
                {
                    if (!isScanning)
                    {
                        ScanEnter(movementControllers[0]);
                        StartCoroutine(ScanningRoutine(movementControllers[0]));
                    }
                }
                else
                {
                    ScanCancelled();

                    if (OnScanEnd != null)
                        OnScanEnd(this, this, transform.position);
                }
            }
        }
    }

    IEnumerator ScanningRoutine(MovementController movement)
    {
        isScanning = true;

        while(elapsedTime < scanningDuration)
        {
            elapsedTime += Time.deltaTime;
            ScanUpdate(movement);
            yield return null;
        }

        if (movement.transform.GetComponent<EnemyController>())
        {
            ScanAccepted(movement);
        }
        else if (movement.transform.GetComponent<PlayerController>())
        {
            ScaneDenied(movement);
        }

        movementControllers.Remove(movement);
        elapsedTime = 0;
        isScanning = false;

        if (OnScanEnd != null)
            OnScanEnd(this, this, transform.position);
    }

    public void ScanEnter(MovementController movementController)
    {
        Material colMaterial = movementController.GetDefaultMaterial();

        materialScan.SetColor("_Color_Skin_Primary", colMaterial.GetColor("_Color_Skin_Primary"));
        materialScan.SetColor("_Color_Skin_Secondary", colMaterial.GetColor("_Color_Skin_Secondary"));
        materialScan.SetColor("_Color_Eye", colMaterial.GetColor("_Color_Eye"));
        materialScan.SetColor("_Color_Clothes_Primary", colMaterial.GetColor("_Color_Clothes_Primary"));
        materialScan.SetColor("_Color_Clothes_Secondary", colMaterial.GetColor("_Color_Clothes_Secondary"));

        movementController.SetCharacterMaterial(materialScan);

        scanLine.gameObject.SetActive(true);
    }

    public void ScanUpdate(MovementController movementController)
    {
        scanLine.SetPosition(0, doorTransform.position);
        scanLine.SetPosition(1, movementController.transform.position);
    }

    public void ScanAccepted(MovementController movementController)
    {
        movementController.SetCharacterMaterial(movementController.GetDefaultMaterial());
        scanLine.gameObject.SetActive(false);

        if (OnScanAccepted != null)
            OnScanAccepted(this, transform.position);

        Debug.Log("Accepted");
    }

    public void ScaneDenied(MovementController movementController) 
    {
        movementController.SetCharacterMaterial(movementController.GetDefaultMaterial());
        scanLine.gameObject.SetActive(false);

        if (OnScanDenied != null)
            OnScanDenied(this, transform.position);

        Debug.Log("Denied");
    }

    public void ScanCancelled()
    {
        scanLine.gameObject.SetActive(false);
        StopAllCoroutines();
        elapsedTime = 0;
        isScanning = false;
    }

    public bool GetCanScan() 
    {
        return canScan;
    }

    public void SetCanScan(bool value)
    {
        canScan = value;
    }

}
