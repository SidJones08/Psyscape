using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DoorController))]
public class DoorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DoorController doorController = (DoorController)target;

        if (GUILayout.Button("Door Open"))
            doorController.OpenDoor();

        if (GUILayout.Button("Door Close"))
            doorController.CloseDoor();

        base.OnInspectorGUI();
    }
}
