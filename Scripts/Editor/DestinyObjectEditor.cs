using System;
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
        private Object objectTarget = null;
        private bool isCreateNewObjectMode = false;

        [MenuItem("Destiny Engine/Object Database Editor Window")]
        public static void OpenWindow(ObjectDatabase objectdatabase_, ObjectEditor_TypeList typelist, Object objectTarget_, bool isCreateNewObjectMode_ = false)
        {
            DestinyObjectEditor window = GetWindow<DestinyObjectEditor>();

            window.typeList = typelist;
            window.serializedObject = new SerializedObject(objectdatabase_);
            window.objectDatabase = objectdatabase_;
            window.objectTarget = objectTarget_;
            window.isCreateNewObjectMode = isCreateNewObjectMode_;
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

                default:

                    break;
            }

            if (index == -1)
            {
                Debug.LogError("Something wrong is going on. The ObjectDatabase may be messed up.");
            }

            currentProperty = serializedObject.FindProperty("Data");
            SerializedProperty serializedList = currentProperty.FindPropertyRelative(listName).GetArrayElementAtIndex(index);
            DrawProperties(serializedList, true);

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
                default:

                    break;
            }
        }

    }
}