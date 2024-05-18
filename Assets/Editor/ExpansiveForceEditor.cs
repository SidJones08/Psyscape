using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ExpansiveForce))]
public class ExpansiveForceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ExpansiveForce expansiveForce = (ExpansiveForce)target;

        //if (GUILayout.Button("Expand Routine"))
        //    expansiveForce.StartExpandRoutine();

        /*
        if (GUILayout.Button("Create Positions"))
            expansiveForce.CreatePositions();

        if (GUILayout.Button("Clear Positions"))
            expansiveForce.ClearPositions();
        */


        base.OnInspectorGUI();
    }
}
