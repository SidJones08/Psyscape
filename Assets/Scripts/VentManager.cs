using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentManager : MonoBehaviour
{
    [SerializeField] private List<VentController> ventControllersAll = new List<VentController>();
    
    private void Awake()
    {
        ventControllersAll.AddRange(FindObjectsOfType<VentController>());
    }

    public void Start()
    {
        for (int i = 0; i < ventControllersAll.Count; i++)
        {
            ventControllersAll[i].OnVentEnter += OnVentEnterCheck;
            ventControllersAll[i].OnVentExit += OnVentExitCheck;
        }
    }

    public void OnVentEnterCheck()
    {
        for (int i = 0; i < ventControllersAll.Count; i++)
        {
            ventControllersAll[i].VentEnterUpdateVisual();
        }
    }

    public void OnVentExitCheck()
    {
        if(GetOccupiedVents().Count == 0)
        {
            for (int i = 0; i < ventControllersAll.Count; i++)
            {
                ventControllersAll[i].VentExitUpdateVisual();
            }
        }
    }

    public VentController GetVentFromPosition(Vector3 pos)
    {
        VentController ventController = null;

        for (int i = 0; i < ventControllersAll.Count; i++)
        {
            if(ventControllersAll[i].transform.position == pos)
            {
                ventController = ventControllersAll[i];
                break;
            }
        }

        return ventController;
    }

    public List<VentController> GetOccupiedVents()
    {
        List<VentController> vents = new List<VentController>();

        for (int i = 0; i < ventControllersAll.Count; i++)
        {
            if (ventControllersAll[i].GetIsOccupied())
                vents.Add(ventControllersAll[i]);
        }

        return vents;
    }
}
