using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using DestinyEngine;

namespace DestinyEngine.Object
{
    public enum ObjectEditor_TypeList
    {
        None,
        Ammo,
        Junk,
        Key,
        Weapon,
        Misc,
        BaseWorldObject
    }

    public class ObjectDatabaseEditorWindow : EditorWindow
    {


        public ObjectDatabase objectDatabase;
        private List<BaseObject>    pooled_Objects = new List<BaseObject>();
        private List<BaseObject>    filter_Objects = new List<BaseObject>();
        private string          wordFilter = "";


        private GUISkin skin;
        private GUIStyle buttonStyle;
        private GUIStyle labelStyle;
        private Color almostwhite = new Color(1, 1, 1, 0.2f);
        private ObjectEditor_TypeList typeList = ObjectEditor_TypeList.None;

        bool foldoutShow_Audio;
        bool foldoutShow_Item;

        Vector2 scrollPos_ItemCategory;
        Vector2 scrollPos_IDObject;

        [MenuItem("Destiny Engine/Object Database Editor Window")]
        public static void OpenWindow(ObjectDatabase objectDatabase)
        {
            ObjectDatabaseEditorWindow window = GetWindow<ObjectDatabaseEditorWindow>();
        }

        #region useless2
        /*
        protected override void OnGUI()
        {
            SirenixEditorGUI.Title("Object Database Editor", "I'm sick of it", TextAlignment.Left, true);
            objectDatabase = (ObjectDatabase)EditorGUILayout.ObjectField(objectDatabase, typeof(ObjectDatabase), false, GUILayout.MaxWidth(200));
            OdinMenuTree test = this.MenuTree;
            SirenixEditorGUI.

        }
        */
        #endregion

        #region GUI
        
        private void OnGUI()
        {
            GUILayout.Label("Object Database Editor", EditorStyles.boldLabel);
            objectDatabase = (ObjectDatabase) EditorGUILayout.ObjectField(objectDatabase, typeof(ObjectDatabase), false, GUILayout.MaxWidth(200));

            EditorGUILayout.BeginHorizontal("Box");
            {
                EditorGUILayout.BeginVertical("Box", GUILayout.Width(200), GUILayout.ExpandHeight(true));
                {
                    scrollPos_ItemCategory = EditorGUILayout.BeginScrollView(scrollPos_ItemCategory);
                    {
                        foldoutShow_Item = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutShow_Item, "Items");
                        if (foldoutShow_Item)
                        {
                            if (GUILayout.Button("Ammo", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.Ammo;
                                Change_List();
                            }
                            if (GUILayout.Button("Junk", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.Junk;
                                Change_List();
                            }
                            if (GUILayout.Button("Key", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.Key;
                                Change_List();
                            }
                            if (GUILayout.Button("Weapon", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.Weapon;
                                Change_List();
                            }
                            if (GUILayout.Button("Misc", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.Misc;
                                Change_List();
                            }
                        }
                        EditorGUILayout.EndFoldoutHeaderGroup();

                        foldoutShow_Audio = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutShow_Audio, "WorldObjects");
                        if (foldoutShow_Audio)
                        {
                            if (GUILayout.Button("Base WorldObject", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.BaseWorldObject;
                                Change_List();
                            }
                        }
                        EditorGUILayout.EndFoldoutHeaderGroup();
                    }
                    EditorGUILayout.EndScrollView();

                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("Box", GUILayout.Width(320), GUILayout.ExpandHeight(true));
                {
                    scrollPos_IDObject = EditorGUILayout.BeginScrollView(scrollPos_IDObject);
                    {
                        ShowItemsGUI();
                    }
                    EditorGUILayout.EndScrollView();

                }
                EditorGUILayout.EndVertical();

            }
            EditorGUILayout.EndHorizontal();
        }

        public void Change_List()
        {
            pooled_Objects.Clear();

            if (objectDatabase == null)
            {
                Debug.LogError("ObjectDatabase not found! Assign an already created ObjectDatabase or create new ObjectDatabase by right click into the project Create > DestinyEngine > ObjectDatabase!");
                return;
            }

            if (typeList == ObjectEditor_TypeList.Ammo)
            {
                objectDatabase.Data.allItemAmmo = objectDatabase.Data.allItemAmmo.OrderBy(z => z.ID).ToList();
                foreach (Item_Ammo item in objectDatabase.Data.allItemAmmo)
                {
                    pooled_Objects.Add(item);
                }
            }

            if (typeList == ObjectEditor_TypeList.Junk)
            {
                objectDatabase.Data.allItemJunk = objectDatabase.Data.allItemJunk.OrderBy(z => z.ID).ToList();
                foreach (Item_Junk item in objectDatabase.Data.allItemJunk)
                {
                    pooled_Objects.Add(item);
                }
            }

            if (typeList == ObjectEditor_TypeList.Key)
            {
                objectDatabase.Data.allItemKey = objectDatabase.Data.allItemKey.OrderBy(z => z.ID).ToList();
                foreach (Item_Key item in objectDatabase.Data.allItemKey)
                {
                    pooled_Objects.Add(item);
                }
            }

            if (typeList == ObjectEditor_TypeList.Weapon)
            {
                objectDatabase.Data.allItemWeapon = objectDatabase.Data.allItemWeapon.OrderBy(z => z.ID).ToList();
                foreach (Item_Weapon item in objectDatabase.Data.allItemWeapon)
                {
                    pooled_Objects.Add(item);
                }
            }

            if (typeList == ObjectEditor_TypeList.Misc)
            {
                objectDatabase.Data.allItemMiscs = objectDatabase.Data.allItemMiscs.OrderBy(z => z.ID).ToList();
                foreach (Item_Misc item in objectDatabase.Data.allItemMiscs)
                {
                    pooled_Objects.Add(item);
                }
            }

            if (typeList == ObjectEditor_TypeList.BaseWorldObject)
            {
                objectDatabase.Data.allBaseWorldObjects = objectDatabase.Data.allBaseWorldObjects.OrderBy(z => z.ID).ToList();
                foreach (WorldObject worldobject in objectDatabase.Data.allBaseWorldObjects)
                {
                    pooled_Objects.Add(worldobject);
                }
            }
        }

        private bool    filter_isCapsSensitive = false;
        private bool    filter_limitPage = true;
        private int     filter_itemPerPage = 50;
        private int     pageIndex = 0;
        private BaseObject objectTarget = null;

        void ShowItemsGUI()
        {
            filter_Objects.Clear();
            string titletext = "";
            string filter = "";

            titletext = typeList.ToString();
            titletext += " | Total: " + pooled_Objects.Count.ToString();

            GUILayout.Label(titletext, labelStyle);

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Filter: ", GUILayout.MaxWidth(50));
                wordFilter = GUILayout.TextField(wordFilter, GUILayout.MaxWidth(100));
                filter = wordFilter;
            }
            EditorGUILayout.EndHorizontal();

            filter_isCapsSensitive = GUILayout.Toggle(filter_isCapsSensitive, "Is Caps Sensitive");

            if (filter != "" | filter != null) {

                if (filter_isCapsSensitive)
                {
                    filter_Objects = pooled_Objects.FindAll(x => x.ID.Contains(filter));
                }
                else
                {
                    filter_Objects = pooled_Objects.FindAll(x => x.ID.ToLower().Contains(filter.ToLower()));
                }
            }
            else
            {
                filter_Objects = pooled_Objects;
            }

            EditorGUILayout.BeginHorizontal();
            {
                filter_limitPage = GUILayout.Toggle(filter_limitPage, "Limit item per page");

                if (filter_limitPage)
                {
                    filter_itemPerPage = EditorGUILayout.IntField(filter_itemPerPage, GUILayout.MaxWidth(40));
                    if (filter_itemPerPage == 0)
                    {
                        filter_itemPerPage = 1;
                    }

                    int maxPage = Mathf.FloorToInt((filter_Objects.Count - 1) / filter_itemPerPage) + 1;

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label("Page: ");

                    pageIndex = EditorGUILayout.IntField(pageIndex, GUILayout.MaxWidth(40));
                    GUILayout.Label( "/" + maxPage.ToString() + ")");

                    bool pageIndex_decrease = false;
                    bool pageIndex_increase= false;

                    if (pageIndex > 1)
                    {
                        pageIndex_decrease = true;
                    }

                    if (pageIndex < maxPage)
                    {
                        pageIndex_increase = true;
                    }

                    EditorGUI.BeginDisabledGroup(pageIndex_decrease == false);
                    if (GUILayout.Button("<", GUILayout.Width(21)))
                    {
                        pageIndex--;
                    }
                    EditorGUI.EndDisabledGroup();

                    EditorGUI.BeginDisabledGroup(pageIndex_increase == false);
                    if (GUILayout.Button(">", GUILayout.Width(21)))
                    {
                        pageIndex++;
                    }
                    EditorGUI.EndDisabledGroup();

                    if (pageIndex <= 0)
                    {
                        pageIndex = 1;
                    }
                    if (pageIndex > maxPage)
                    {
                        pageIndex = maxPage;
                    }

                    List<BaseObject> filter2 = new List<BaseObject>();

                    //PageIndex Start filter_Objects:
                    for (int x = 0; x < filter_itemPerPage; x++)
                    {
                        int index = x + ((pageIndex - 1) * filter_itemPerPage);

                        if (index >= filter_Objects.Count)
                        {
                            break;
                        }
                        else
                        {
                            filter2.Add(filter_Objects[index]);
                        }
                    }
                    filter_Objects = filter2;

                }
                else
                {
                    pageIndex = 0;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            string filtertext = "Filtered: " + filter_Objects.Count.ToString();
            GUILayout.Label(filtertext, labelStyle);

            for (int x = 0; x < filter_Objects.Count; x++)
            {
                if (GUILayout.Button(filter_Objects[x].ID, buttonStyle))
                {
                    objectTarget = filter_Objects[x];
                    GenericMenu menu = new GenericMenu();

                    menu.AddItem(new GUIContent("Copy ID to Clipboard"), false, Context_CopyClipboard);
                    menu.AddItem(new GUIContent("New"), false, Context_New);
                    menu.AddItem(new GUIContent("Edit"), false, Context_Edit);
                    menu.AddItem(new GUIContent("Duplicate"), false, Context_Duplicate);
                    menu.AddItem(new GUIContent("Delete "), false, Context_Delete);
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Create object in scene "), false, Context_CreateObjectInScene);

                    menu.ShowAsContext();
                }
            }
        }

        void Context_New()
        {
            switch (typeList)
            {
                case ObjectEditor_TypeList.Ammo:
                    {
                        Item_Ammo newItem1 = new Item_Ammo();
                        objectTarget = newItem1;
                        objectDatabase.Data.allItemAmmo.Add(newItem1);

                        break;
                    }

                case ObjectEditor_TypeList.Junk:
                    {
                        Item_Junk newItem1 = new Item_Junk();
                        objectTarget = newItem1;
                        objectDatabase.Data.allItemJunk.Add(newItem1);

                        break;
                    }
                case ObjectEditor_TypeList.Key:
                    {
                        Item_Key newItem1 = new Item_Key();
                        objectTarget = newItem1;
                        objectDatabase.Data.allItemKey.Add(newItem1);

                        break;
                    }
                case ObjectEditor_TypeList.Weapon:
                    {
                        Item_Weapon newItem1 = new Item_Weapon();
                        objectTarget = newItem1;
                        objectDatabase.Data.allItemWeapon.Add(newItem1);

                        break;
                    }
                case ObjectEditor_TypeList.Misc:
                    {
                        Item_Misc newItem1 = new Item_Misc();
                        objectTarget = newItem1;
                        objectDatabase.Data.allItemMiscs.Add(newItem1);

                        break;
                    }
                case ObjectEditor_TypeList.BaseWorldObject:
                    {
                        WorldObject newObject = new WorldObject();
                        objectTarget = newObject;
                        objectDatabase.Data.allBaseWorldObjects.Add(newObject);

                        break;
                    }
                default:

                    break;
            }

            Change_List();
            DestinyObjectEditor.OpenWindow(objectDatabase, typeList, objectTarget, true);
        }


        void Context_Edit()
        {
            DestinyObjectEditor.OpenWindow(objectDatabase, typeList, objectTarget);
        }

        void Context_CopyClipboard()
        {
            EditorGUIUtility.systemCopyBuffer = objectTarget.ID;
        }

        void Context_Duplicate()
        {
            if (EditorUtility.DisplayDialog("Duplicate Object", "Are you sure you want to duplicate '" + objectTarget.ID + "'?",
                "Yes", "No"))
            {
                switch (typeList)
                {
                    case ObjectEditor_TypeList.Ammo:
                        {
                            Item_Ammo newItem1 = Item_Ammo.Copy(objectTarget as Item_Ammo);
                            objectDatabase.Data.allItemAmmo.Add(newItem1);

                            break;
                        }
                    case ObjectEditor_TypeList.Junk:
                        {
                            Item_Junk newItem2 = Item_Junk.Copy(objectTarget as Item_Junk);
                            objectDatabase.Data.allItemJunk.Add(newItem2);

                            break;
                        }
                    case ObjectEditor_TypeList.Key:
                        {
                            Item_Key newItem3 = Item_Key.Copy(objectTarget as Item_Key);
                            objectDatabase.Data.allItemKey.Add(newItem3);

                            break;
                        }
                    case ObjectEditor_TypeList.Weapon:
                        {
                            Item_Weapon newItem4 = Item_Weapon.Copy(objectTarget as Item_Weapon);
                            objectDatabase.Data.allItemWeapon.Add(newItem4);

                            break;
                        }
                    case ObjectEditor_TypeList.Misc:
                        {
                            Item_Misc newItem5 = Item_Misc.Copy(objectTarget as Item_Misc);
                            objectDatabase.Data.allItemMiscs.Add(newItem5);

                            break;
                        }
                    case ObjectEditor_TypeList.BaseWorldObject:
                        {
                            WorldObject newItem4 = WorldObject.Copy(objectTarget as WorldObject);
                            objectDatabase.Data.allBaseWorldObjects.Add(newItem4);

                            break;
                        }
                    default:

                        break;
                }

                Change_List();
            }
        }

        void Context_Delete()
        {
            if ( EditorUtility.DisplayDialog("Delete Object", "Are you sure you want to delete ' " + objectTarget.ID + " ' ?",
                "Yes", "No"))
            {
                switch (typeList)
                {
                    case ObjectEditor_TypeList.Ammo:
                        objectDatabase.Data.allItemAmmo.Remove(objectTarget as Item_Ammo);
                        break;

                    case ObjectEditor_TypeList.Junk:
                        objectDatabase.Data.allItemJunk.Remove(objectTarget as Item_Junk);
                        break;

                    case ObjectEditor_TypeList.Key:
                        objectDatabase.Data.allItemKey.Remove(objectTarget as Item_Key);

                        break;

                    case ObjectEditor_TypeList.Weapon:
                        objectDatabase.Data.allItemWeapon.Remove(objectTarget as Item_Weapon);

                        break;

                    case ObjectEditor_TypeList.Misc:
                        objectDatabase.Data.allItemMiscs.Remove(objectTarget as Item_Misc);

                        break;

                    case ObjectEditor_TypeList.BaseWorldObject:
                        objectDatabase.Data.allBaseWorldObjects.Remove(objectTarget as WorldObject);

                        break;

                    default:

                        break;
                }

                Change_List();
            }
        }

        void Context_CreateObjectInScene()
        {
            if (objectTarget.gameModel != null)
            {
                if (objectTarget is Item)
                {
                    GameObject createdObject = Instantiate(objectTarget.gameModel);
                    Pickable pickable = createdObject.GetComponent<Pickable>();

                    if (pickable == null)
                    {
                        if (createdObject.GetComponent<Spawnables>() != null)
                        {
                            Spawnables spawnable = createdObject.GetComponent<Spawnables>();
                            DestroyImmediate(spawnable);
                        }

                        pickable = objectTarget.gameModel.AddComponent<Pickable>();
                        pickable.pickableData.formID.BaseID = objectTarget.ID;
                        pickable.pickableData.formID.DatabaseID = objectDatabase.Data.name;
                        pickable.pickableData.formID.ObjectType = DestinyMainUtility.Check_ObjectType(objectTarget);
                        pickable.pickableData.formID.ReferenceID = objectTarget.ID + "_" + DestinyMainUtility.GenerateStupidID(6);

                        pickable.pickableData.itemData.DatabaseName = objectDatabase.Data.name;
                        pickable.pickableData.itemData.ID = objectTarget.ID;
                        pickable.pickableData.itemData.item_Type = DestinyMainUtility.Check_ItemType(objectTarget as Item);
                    }
                    else
                    {
                        pickable.pickableData.formID.BaseID = objectTarget.ID;
                        pickable.pickableData.formID.DatabaseID = objectDatabase.Data.name;
                        pickable.pickableData.formID.ObjectType = DestinyMainUtility.Check_ObjectType(objectTarget);
                        pickable.pickableData.formID.ReferenceID = objectTarget.ID + "_" + DestinyMainUtility.GenerateStupidID(6);

                        pickable.pickableData.itemData.DatabaseName = objectDatabase.Data.name;
                        pickable.pickableData.itemData.ID = objectTarget.ID;
                        pickable.pickableData.itemData.item_Type = DestinyMainUtility.Check_ItemType(objectTarget as Item);
                    }
                }
                else
                {
                    Spawnables spawnable = Instantiate(objectTarget.gameModel).GetComponent<Spawnables>();

                    if (spawnable == null)
                    {
                        spawnable = objectTarget.gameModel.AddComponent<Spawnables>();
                        spawnable.Data.formID.BaseID = objectTarget.ID;
                        spawnable.Data.formID.DatabaseID = objectDatabase.Data.name;
                        spawnable.Data.formID.ObjectType = DestinyMainUtility.Check_ObjectType(objectTarget);
                        spawnable.Data.formID.ReferenceID = objectTarget.ID + "_" + DestinyMainUtility.GenerateStupidID(6);
                    }
                    else
                    {
                        spawnable.Data.formID.BaseID = objectTarget.ID;
                        spawnable.Data.formID.DatabaseID = objectDatabase.Data.name;
                        spawnable.Data.formID.ObjectType = DestinyMainUtility.Check_ObjectType(objectTarget);
                        spawnable.Data.formID.ReferenceID = objectTarget.ID + "_" + DestinyMainUtility.GenerateStupidID(6);

                    }
                }
            }
            else
            {
                Debug.LogError("No gameModel assigned to " + objectTarget.name + "!");
            }
        }



        void Test1()
        {

        }

        #endregion
        void OnInspectorUpdate()
        {
            Repaint();
        }

        protected void OnEnable()
        {
            skin = (GUISkin)Resources.Load("DatabaseSkin");
            buttonStyle = skin.GetStyle("Button");
            labelStyle = skin.GetStyle("Label");
            // Here we retrieve the data if it exists or we save the default field initialisers we set above
            var data = EditorPrefs.GetString("ObjectDatabaseEditor", JsonUtility.ToJson(this, false));
            // Then we apply them to this window
            JsonUtility.FromJsonOverwrite(data, this);
        }

        protected void OnDisable()
        {
            // We get the Json data
            var data = JsonUtility.ToJson(this, false);
            // And we save it
            EditorPrefs.SetString("ObjectDatabaseEditor", data);
        }

    }
}