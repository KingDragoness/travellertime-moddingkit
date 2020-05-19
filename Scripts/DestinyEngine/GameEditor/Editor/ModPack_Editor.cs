using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using DestinyEngine;

public class ModPack_Editor : EditorWindow
{
    public ModPack_Data modData = new ModPack_Data();

    [MenuItem("Mod Tools/Edit Modpack Settings")]
    static void Init()
    {
        ModPack_Editor window = (ModPack_Editor)EditorWindow.GetWindow(typeof(ModPack_Editor));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Modpack Settings", EditorStyles.boldLabel);
        modData.modName = EditorGUILayout.TextField(new GUIContent("Mod Name", "Name it based on the exported file."), modData.modName);

        modData.loadImmediately = EditorGUILayout.Toggle("Load Immediately", modData.loadImmediately);

        if (!modData.loadImmediately)
        {
            modData.GCECoord = EditorGUILayout.Vector3Field("GCE Coord", modData.GCECoord);
        }

        if (GUILayout.Button("Save Modpack Setting"))
        {
            Save_ModpackSetting();
        }
}

    void Save_ModpackSetting()
    {
        modData.objectName = modData.ToString();
        string content = JsonUtility.ToJson(modData, true);
        File.WriteAllText(EditorUtility.SaveFilePanel("Save modpack setting (.setting)", "give random name here", "setting", "setting"), content);

        Debug.Log("Modpack setting has been saved.");
    }

    protected void OnEnable()
    {
        // Here we retrieve the data if it exists or we save the default field initialisers we set above
        var data = EditorPrefs.GetString("AnExampleWindow", JsonUtility.ToJson(this, false));
        // Then we apply them to this window
        JsonUtility.FromJsonOverwrite(data, this);
    }

    protected void OnDisable()
    {
        // We get the Json data
        var data = JsonUtility.ToJson(this, false);
        // And we save it
        EditorPrefs.SetString("AnExampleWindow", data);

        // Et voilà !
    }
}