using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DestinyEngine.Object
{
    public class DestinyObjectEditor : ExtendedEditorWindow
    {
        private ObjectEditor_TypeList typeList = ObjectEditor_TypeList.None;
        private ObjectDatabase objectDatabase = null;
        private BaseObject objectTarget = null;
        private bool isCreateNewObjectMode = false;
        private List<int>       indexes = new List<int>();
        private List<bool>      booleans = new List<bool>();
        private List<string>    string_cache = new List<string>();


        [MenuItem("Destiny Engine/Object Database Editor Window")]
        public static void OpenWindow(ObjectDatabase objectdatabase_, ObjectEditor_TypeList typelist, BaseObject objectTarget_, bool isCreateNewObjectMode_ = false)
        {
            DestinyObjectEditor window = GetWindow<DestinyObjectEditor>();

            window.typeList = typelist;
            window.serializedObject = new SerializedObject(objectdatabase_);
            window.objectDatabase = objectdatabase_;
            window.objectTarget = objectTarget_;
            window.isCreateNewObjectMode = isCreateNewObjectMode_;

            while(window.indexes.Count < 100)
            {
                window.indexes.Add(1);
            }
            while (window.string_cache.Count < 100)
            {
                window.string_cache.Add("*");
            }
            while (window.booleans.Count < 100)
            {
                window.booleans.Add(false);
            }
        }

        private void OnGUI()
        {
            if (isCreateNewObjectMode)
            {
                GUILayout.Label("Create New Object");
            }

            DrawInspector();

        }

        void DrawInspector()
        {

            int index = -1;
            string listName = "";

            switch (typeList)
            {
                case ObjectEditor_TypeList.Ammo:
                    index = objectDatabase.Data.allItemAmmo.FindIndex(x => x == objectTarget);
                    listName = "allItemAmmo";
                    break;

                case ObjectEditor_TypeList.Junk:
                    index = objectDatabase.Data.allItemJunk.FindIndex(x => x == objectTarget);
                    listName = "allItemJunk";
                    break;

                case ObjectEditor_TypeList.Key:
                    index = objectDatabase.Data.allItemKey.FindIndex(x => x == objectTarget);
                    listName = "allItemKey";
                    break;

                case ObjectEditor_TypeList.Weapon:
                    index = objectDatabase.Data.allItemWeapon.FindIndex(x => x == objectTarget);
                    listName = "allItemWeapon";
                    break;

                case ObjectEditor_TypeList.Misc:
                    index = objectDatabase.Data.allItemMiscs.FindIndex(x => x == objectTarget);
                    listName = "allItemMiscs";
                    break;

                case ObjectEditor_TypeList.BaseWorldObject:
                    index = objectDatabase.Data.allBaseWorldObjects.FindIndex(x => x == objectTarget);
                    listName = "allBaseWorldObjects";
                    break;

                default:

                    break;
            }

            if (index == -1)
            {
                Debug.LogError("Something wrong is going on. The ObjectDatabase may be messed up.");
            }

            currentProperty = serializedObject.FindProperty("Data");
            SerializedProperty serializedList = currentProperty.FindPropertyRelative(listName).GetArrayElementAtIndex(index);
            Attempt_DrawInspector(serializedList);

            EditorGUILayout.Space();

            if (!isCreateNewObjectMode)
            {
                if (GUILayout.Button("Save"))
                {
                    Apply();
                }
            }
            else
            {
                if (GUILayout.Button("Create Object"))
                {
                    Apply();
                    ObjectDatabaseEditorWindow window = (ObjectDatabaseEditorWindow)EditorWindow.GetWindow(typeof(ObjectDatabaseEditorWindow));
                    //Create_Object();
                    window.Change_List();
                    Close();
                }
            }
        }


        void Attempt_DrawInspector(SerializedProperty serializedList)
        {
            List<string> propNames = new List<string>();
            currentProperty = serializedList;

            switch (typeList)
            {
                case ObjectEditor_TypeList.Ammo:
                    DrawProperties(serializedList, true);

                    break;

                case ObjectEditor_TypeList.Junk:
                    DrawProperties(serializedList, true);

                    break;

                case ObjectEditor_TypeList.Key:
                    DrawProperties(serializedList, true);

                    break;

                case ObjectEditor_TypeList.Weapon:
                    propNames.AddRange(Get_ItemPropNames(objectTarget));
                    propNames.Reverse();

                    foreach(string s in propNames)
                    {
                        DrawField(s, true);

                        if (s == "ammoID")
                        {
                            booleans[0] = GUILayout.Toggle(booleans[0], "Show Ammo IDs");

                            if (booleans[0])
                            {
                                EditorGUILayout.BeginVertical("Box");

                                for (int x = 0; x < objectDatabase.Data.allItemAmmo.Count; x++)
                                {
                                    bool endBox = false;

                                    if (x % 3 == 0)
                                    {
                                        EditorGUILayout.BeginHorizontal();
                                    }
                                    else if (x % 3 == 2)
                                    {
                                        endBox = true;
                                    }

                                    EditorGUILayout.SelectableLabel(objectDatabase.Data.allItemAmmo[x].ID);

                                    if (endBox)
                                    {
                                        EditorGUILayout.EndHorizontal();
                                    }
                                    else if (x == objectDatabase.Data.allItemAmmo.Count - 1)
                                    {
                                        EditorGUILayout.EndHorizontal();
                                    }

                                }

                                EditorGUILayout.EndVertical();
                            }
                        }

                    }
                    break;

                case ObjectEditor_TypeList.Misc:
                    DrawProperties(serializedList, true);

                    break;

                case ObjectEditor_TypeList.BaseWorldObject:
                    DrawProperties(serializedList, true);

                    break;

                default:
                    DrawProperties(serializedList, true);

                    break;
            }
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="type">Item / Weapon</param>
        /// <returns></returns>
        List<string> Get_ItemPropNames(BaseObject object_)
        {
            List<string> propNames = new List<string>();
            var prop = object_.GetType().GetFields();
            foreach(FieldInfo p in prop)
            {
                propNames.Add(p.Name);
            }

            return propNames;
        }

        void Create_Object()
        {
            switch (typeList)
            {
                case ObjectEditor_TypeList.Ammo:
                    {
                        Item_Ammo newItem1 = objectTarget as Item_Ammo;
                        objectDatabase.Data.allItemAmmo.Add(newItem1);

                        break;
                    }
                case ObjectEditor_TypeList.Junk:
                    {
                        Item_Junk newItem2 = objectTarget as Item_Junk;
                        objectDatabase.Data.allItemJunk.Add(newItem2);

                        break;
                    }
                case ObjectEditor_TypeList.Key:
                    {
                        Item_Key newItem3 = objectTarget as Item_Key;
                        objectDatabase.Data.allItemKey.Add(newItem3);

                        break;
                    }
                case ObjectEditor_TypeList.Weapon:
                    {
                        Item_Weapon newItem4 = objectTarget as Item_Weapon;
                        objectDatabase.Data.allItemWeapon.Add(newItem4);

                        break;
                    }
                case ObjectEditor_TypeList.Misc:
                    {
                        Item_Misc newItem5 = objectTarget as Item_Misc;
                        objectDatabase.Data.allItemMiscs.Add(newItem5);

                        break;
                    }
                default:

                    break;
            }
        }

    }
}