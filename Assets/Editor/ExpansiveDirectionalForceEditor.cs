using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DirectionalExpanseController))]
public class ExpansiveDirectionalForceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DirectionalExpanseController directionalExpanse = (DirectionalExpanseController)target;

        if (GUILayout.Button("Set Path"))
            directionalExpanse.CreatePath();

        if (GUILayout.Button("Follow Path"))
            directionalExpanse.FollowPath();

        base.OnInspectorGUI();
    }
}
