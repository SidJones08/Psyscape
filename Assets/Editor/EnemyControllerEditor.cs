using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyController))]
public class EnemyControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnemyController enemyController = (EnemyController)target;

        if (GUILayout.Button("Test Knocked Out"))
            enemyController.KnockedOut();

        base.OnInspectorGUI();
    }
}
