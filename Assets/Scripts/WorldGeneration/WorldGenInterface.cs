using UnityEditor;
using UnityEngine;

namespace WorldGeneration
{
    [CustomEditor(typeof(WorldGenerator))]
    public class WorldGenInterface : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var script = (WorldGenerator)target;
            if (GUILayout.Button("Generate"))
            {
                script.Generate();
                script.generateWorldOnStart = false;
            }
        }
    }
}