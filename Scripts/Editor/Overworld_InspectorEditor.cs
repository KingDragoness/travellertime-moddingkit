using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DestinyEngine;

[CustomEditor(typeof(OverworldEdit))]
public class Overworld_InspectorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        OverworldEdit overworldEdit = (OverworldEdit)target;

        var style0 = new GUIStyle(GUI.skin.label) { font = (Font)Resources.Load("Fonts/AbadiMTStd.otf", typeof(Font)) };
        EditorGUILayout.LabelField("Overworld Editor", style0);

        #region Horizontal 1
        EditorGUILayout.BeginHorizontal();
            var style1 = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };

            Vector3 GCE_Position = SceneView.GetAllSceneCameras()[0].transform.position;

            EditorGUILayout.LabelField("Current GCE: (" + Mathf.Floor(GCE_Position.x).ToString() + ", " + Mathf.Floor(GCE_Position.z).ToString()+ ")", style1);
        EditorGUILayout.EndHorizontal();
        #endregion

        #region Horizontal 2
        EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Load World", GUILayout.Width(95)))
            {
                overworldEdit.Load_WorldMap(); 
            }
            if (GUILayout.Button("Save World", GUILayout.Width(95)))
            {
                var path = EditorUtility.SaveFilePanelInProject("Save World Data", Application.dataPath, "json", "Save World Data");
                overworldEdit.Save_WorldMap(path);
            }

        EditorGUILayout.EndHorizontal();
        #endregion

        if (GUILayout.Button("Refresh!", GUILayout.Height(42)))
        {
            overworldEdit.Update_WorldMap();
        }
        EditorGUILayout.HelpBox("Refresh! to reload accidentally deleted OverworldEditor_Island gameobjects.", MessageType.Info);

        EditorUtility.SetDirty(target);

        DrawDefaultInspector();

    }

}
