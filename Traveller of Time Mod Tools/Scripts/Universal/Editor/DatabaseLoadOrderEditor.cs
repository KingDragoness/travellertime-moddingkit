using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DestinyEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.IO;

[CustomEditor(typeof(DatabaseLoadOrderEdit))]
public class DatabaseLoadOrderEditor : OdinEditor
{
    DatabaseLoadOrderEdit databseEditor;

    public override void OnInspectorGUI()
    {
        databseEditor = (DatabaseLoadOrderEdit)target;
        var style0 = new GUIStyle(GUI.skin.label) { font = (Font)Resources.Load("Fonts/AbadiMTStd.otf", typeof(Font)) };
        EditorGUILayout.LabelField("Database Editor", style0);


        #region Horizontal 2
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Load", GUILayout.Width(95)))
        {
            LoadJSON();
        }
        if (GUILayout.Button("Save", GUILayout.Width(95)))
        {
            SaveJSON();
        }


        EditorGUILayout.EndHorizontal();
        #endregion

        EditorUtility.SetDirty(target);

        DrawDefaultInspector();

    }

    public void LoadJSON()
    {
        string filePath = EditorUtility.OpenFilePanel("Open database load order setting (.loadOrder)", Application.streamingAssetsPath, "loadOrder");
        databseEditor.databaseSettings = JsonUtility.FromJson<DatabaseLoadOrder_Data>(File.ReadAllText(filePath));
    }

    public void SaveJSON()
    {
        string content = JsonUtility.ToJson(databseEditor.databaseSettings, true);
        File.WriteAllText(EditorUtility.SaveFilePanel("Save database load order setting (.loadOrder)", Application.streamingAssetsPath, "loadOrder", "loadOrder"), content);

        Debug.Log("Database setting has been saved.");
    }

}
