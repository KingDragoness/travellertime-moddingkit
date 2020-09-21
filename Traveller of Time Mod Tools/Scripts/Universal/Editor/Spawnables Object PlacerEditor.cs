using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using DestinyEngine.Object;
using Newtonsoft.Json;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System.Linq;

namespace DestinyEngine.Editor
{
    [CustomEditor(typeof(SpawnablesObjectPlacer))]
    public class SpawnablesObjectPlacerEditor : OdinEditor
    {

        public ObjectDatabase currentDatabase;
        public ObjectData_Type objectType;

        private GUISkin skin;
        private GUIStyle buttonStyle;
        private GUIStyle labelStyle;

        public int selectedItem = 0;
        private BaseObject currentObject = null;

        SerializedProperty editorCellDat_Spawnables;
        SerializedProperty editorCell;

        private SpawnablesObjectPlacer objectPlacer;

        void OnEnable()
        {
            editorCellDat_Spawnables = serializedObject.FindProperty("editorCellDat_Spawnables");
            editorCell = serializedObject.FindProperty("editorCellObject");
            skin = (GUISkin)Resources.Load("DatabaseSkin");
            buttonStyle = skin.GetStyle("Button");
            labelStyle = skin.GetStyle("Label");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            objectPlacer = (SpawnablesObjectPlacer)target;
            EditorUtility.SetDirty(target);
            EditorUtility.SetDirty(objectPlacer.editorCellObject);
            serializedObject.Update();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Revert cell data", GUILayout.Width(150)))
                {
                    RevertCell();
                }

                if (GUILayout.Button("Initialize", GUILayout.Width(150)))
                {
                    objectPlacer.Initialize_ObjectPlacer();
                }

                if (GUILayout.Button("Clear all objects", GUILayout.Width(150)))
                {
                    objectPlacer.ClearAll_ObjectPlacer();
                }

                if (GUILayout.Button("Override cell data", GUILayout.Width(150)))
                {
                    OverrideCell();
                }

                if (GUILayout.Button("Save Cell as JSON", GUILayout.Width(150)))
                {
                    SaveAsJSON();
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Spawnable Editor", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("Box");
            {
                currentDatabase = (ObjectDatabase)EditorGUILayout.ObjectField(currentDatabase, typeof(ObjectDatabase), false, GUILayout.MaxWidth(200));

                List<string> options = new List<string>();
                List<BaseObject> baseObjects = new List<BaseObject>();

                if (currentDatabase != null)
                {
                    objectType = (ObjectData_Type)EditorGUILayout.EnumPopup("Object type", objectType);

                    if (objectType == ObjectData_Type.Ammo)
                    {
                        baseObjects.AddRange(currentDatabase.Data.allItemAmmo);
                    }
                    else if (objectType == ObjectData_Type.Armor)
                    {
                        baseObjects.AddRange(currentDatabase.Data.allItemAmmo);
                    }
                    else if (objectType == ObjectData_Type.BaseActor)
                    {
                        baseObjects.AddRange(currentDatabase.Data.allBaseActors);
                    }
                    else if (objectType == ObjectData_Type.BaseWorldObject)
                    {
                        baseObjects.AddRange(currentDatabase.Data.allBaseWorldObjects);
                    }
                    else if (objectType == ObjectData_Type.Junk)
                    {
                        baseObjects.AddRange(currentDatabase.Data.allItemJunk);
                    }
                    else if (objectType == ObjectData_Type.Key)
                    {
                        baseObjects.AddRange(currentDatabase.Data.allItemKey);
                    }
                    else if (objectType == ObjectData_Type.Quest)
                    {
                        baseObjects.AddRange(currentDatabase.Data.allBaseQuests);
                    }
                    else if (objectType == ObjectData_Type.Weapon)
                    {
                        baseObjects.AddRange(currentDatabase.Data.allItemWeapon);
                    }
                    else if (objectType == ObjectData_Type.Misc)
                    {
                        baseObjects.AddRange(currentDatabase.Data.allItemMiscs);
                    }

                    baseObjects.OrderBy(z => z);

                    foreach (BaseObject baseObject in baseObjects)
                    {
                        options.Add(baseObject.ID);
                    }
                }


                selectedItem = EditorGUILayout.Popup("Object", selectedItem, options.ToArray());

                if (baseObjects.Count > selectedItem)
                {
                    if (baseObjects[selectedItem] != null)
                    {
                        currentObject = baseObjects[selectedItem];
                    }
                }
                if (GUILayout.Button("Spawn new Object", GUILayout.Width(250)))
                {
                    if (currentObject != null)
                    {
                        objectPlacer.SpawnBrandNewObject(currentObject, currentDatabase.Data.name, objectType);
                    }
                    else
                    {
                        Debug.LogError("No object!");
                    }
                }
            }
            EditorGUILayout.EndVertical();

            objectPlacer.UpdateSpawnData();

            serializedObject.ApplyModifiedProperties();
        }


        private void RevertCell()
        {
            objectPlacer.editorCellDat_Spawnables = EditorCellDat_Spawnables.Copy(objectPlacer.editorCellObject.data.editorCellDat_Spawnables);
        }

        private void OverrideCell()
        {
            objectPlacer.editorCellObject.data.editorCellDat_Spawnables  = EditorCellDat_Spawnables.Copy(objectPlacer.editorCellDat_Spawnables);
            Debug.Log("Cell is saved!");
        }

        private void SaveAsJSON()
        {
            var path = EditorUtility.SaveFilePanel(
            "Save cell as JSON format",
            "",
            objectPlacer.editorCellObject.name + ".json",
            "json");

            string dataToSave = JsonConvert.SerializeObject(objectPlacer.editorCellDat_Spawnables, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            if (path.Length != 0)
            {
                File.WriteAllText(path, dataToSave);
            }

        }


    }
}

