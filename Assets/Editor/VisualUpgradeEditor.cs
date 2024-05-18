using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VisualUpgradeController))]
public class VisualUpgradeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        VisualUpgradeController visualUpgrade = (VisualUpgradeController)target;

        if (GUILayout.Button("Upgrade"))
            visualUpgrade.StartUpgradeRoutine();

        base.OnInspectorGUI();
    }
}
