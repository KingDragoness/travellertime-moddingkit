using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using DestinyEngine;

public class ModPack_Editor : EditorWindow
{
    public WorldBundle_Setting packSetting = new WorldBundle_Setting();

    private string PATH_ALL_MODS = "";
    private Vector2 scrollPos;
    private Color almostwhite = new Color(1, 1, 1, 0.2f);

    [MenuItem("Mod Tools/Edit Modpack Settings")]
    static void Init()
    {
        ModPack_Editor window = (ModPack_Editor)EditorWindow.GetWindow(typeof(ModPack_Editor));
        window.Show();
    }

    public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }

    void OnGUI()
    {
        GUILayout.Label("Modpack Settings", EditorStyles.boldLabel);
        PATH_ALL_MODS = EditorGUILayout.TextField(new GUIContent("All mods folder path", "Auto-scan island pack settings"), PATH_ALL_MODS);


        if (GUILayout.Button("+",  GUILayout.MaxWidth(30)))
        {
            packSetting.packDatas.Add(new ModPack_Data());
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.MinWidth(300), GUILayout.MaxWidth(500), GUILayout.MinHeight(20), GUILayout.MaxHeight(1080));
        foreach (ModPack_Data modpack in packSetting.packDatas)
        {
            DrawUILine(almostwhite);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("-", GUILayout.MaxWidth(30)))
            {
                packSetting.packDatas.Remove(modpack);
                break;
            }
            modpack.modName = EditorGUILayout.TextField(new GUIContent("Mod Name", "Name it based on the exported file."), modpack.modName);
            GUILayout.EndHorizontal();
            modpack.loadImmediately = EditorGUILayout.Toggle("Load Immediately", modpack.loadImmediately);

            if (!modpack.loadImmediately)
            {
                modpack.GCECoord = EditorGUILayout.Vector3Field("GCE Coord", modpack.GCECoord);
            }

        }
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Scan vanilla islands"))
        {
            Scan_Mods(true);
        }
        if (GUILayout.Button("Scan Islands"))
        {
            Scan_Mods();
        }
        if (GUILayout.Button("Load Modpack Setting"))
        {
            Load_ModpackSetting();
        }
        if (GUILayout.Button("Save Modpack Setting"))
        {
            Save_ModpackSetting();
        }
    }

    void Scan_Mods(bool includeVanillaIsland = false)
    {
        List<string> islands = new List<string>();

        if (includeVanillaIsland)
        {
            string filePath = Application.streamingAssetsPath + "/OverworldData.worldDat";

            islands.Add(filePath);
        }
        else
        {
            islands = Directory.EnumerateFiles(PATH_ALL_MODS, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".json")).ToList();
        }

        foreach (string path in islands)
        {
            if (File.Exists(path))
            {

            }
            else
            {
                continue;
            }

            try
            {
                Overworld_MapData overworldData = JsonUtility.FromJson<Overworld_MapData>(File.ReadAllText(path));
                foreach(IslandData islandDat in overworldData.GlobalIslands)
                {
                    if (islandDat == null)
                        continue;

                    if (packSetting.packDatas.Find(x => x.modName == islandDat.ModPackName) == null)
                    {
                        ModPack_Data pack_Data = new ModPack_Data();
                        pack_Data.GCECoord  = islandDat.GCECoord;
                        pack_Data.modName   = islandDat.ModPackName;

                        Debug.Log(pack_Data.modName + " has been created.");
                        packSetting.packDatas.Add(pack_Data);
                    }
                    else if (packSetting.packDatas.Find(x => x.modName == islandDat.ModPackName) != null)
                    {
                        ModPack_Data pack_Data = packSetting.packDatas.Find(x => x.modName == islandDat.ModPackName);
                        pack_Data.GCECoord = islandDat.GCECoord;
                        pack_Data.modName = islandDat.ModPackName;

                        Debug.Log(pack_Data.modName + " has been written.");
                    }
                }

            }
            catch
            {
                continue;

            }
        }
    }

    void Load_ModpackSetting()
    {
        string filePath = EditorUtility.OpenFilePanel("Open modpack setting (.setting)", Application.dataPath, "setting");
        packSetting = JsonUtility.FromJson<WorldBundle_Setting>(File.ReadAllText(filePath));

    }

    void Save_ModpackSetting()
    {
        packSetting.objectName = packSetting.ToString();
        string content = JsonUtility.ToJson(packSetting, true);
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