using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ExpansiveForceBoarderVisualTileController))]
public class ExpansiveForceBoarderVisualEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ExpansiveForceBoarderVisualTileController forceBoarderVisual = (ExpansiveForceBoarderVisualTileController)target;

        if (GUILayout.Button("Detect Tiles"))
            forceBoarderVisual.DetectTiles();

        if (GUILayout.Button("Clear Tiles"))
            forceBoarderVisual.ClearTiles();

        base.OnInspectorGUI();
    }
}
