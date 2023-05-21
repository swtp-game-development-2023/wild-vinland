using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using World;

///<summary>
/// Class to Creates Buttons to Control Saving Loadung and Clearing from Unity Editor
///</summary>
[CustomEditor(typeof(MapSaveSystem))]
public class MapSaveSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var script = (MapSaveSystem)target;
        if (GUILayout.Button("Save Map"))
        {
            script.SaveMap();
        }

        if (GUILayout.Button("Clear Map"))
        {
            WorldHelper.ClearMap();
        }

        if (GUILayout.Button("Load Map"))
        {
            script.LoadMap();
        }
    }
}