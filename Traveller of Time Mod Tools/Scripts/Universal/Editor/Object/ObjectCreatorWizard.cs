using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using DestinyEngine;
using DestinyEngine.Object;
using DestinyEngine.Utility;

namespace DestinyEngine.Editor
{


    public class ObjectCreatorWizard : EditorWindow
    {

        public ObjectDatabase objectDatabase;
        public ObjectReferenceScript target;

        private GUISkin skin;
        private GUIStyle buttonStyle;
        private GUIStyle labelStyle;
        private Color almostwhite = new Color(1, 1, 1, 0.2f);

        public static string PATH_NEW_FOLDER_OBJECT = Application.streamingAssetsPath;
        public static string PATH_DEFAULT_DATABASE = Application.streamingAssetsPath;

        [MenuItem("Destiny Engine/Object Creator Wizard")]
        static void Init()
        {
            ObjectCreatorWizard window = (ObjectCreatorWizard)EditorWindow.GetWindow(typeof(ObjectCreatorWizard), false, "Object Creator Wizard");
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

        protected void OnEnable()
        {
            skin = (GUISkin)Resources.Load("DatabaseSkin");
            buttonStyle = skin.GetStyle("Button");
            labelStyle = skin.GetStyle("Label");

            PATH_NEW_FOLDER_OBJECT = EditorPrefs.GetString("PATH_NEW_FOLDER_OBJECT", PATH_DEFAULT_DATABASE);

        }

        protected void OnDisable()
        {
            EditorPrefs.SetString("PATH_NEW_FOLDER_OBJECT", PATH_NEW_FOLDER_OBJECT);
        }

        void OnGUI()
        {
            GUILayout.Label("Object Creator Wizard", EditorStyles.boldLabel);
            GUILayout.Label("Tool to automatically create object class.");
            GUILayout.Space(10);

            objectDatabase = (ObjectDatabase)EditorGUILayout.ObjectField(objectDatabase, typeof(ObjectDatabase), false, GUILayout.MaxWidth(200));

            if (GUILayout.Button(PATH_NEW_FOLDER_OBJECT, buttonStyle, GUILayout.Width(500)))
            {
                string path = EditorUtility.OpenFolderPanel("Folder path for output creation", PATH_NEW_FOLDER_OBJECT, "");

                if (path.Length != 0)
                {
                    PATH_NEW_FOLDER_OBJECT = path;
                    EditorPrefs.SetString("PATH_NEW_FOLDER_OBJECT", PATH_NEW_FOLDER_OBJECT);
                }
            }

            DrawUILine(almostwhite);

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Object Reference target: ");
                target = (ObjectReferenceScript)EditorGUILayout.ObjectField(target, typeof(ObjectReferenceScript), true, GUILayout.Width(250));
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Object Reference type: ");

                if (target is WorldObjectScript)
                {
                    GUILayout.Label("WorldObject");
                }
                else if (target is PickableScript)
                {
                    GUILayout.Label("Pickable");
                }
                else if (target is ActorScript)
                {
                    GUILayout.Label("Actor");
                }
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            {
                AssessmentCheck();
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Create new object", GUILayout.MaxWidth(200)))
                {
                    DisplayDialog_New();
                }
            }
            EditorGUILayout.EndHorizontal();

        }

        private void DisplayDialog_New()
        {
            bool dontAllowCreation = false;

            bool hasSimilarBaseID = AnySimilarBaseID(target.Get_ObjectRefData().formID);

            if (hasSimilarBaseID)
            {
                Debug.LogError("Conflict detected! Database found similar ID!");
                dontAllowCreation = true;
            }

            if (target == null)
            {
                Debug.LogError("Conflict detected! Target objectreferencescript is empty!");
                dontAllowCreation = true;
            }

            if (dontAllowCreation)
            {
                return;
            }

            if (EditorUtility.DisplayDialog($"New object",
                $"Are you sure you want to create new object?", "Create", "Cancel"))
            {
                if (target is WorldObjectScript)
                {
                    CreateNew_WorldObject();
                }
                else if (target is PickableScript)
                {
                    CreateNew_Pickables();
                }
                else if (target is ActorScript)
                {
                    CreateNew_Actor();
                }
            }
        }

        #region Let's create a new object

        private void CreateNew_WorldObject()
        {
            WorldObjectScript worldObjectScript = target as WorldObjectScript;
            PickableScript pickableScript = target as PickableScript;
            ActorScript actorScript = target as ActorScript;

            var dir = NewFolder(target.gameObject.name);
            var path = $"{dir}/{target.gameObject.name}.prefab";

            var prefab1 = PrefabUtility.SaveAsPrefabAssetAndConnect(target.gameObject, path, InteractionMode.UserAction);

            BaseWorldObject baseWorldObject = new BaseWorldObject();
            baseWorldObject.ID = worldObjectScript.Get_ObjectRefData().formID.BaseID;
            baseWorldObject.gameModel = prefab1;

            objectDatabase.Data.allBaseWorldObjects.Add(baseWorldObject);
        }

        private void CreateNew_Pickables()
        {
            WorldObjectScript worldObjectScript = target as WorldObjectScript;
            PickableScript pickableScript = target as PickableScript;
            ActorScript actorScript = target as ActorScript;

            var dir = NewFolder(target.gameObject.name);
            var path = $"{dir}/{target.gameObject.name}.prefab";

            var prefab1 = PrefabUtility.SaveAsPrefabAssetAndConnect(target.gameObject, path, InteractionMode.UserAction);

            ItemData_Type itemType = MainUtility.Convert_ObjectToItemType(pickableScript.Get_Pickable_Data().formID.ObjectType);

            if (itemType == ItemData_Type.Ammo)
            {
                Item_Ammo item = new Item_Ammo();
                item.ID = pickableScript.Get_ObjectRefData().formID.BaseID;
                item.gameModel = prefab1;

                objectDatabase.Data.allItemAmmo.Add(item);
            }
            else if (itemType == ItemData_Type.Armor)
            {
                Item_Armor item = new Item_Armor();
                item.ID = pickableScript.Get_ObjectRefData().formID.BaseID;
                item.gameModel = prefab1;

                objectDatabase.Data.allItemArmors.Add(item);
            }
            else if (itemType == ItemData_Type.Consume)
            {
                Item_Consumables item = new Item_Consumables();
                item.ID = pickableScript.Get_ObjectRefData().formID.BaseID;
                item.gameModel = prefab1;

                objectDatabase.Data.allItemConsumables.Add(item);
            }
            else if (itemType == ItemData_Type.Junk)
            {
                Item_Junk item = new Item_Junk();
                item.ID = pickableScript.Get_ObjectRefData().formID.BaseID;
                item.gameModel = prefab1;

                objectDatabase.Data.allItemJunk.Add(item);
            }
            else if (itemType == ItemData_Type.Key)
            {
                Item_Key item = new Item_Key();
                item.ID = pickableScript.Get_ObjectRefData().formID.BaseID;
                item.gameModel = prefab1;

                objectDatabase.Data.allItemKey.Add(item);
            }
            else if (itemType == ItemData_Type.Misc)
            {
                Item_Misc item = new Item_Misc();
                item.ID = pickableScript.Get_ObjectRefData().formID.BaseID;
                item.gameModel = prefab1;

                objectDatabase.Data.allItemMiscs.Add(item);
            }
            else if (itemType == ItemData_Type.Weapon)
            {
                Item_Weapon item = new Item_Weapon();
                item.ID = pickableScript.Get_ObjectRefData().formID.BaseID;
                item.gameModel = prefab1;

                objectDatabase.Data.allItemWeapon.Add(item);
            }
        }

        private void CreateNew_Actor()
        {
            WorldObjectScript worldObjectScript = target as WorldObjectScript;
            PickableScript pickableScript = target as PickableScript;
            ActorScript actorScript = target as ActorScript;

            var dir = NewFolder(target.gameObject.name);
            var path = $"{dir}/{target.gameObject.name}.prefab";

            var prefab1 = PrefabUtility.SaveAsPrefabAssetAndConnect(target.gameObject, path, InteractionMode.UserAction);

            Actor actor = new Actor();
            actor.ID = actorScript.Get_ObjectRefData().formID.BaseID;
            actor.gameModel = prefab1;

            objectDatabase.Data.allBaseActors.Add(actor);
        }

        private string NewFolder(string folderName)
        {
            string unityRetardedPath = PATH_NEW_FOLDER_OBJECT + "/bait";

            List<string> allDirectories = new List<string>();
            int i = 0;
            var parentDir = Directory.GetParent(unityRetardedPath);
            

            while (parentDir != null)
            {
                if (i > 1000)
                {
                    Debug.Log("while loop has given up");
                    break;
                }

                allDirectories.Add(parentDir.Name);

                if (parentDir.Name == "Assets")
                {
                    break;
                }

                parentDir = Directory.GetParent(parentDir.FullName);
                i++;
            }

            allDirectories.Reverse();
            string result = "";

            foreach(var s in allDirectories)
            {
                result += $"{s}/";
            }

            result = result.Remove(result.Length - 1, 1);
            //loop until the get the stupid /Assets folder path

            string guid = AssetDatabase.CreateFolder(result, folderName);
            string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return result + "/" + folderName + "/";
        }

        #endregion



        #region Object Database conflict check

        private bool AnySimilarBaseID(FormID targetFormID)
        {
            bool result = false;

            var allBaseObjects = objectDatabase.GetAllBaseObjectsFromDatabase();

            foreach(var baseObject in allBaseObjects)
            {

                if (baseObject.ID == targetFormID.BaseID &&
                    MainUtility.Check_ObjectType(baseObject) == targetFormID.ObjectType)
                {
                    result = true;
                }
            }

            return result;
        }

        #endregion


        #region Object Assessments

        private void AssessmentCheck()
        {
            GUIStyle redLabel = new GUIStyle(EditorStyles.label);
            GUIStyle yellowLabel = new GUIStyle(EditorStyles.label);
            redLabel.normal.textColor = Color.red;
            yellowLabel.normal.textColor = new Color(0.47f, 0.38f, 0.1f);

            WorldObjectScript worldObjectScript = target as WorldObjectScript;
            PickableScript pickableScript = target as PickableScript;
            ActorScript actorScript = target as ActorScript;

            if (worldObjectScript != null)
            {

            }
            else if (pickableScript != null)
            {
                if (PickableHasAnyTag() == false)
                {
                    GUILayout.Label("No 'pickable' tag detected!", redLabel);
                }
                if (PickableItemDataFormMatch(pickableScript) == false)
                {
                    GUILayout.Label("FormID & ItemDat not match.", yellowLabel);
                }
            }
        }

        private bool PickableHasAnyTag()
        {
            bool result = false;

            var gameobjects = target.GetComponentsInChildren<Transform>();

            foreach(var go in gameobjects)
            {
                if (go.CompareTag("Pickable"))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        private bool PickableItemDataFormMatch(PickableScript pickScript)
        {
            bool result = false;

            var formID = target.Get_ObjectRefData().formID;

            ItemData itemDat = pickScript.Get_Pickable_Data().itemData;

            if (formID.BaseID == itemDat.ID
                && formID.DatabaseID == itemDat.DatabaseName
                && formID.ObjectType == MainUtility.Convert_ItemToObjectType(itemDat.item_Type))
            {
                result = true;
            }
            //match form with itemdat

            return result;
        }

        #endregion

    }

}