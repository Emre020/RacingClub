using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CheckPointAI))]
public class CheckpointsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CheckPointAI script = (CheckPointAI)target;

        GUI.backgroundColor = Color.yellow;
        if (GUILayout.Button("Angle Size Checkpoint Walls") == true)
        {
            script.AngleSizeCheckpointWalls();
        }

        GUILayout.Label(script.Description());
    }
}
