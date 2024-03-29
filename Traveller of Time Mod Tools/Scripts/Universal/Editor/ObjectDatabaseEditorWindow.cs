﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using DestinyEngine;
using System.IO;

namespace DestinyEngine.Object
{
    public enum ObjectEditor_TypeList
    {
        None,
        Ammo,
        Junk,
        Key,
        Weapon,
        Armor,
        Consume,
        Misc,
        Music,
        BaseWorldObject,
        CampObject,
        Actors,
        Quests,
        Crafting,
        VehiclePart,
        Animation,
        SmartphoneApp,
        InteriorMat,
        WindowDoor,
        GridObject,
        Faction,
        Trivia,
        NaturePopulatorObject,
        TemplateAilment
    }

    public class ObjectDatabaseEditorWindow : EditorWindow
    {


        public ObjectDatabase objectDatabase;
        private List<BaseObject>    pooled_Objects = new List<BaseObject>();
        private List<BaseObject>    filter_Objects = new List<BaseObject>();
        private string          wordFilter = "";

        public static string PATH_MODDINGHELPER_DATABASE_PRINT = Application.streamingAssetsPath + "/Modding Help/";
        public static string PATH_DEFAULT_DATABASE = Application.streamingAssetsPath + "/Modding Help/";

        private GUISkin skin;
        private GUIStyle buttonStyle;
        private GUIStyle labelStyle;
        private Color almostwhite = new Color(1, 1, 1, 0.2f);
        private ObjectEditor_TypeList typeList = ObjectEditor_TypeList.None;

        bool foldoutShow_WorldObject;
        bool foldoutShow_Item;
        bool foldoutShow_Faction;
        bool foldoutShow_Vehicle;
        bool foldoutShow_Buildings;
        bool foldoutShow_Miscellanous;

        Vector2 scrollPos_ItemCategory;
        Vector2 scrollPos_IDObject;

        private bool filter_isCapsSensitive = false;
        private bool filter_limitPage = true;
        private int filter_itemPerPage = 50;
        private int pageIndex = 0;
        private BaseObject objectTarget = null;

        [MenuItem("Destiny Engine/Object Database Editor Window")]
        public static void OpenWindow(ObjectDatabase objectDatabase)
        {
            ObjectDatabaseEditorWindow window = GetWindow<ObjectDatabaseEditorWindow>(false, "Object Database Editor");
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

            EditorGUILayout.BeginHorizontal();
            objectDatabase = (ObjectDatabase) EditorGUILayout.ObjectField(objectDatabase, typeof(ObjectDatabase), false, GUILayout.MaxWidth(200));

            if (GUILayout.Button("Print Database", buttonStyle, GUILayout.Width(200)))
            {
                PrintDatabase();
            }

            if (GUILayout.Button("Backup!", buttonStyle, GUILayout.Width(200)))
            {
                Backup();
            }
            EditorGUILayout.EndHorizontal();


            if (GUILayout.Button(PATH_MODDINGHELPER_DATABASE_PRINT, buttonStyle, GUILayout.Width(500)))
            {
                string path = EditorUtility.OpenFolderPanel("Folder path for database", PATH_DEFAULT_DATABASE, "");

                if (path.Length != 0)
                {
                    PATH_MODDINGHELPER_DATABASE_PRINT = path;
                }
            }

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
                            if (GUILayout.Button("Armor", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.Armor;
                                Change_List();
                            }
                            if (GUILayout.Button("Consumables", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.Consume;
                                Change_List();
                            }
                            if (GUILayout.Button("Misc", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.Misc;
                                Change_List();
                            }
                        }
                        EditorGUILayout.EndFoldoutHeaderGroup();



                        foldoutShow_WorldObject = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutShow_WorldObject, "WorldObjects");
                        if (foldoutShow_WorldObject)
                        {
                            if (GUILayout.Button("Base WorldObject", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.BaseWorldObject;
                                Change_List();
                            }
                            if (GUILayout.Button("Actors", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.Actors;
                                Change_List();
                            }
                            if (GUILayout.Button("Camp Object", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.CampObject;
                                Change_List();
                            }
                        }
                        EditorGUILayout.EndFoldoutHeaderGroup();



                        foldoutShow_Faction = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutShow_Faction, "Characters");
                        if (foldoutShow_Faction)
                        {
                            if (GUILayout.Button("Quests", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.Quests;
                                Change_List();
                            }
                            if (GUILayout.Button("Factions", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.Faction;
                                Change_List();
                            }
                            if (GUILayout.Button("Ailments", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.TemplateAilment;
                                Change_List();
                            }
                        }
                        EditorGUILayout.EndFoldoutHeaderGroup();



                        foldoutShow_Vehicle = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutShow_Vehicle, "Vehicles");
                        if (foldoutShow_Vehicle)
                        {
                            if (GUILayout.Button("Vehicle Part", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.VehiclePart;
                                Change_List();
                            }
                        }
                        EditorGUILayout.EndFoldoutHeaderGroup();



                        foldoutShow_Buildings = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutShow_Buildings, "Buildings");
                        if (foldoutShow_Buildings)
                        {
                            if (GUILayout.Button("Interior Materials", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.InteriorMat;
                                Change_List();
                            }
                            if (GUILayout.Button("Window & Doors", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.WindowDoor;
                                Change_List();
                            }
                            if (GUILayout.Button("Grid Objects", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.GridObject;
                                Change_List();
                            }
                        }
                        EditorGUILayout.EndFoldoutHeaderGroup();


                        foldoutShow_Miscellanous = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutShow_Miscellanous, "Miscellanous");
                        if (foldoutShow_Miscellanous)
                        {
                            if (GUILayout.Button("Musics", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.Music;
                                Change_List();
                            }
                            if (GUILayout.Button("Crafting Workbench", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.Crafting;
                                Change_List();
                            }
                            if (GUILayout.Button("Animation", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.Animation;
                                Change_List();
                            }
                            if (GUILayout.Button("Smartphone Apps", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.SmartphoneApp;
                                Change_List();
                            }
                            if (GUILayout.Button("Trivias", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.Trivia;
                                Change_List();
                            }
                            if (GUILayout.Button("Nature Object", buttonStyle))
                            {
                                typeList = ObjectEditor_TypeList.NaturePopulatorObject;
                                Change_List();
                            }
                        }
                        EditorGUILayout.EndFoldoutHeaderGroup();
                    }
                    EditorGUILayout.EndScrollView();

                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("Box", GUILayout.Width(440), GUILayout.ExpandHeight(true));
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

        private void PrintDatabase()
        {
            string s = objectDatabase.Data.name;
            var path = PATH_MODDINGHELPER_DATABASE_PRINT + $"{s}.txt";
            EditorUtility.RevealInFinder(path);

            var allObjectDatabase = objectDatabase.GetAllBaseObjectsFromDatabase();
            s += System.Environment.NewLine;
            s += System.Environment.NewLine;

            foreach (var baseObj in allObjectDatabase)
            {
                var name = baseObj.ID;
                var typeObj = MainUtility.Check_ObjectType(baseObj);

                s += name.ToString();
                s += " | " + typeObj.ToString();
                s += System.Environment.NewLine;

            }

            StreamWriter writer = new StreamWriter(path, false);
            writer.WriteLine(s);
            writer.Close();

        }

        private void Apply_PrefabChanges()
        {
            var ListObjects = objectDatabase.GetAllBaseObjectsFromDatabase();
            List<GameObject> listGameObjectDuplicatesCheck = new List<GameObject>();

            foreach(var baseObject in ListObjects)
            {
                if (baseObject.gameModel == null)
                {
                    continue;
                }

                if (listGameObjectDuplicatesCheck.Find(x => x == baseObject.gameModel) != null)
                {
                    baseObject.gameModel = Create_BrandNewPrefab(baseObject);
                }

                listGameObjectDuplicatesCheck.Add(baseObject.gameModel);

                if (baseObject is BaseWorldObject)
                {
                    WorldObjectScript worldObject = baseObject.gameModel.GetComponent<WorldObjectScript>();

                    if (worldObject == null)
                    {
                        worldObject = baseObject.gameModel.AddComponent<WorldObjectScript>();
                        ObjectReference_Data data = new ObjectReference_Data();
                        data.formID.BaseID = baseObject.ID;
                        data.formID.DatabaseID = objectDatabase.Data.name;
                        data.formID.ObjectType = MainUtility.Check_ObjectType(baseObject);
                        worldObject.Data = data;
                    }
                    else
                    {
                        ObjectReference_Data data = worldObject.Data;
                        data.formID.BaseID = baseObject.ID;
                        data.formID.DatabaseID = objectDatabase.Data.name;
                        data.formID.ObjectType = MainUtility.Check_ObjectType(baseObject);
                        worldObject.Data = data;
                    }
                }

                if (baseObject is Item)
                {
                    PickableScript pickable = baseObject.gameModel.GetComponent<PickableScript>();

                    if (pickable == null)
                    {
                        pickable = baseObject.gameModel.AddComponent<PickableScript>();
                        Pickable_Data pickableData = new Pickable_Data();
                        ItemData itemData = new ItemData();
                        pickableData.formID.BaseID = baseObject.ID;
                        pickableData.formID.DatabaseID = objectDatabase.Data.name;
                        pickableData.formID.ObjectType = MainUtility.Check_ObjectType(baseObject);
                        pickableData.itemData.DatabaseName = objectDatabase.Data.name;
                        pickableData.itemData.ID = baseObject.ID;
                        pickableData.itemData.item_Type = MainUtility.Check_ItemType(baseObject as Item);

                        pickableData.itemData = itemData;
                        pickable.pickableData = pickableData;
                    }
                    else
                    {
                        Pickable_Data pickableData = pickable.pickableData;
                        pickableData.formID.BaseID = baseObject.ID;
                        pickableData.formID.DatabaseID = objectDatabase.Data.name;
                        pickableData.formID.ObjectType = MainUtility.Check_ObjectType(baseObject);
                        pickableData.itemData.DatabaseName = objectDatabase.Data.name;
                        pickableData.itemData.ID = baseObject.ID;
                        pickableData.itemData.item_Type = MainUtility.Check_ItemType(baseObject as Item);

                        pickable.pickableData = pickableData;
                    }
                }

                if (baseObject is Actor)
                {
                    ActorScript actorScript = baseObject.gameModel.GetComponent<ActorScript>();

                    if (actorScript == null)
                    {
                        actorScript = baseObject.gameModel.AddComponent<ActorScript>();
                        Actor_Data actor_Data = new Actor_Data();
                        actor_Data.formID.BaseID = baseObject.ID;
                        actor_Data.formID.DatabaseID = objectDatabase.Data.name;
                        actor_Data.formID.ObjectType = MainUtility.Check_ObjectType(baseObject);

                        actorScript.actorData = actor_Data;
                    }
                    else
                    {
                        Actor_Data actor_Data = actorScript.actorData;
                        actor_Data.formID.BaseID = baseObject.ID;
                        actor_Data.formID.DatabaseID = objectDatabase.Data.name;
                        actor_Data.formID.ObjectType = MainUtility.Check_ObjectType(baseObject);

                        actorScript.actorData = actor_Data;
                    }
                }

                if (baseObject is CampObject)
                {
                    WorldObjectScript worldObject = baseObject.gameModel.GetComponent<WorldObjectScript>();

                    if (worldObject == null)
                    {
                        worldObject = baseObject.gameModel.AddComponent<WorldObjectScript>();
                        ObjectReference_Data data = new ObjectReference_Data();
                        data.formID.BaseID = baseObject.ID;
                        data.formID.DatabaseID = objectDatabase.Data.name;
                        data.formID.ObjectType = MainUtility.Check_ObjectType(baseObject);
                        worldObject.Data = data;
                    }
                    else
                    {
                        ObjectReference_Data data = worldObject.Data;
                        data.formID.BaseID = baseObject.ID;
                        data.formID.DatabaseID = objectDatabase.Data.name;
                        data.formID.ObjectType = MainUtility.Check_ObjectType(baseObject);
                        worldObject.Data = data;
                    }
                }

                //Goddamnit UNITY, NO WONDER IT'S KEEP GLITCHING
                PrefabUtility.RecordPrefabInstancePropertyModifications(baseObject.gameModel);

            }
        }

        private void Backup()
        {
            if (objectDatabase == null) return;
            try
            {
                var path = AssetDatabase.GetAssetPath(objectDatabase);
                if (path.EndsWith("(Auto-Backup).asset"))
                {
                    Debug.Log("Object Database Editor: Not creating an auto-backup. You're already editing the auto-backup file.");
                    return;
                }
                var backupPath = Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path) + " (Auto-Backup).asset";


                EditorUtility.DisplayProgressBar("Object Database  Editor Auto-Backup", "Creating auto-backup " + Path.GetFileNameWithoutExtension(backupPath), 0);
                AssetDatabase.DeleteAsset(backupPath);
                AssetDatabase.CopyAsset(path, backupPath);
                AssetDatabase.Refresh();
#if !(UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
                if (!string.IsNullOrEmpty(AssetImporter.GetAtPath(backupPath).assetBundleName))
                {
                    AssetImporter.GetAtPath(backupPath).assetBundleVariant = string.Empty;
                    AssetImporter.GetAtPath(backupPath).assetBundleName = string.Empty;
                }
#endif
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }

        }

        private GameObject Create_BrandNewPrefab(BaseObject baseObject)
        {
            var instanceRoot = (GameObject)PrefabUtility.InstantiatePrefab(baseObject.gameModel);

            string localPath = "Assets/Resources/" + baseObject.ID + ".prefab";
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(instanceRoot, localPath);
            DestroyImmediate(instanceRoot);
            return prefab;
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

            if (typeList == ObjectEditor_TypeList.Armor)
            {
                objectDatabase.Data.allItemArmors = objectDatabase.Data.allItemArmors.OrderBy(z => z.ID).ToList();
                foreach (Item_Armor item in objectDatabase.Data.allItemArmors)
                {
                    pooled_Objects.Add(item);
                }
            }

            if (typeList == ObjectEditor_TypeList.Consume)
            {
                objectDatabase.Data.allItemConsumables = objectDatabase.Data.allItemConsumables.OrderBy(z => z.ID).ToList();
                foreach (Item_Consumables item in objectDatabase.Data.allItemConsumables)
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
                foreach (BaseWorldObject worldobject in objectDatabase.Data.allBaseWorldObjects)
                {
                    pooled_Objects.Add(worldobject);
                }
            }

            if (typeList == ObjectEditor_TypeList.CampObject)
            {
                objectDatabase.Data.allCampObjects = objectDatabase.Data.allCampObjects.OrderBy(z => z.ID).ToList();
                foreach (CampObject campobject in objectDatabase.Data.allCampObjects)
                {
                    pooled_Objects.Add(campobject);
                }
            }

            if (typeList == ObjectEditor_TypeList.Actors)
            {
                objectDatabase.Data.allBaseActors = objectDatabase.Data.allBaseActors.OrderBy(z => z.ID).ToList();
                foreach (Actor actor in objectDatabase.Data.allBaseActors)
                {
                    pooled_Objects.Add(actor);
                }
            }

            if (typeList == ObjectEditor_TypeList.Quests)
            {
                objectDatabase.Data.allBaseQuests = objectDatabase.Data.allBaseQuests.OrderBy(z => z.ID).ToList();
                foreach (Quest quest in objectDatabase.Data.allBaseQuests)
                {
                    pooled_Objects.Add(quest);
                }
            }


            if (typeList == ObjectEditor_TypeList.Music)
            {
                objectDatabase.Data.allMusics = objectDatabase.Data.allMusics.OrderBy(z => z.ID).ToList();
                foreach (Music music in objectDatabase.Data.allMusics)
                {
                    pooled_Objects.Add(music);
                }
            }

            if (typeList == ObjectEditor_TypeList.Crafting)
            {
                objectDatabase.Data.allCraftingBench = objectDatabase.Data.allCraftingBench.OrderBy(z => z.ID).ToList();
                foreach (CraftingWorkbench workbench in objectDatabase.Data.allCraftingBench)
                {
                    pooled_Objects.Add(workbench);
                }
            }

            if (typeList == ObjectEditor_TypeList.Animation)
            {
                objectDatabase.Data.allAnimations = objectDatabase.Data.allAnimations.OrderBy(z => z.ID).ToList();
                foreach (BaseAnimation animation in objectDatabase.Data.allAnimations)
                {
                    pooled_Objects.Add(animation);
                }
            }


            if (typeList == ObjectEditor_TypeList.VehiclePart)
            {
                objectDatabase.Data.allVehicleParts = objectDatabase.Data.allVehicleParts.OrderBy(z => z.ID).ToList();
                foreach (VehiclePart vehiclePart in objectDatabase.Data.allVehicleParts)
                {
                    pooled_Objects.Add(vehiclePart);
                }
            }


            if (typeList == ObjectEditor_TypeList.SmartphoneApp)
            {
                objectDatabase.Data.allSmartphoneApps = objectDatabase.Data.allSmartphoneApps.OrderBy(z => z.ID).ToList();
                foreach (SmartphoneApp app in objectDatabase.Data.allSmartphoneApps)
                {
                    pooled_Objects.Add(app);
                }
            }

            if (typeList == ObjectEditor_TypeList.Trivia)
            {
                objectDatabase.Data.allTrivias = objectDatabase.Data.allTrivias.OrderBy(z => z.ID).ToList();
                foreach (Trivia trivia in objectDatabase.Data.allTrivias)
                {
                    pooled_Objects.Add(trivia);
                }
            }

            if (typeList == ObjectEditor_TypeList.TemplateAilment)
            {
                objectDatabase.Data.allTemplateAilments = objectDatabase.Data.allTemplateAilments.OrderBy(z => z.ID).ToList();
                foreach (TemplateAilment ailment in objectDatabase.Data.allTemplateAilments)
                {
                    pooled_Objects.Add(ailment);
                }
            }

            if (typeList == ObjectEditor_TypeList.NaturePopulatorObject)
            {
                objectDatabase.Data.allNatureObjects = objectDatabase.Data.allNatureObjects.OrderBy(z => z.ID).ToList();
                foreach (NaturalPopulatorObject natureObject in objectDatabase.Data.allNatureObjects)
                {
                    pooled_Objects.Add(natureObject);
                }
            }

            if (typeList == ObjectEditor_TypeList.InteriorMat)
            {
                objectDatabase.Data.allInteriorMaterials = objectDatabase.Data.allInteriorMaterials.OrderBy(z => z.ID).ToList();
                foreach (InteriorMaterial interiorMat in objectDatabase.Data.allInteriorMaterials)
                {
                    pooled_Objects.Add(interiorMat);
                }
            }

            if (typeList == ObjectEditor_TypeList.WindowDoor)
            {
                objectDatabase.Data.allWindowDoorObject = objectDatabase.Data.allWindowDoorObject.OrderBy(z => z.ID).ToList();
                foreach (var windowDoor in objectDatabase.Data.allWindowDoorObject)
                {
                    pooled_Objects.Add(windowDoor);
                }
            }

            if (typeList == ObjectEditor_TypeList.GridObject)
            {
                objectDatabase.Data.allGridObjects = objectDatabase.Data.allGridObjects.OrderBy(z => z.ID).ToList();
                foreach (var gridObject in objectDatabase.Data.allGridObjects)
                {
                    pooled_Objects.Add(gridObject);
                }
            }

            if (typeList == ObjectEditor_TypeList.Faction)
            {
                objectDatabase.Data.allFactions = objectDatabase.Data.allFactions.OrderBy(z => z.ID).ToList();
                foreach (var faction in objectDatabase.Data.allFactions)
                {
                    pooled_Objects.Add(faction);
                }
            }
        }


        private void DisplayDialog_New()
        {
            if (EditorUtility.DisplayDialog($"New {typeList}",
                $"Are you sure you want to create new object for {typeList}?", "Create", "Cancel")) 
            {
                Context_New();
            }
        }

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

                if (GUILayout.Button("+", buttonStyle, GUILayout.Width(60)))
                {
                    DisplayDialog_New();
                }
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
                if (GUILayout.Button($"{filter_Objects[x].ID} ({filter_Objects[x].name})", buttonStyle))
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
                case ObjectEditor_TypeList.Armor:
                    {
                        Item_Armor newItem1 = new Item_Armor();
                        objectTarget = newItem1;
                        objectDatabase.Data.allItemArmors.Add(newItem1);

                        break;
                    }
                case ObjectEditor_TypeList.Consume:
                    {
                        Item_Consumables newItem1 = new Item_Consumables();
                        objectTarget = newItem1;
                        objectDatabase.Data.allItemConsumables.Add(newItem1);

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
                        BaseWorldObject newObject = new BaseWorldObject();
                        objectTarget = newObject;
                        objectDatabase.Data.allBaseWorldObjects.Add(newObject);

                        break;
                    }
                case ObjectEditor_TypeList.CampObject:
                    {
                        CampObject newCamp = new CampObject();
                        objectTarget = newCamp;
                        objectDatabase.Data.allCampObjects.Add(newCamp);

                        break;
                    }
                case ObjectEditor_TypeList.Actors:
                    {
                        Actor newActor = new Actor();
                        objectTarget = newActor;
                        objectDatabase.Data.allBaseActors.Add(newActor);

                        break;
                    }
                case ObjectEditor_TypeList.Quests:
                    {
                        Quest quest = new Quest();
                        objectTarget = quest;
                        objectDatabase.Data.allBaseQuests.Add(quest);

                        break;
                    }
                case ObjectEditor_TypeList.Music:
                    {
                        Music music = new Music();
                        objectTarget = music;
                        objectDatabase.Data.allMusics.Add(music);

                        break;
                    }
                case ObjectEditor_TypeList.Crafting:
                    {
                        CraftingWorkbench crafting = new CraftingWorkbench();
                        objectTarget = crafting;
                        objectDatabase.Data.allCraftingBench.Add(crafting);

                        break;
                    }
                case ObjectEditor_TypeList.Animation:
                    {
                        BaseAnimation animation = new BaseAnimation();
                        objectTarget = animation;
                        objectDatabase.Data.allAnimations.Add(animation);

                        break;
                    }
                case ObjectEditor_TypeList.VehiclePart:
                    {
                        VehiclePart vehiclePart = new VehiclePart();
                        objectTarget = vehiclePart;
                        objectDatabase.Data.allVehicleParts.Add(vehiclePart);

                        break;
                    }
                case ObjectEditor_TypeList.SmartphoneApp:
                    {
                        SmartphoneApp application = new SmartphoneApp();
                        objectTarget = application;
                        objectDatabase.Data.allSmartphoneApps.Add(application);

                        break;
                    }
                case ObjectEditor_TypeList.Trivia:
                    {
                        Trivia trivia = new Trivia();
                        objectTarget = trivia;
                        objectDatabase.Data.allTrivias.Add(trivia);

                        break;
                    }
                case ObjectEditor_TypeList.TemplateAilment:
                    {
                        TemplateAilment templateAilment = new TemplateAilment();
                        objectTarget = templateAilment;
                        objectDatabase.Data.allTemplateAilments.Add(templateAilment);

                        break;
                    }
                case ObjectEditor_TypeList.NaturePopulatorObject:
                    {
                        NaturalPopulatorObject natureObj = new NaturalPopulatorObject();
                        objectTarget = natureObj;
                        objectDatabase.Data.allNatureObjects.Add(natureObj);

                        break;
                    }
                case ObjectEditor_TypeList.InteriorMat:
                    {
                        InteriorMaterial interiorMat = new InteriorMaterial();
                        objectTarget = interiorMat;
                        objectDatabase.Data.allInteriorMaterials.Add(interiorMat);

                        break;
                    }
                case ObjectEditor_TypeList.WindowDoor:
                    {
                        WindowDoorObject doorObject = new WindowDoorObject();
                        objectTarget = doorObject;
                        objectDatabase.Data.allWindowDoorObject.Add(doorObject);

                        break;
                    }
                case ObjectEditor_TypeList.GridObject:
                    {
                        GridObject GridObject = new GridObject();
                        objectTarget = GridObject;
                        objectDatabase.Data.allGridObjects.Add(GridObject);

                        break;
                    }
                case ObjectEditor_TypeList.Faction:
                    {
                        Faction faction = new Faction();
                        objectTarget = faction;
                        objectDatabase.Data.allFactions.Add(faction);

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
                    case ObjectEditor_TypeList.Armor:
                        {
                            Item_Armor newItem4 = Item_Armor.Copy(objectTarget as Item_Armor);
                            objectDatabase.Data.allItemArmors.Add(newItem4);

                            break;
                        }
                    case ObjectEditor_TypeList.Consume:
                        {
                            Item_Consumables newItem4 = Item_Consumables.Copy(objectTarget as Item_Consumables);
                            objectDatabase.Data.allItemConsumables.Add(newItem4);

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
                            BaseWorldObject newItem4 = BaseWorldObject.Copy(objectTarget as BaseWorldObject);
                            objectDatabase.Data.allBaseWorldObjects.Add(newItem4);

                            break;
                        }
                    case ObjectEditor_TypeList.CampObject:
                        {
                            CampObject campObject = CampObject.Copy(objectTarget as CampObject);
                            objectDatabase.Data.allCampObjects.Add(campObject);

                            break;
                        }
                    case ObjectEditor_TypeList.Actors:
                        {
                            Actor actor = Actor.Copy(objectTarget as Actor);
                            objectDatabase.Data.allBaseActors.Add(actor);

                            break;
                        }
                    case ObjectEditor_TypeList.Quests:
                        {
                            Quest quest = Quest.Copy(objectTarget as Quest);
                            objectDatabase.Data.allBaseQuests.Add(quest);

                            break;
                        }
                    case ObjectEditor_TypeList.Music:
                        {
                            Music music = Music.Copy(objectTarget as Music);
                            objectDatabase.Data.allMusics.Add(music);

                            break;
                        }
                    case ObjectEditor_TypeList.Crafting:
                        {
                            CraftingWorkbench craftBench = CraftingWorkbench.Copy(objectTarget as CraftingWorkbench);
                            objectDatabase.Data.allCraftingBench.Add(craftBench);

                            break;
                        }
                    case ObjectEditor_TypeList.Animation:
                        {
                            BaseAnimation animation = BaseAnimation.Copy(objectTarget as BaseAnimation);
                            objectDatabase.Data.allAnimations.Add(animation);

                            break;
                        }
                    case ObjectEditor_TypeList.VehiclePart:
                        {
                            VehiclePart vehicle = VehiclePart.Copy(objectTarget as VehiclePart);
                            objectDatabase.Data.allVehicleParts.Add(vehicle);

                            break;
                        }
                    case ObjectEditor_TypeList.SmartphoneApp:
                        {
                            SmartphoneApp app = SmartphoneApp.Copy(objectTarget as SmartphoneApp);
                            objectDatabase.Data.allSmartphoneApps.Add(app);

                            break;
                        }
                    case ObjectEditor_TypeList.Trivia:
                        {
                            Trivia trivia = Trivia.Copy(objectTarget as Trivia);
                            objectDatabase.Data.allTrivias.Add(trivia);

                            break;
                        }
                    case ObjectEditor_TypeList.TemplateAilment:
                        {
                            TemplateAilment trivia = TemplateAilment.Copy(objectTarget as TemplateAilment);
                            objectDatabase.Data.allTemplateAilments.Add(trivia);

                            break;
                        }
                    case ObjectEditor_TypeList.NaturePopulatorObject:
                        {
                            NaturalPopulatorObject natureObj = NaturalPopulatorObject.Copy(objectTarget as NaturalPopulatorObject);
                            objectDatabase.Data.allNatureObjects.Add(natureObj);

                            break;
                        }
                    case ObjectEditor_TypeList.InteriorMat:
                        {
                            InteriorMaterial mat = InteriorMaterial.Copy(objectTarget as InteriorMaterial);
                            objectDatabase.Data.allInteriorMaterials.Add(mat);

                            break;
                        }
                    case ObjectEditor_TypeList.WindowDoor:
                        {
                            WindowDoorObject door = WindowDoorObject.Copy(objectTarget as WindowDoorObject);
                            objectDatabase.Data.allWindowDoorObject.Add(door);

                            break;
                        }
                    case ObjectEditor_TypeList.GridObject:
                        {
                            GridObject gridobj = GridObject.Copy(objectTarget as GridObject);
                            objectDatabase.Data.allGridObjects.Add(gridobj);

                            break;
                        }
                    case ObjectEditor_TypeList.Faction:
                        {
                            Faction faction = Faction.Copy(objectTarget as Faction);
                            objectDatabase.Data.allFactions.Add(faction);

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

                    case ObjectEditor_TypeList.Armor:
                        objectDatabase.Data.allItemArmors.Remove(objectTarget as Item_Armor);

                        break;

                    case ObjectEditor_TypeList.Consume:
                        objectDatabase.Data.allItemConsumables.Remove(objectTarget as Item_Consumables);

                        break;

                    case ObjectEditor_TypeList.Misc:
                        objectDatabase.Data.allItemMiscs.Remove(objectTarget as Item_Misc);

                        break;

                    case ObjectEditor_TypeList.BaseWorldObject:
                        objectDatabase.Data.allBaseWorldObjects.Remove(objectTarget as BaseWorldObject);

                        break;

                    case ObjectEditor_TypeList.CampObject:
                        objectDatabase.Data.allCampObjects.Remove(objectTarget as CampObject);

                        break;

                    case ObjectEditor_TypeList.Actors:
                        objectDatabase.Data.allBaseActors.Remove(objectTarget as Actor);

                        break;

                    case ObjectEditor_TypeList.Quests:
                        objectDatabase.Data.allBaseQuests.Remove(objectTarget as Quest);

                        break;


                    case ObjectEditor_TypeList.Music:
                        objectDatabase.Data.allMusics.Remove(objectTarget as Music);

                        break;

                    case ObjectEditor_TypeList.Crafting:
                        objectDatabase.Data.allCraftingBench.Remove(objectTarget as CraftingWorkbench);

                        break;

                    case ObjectEditor_TypeList.Animation:
                        objectDatabase.Data.allAnimations.Remove(objectTarget as BaseAnimation);

                        break;

                    case ObjectEditor_TypeList.VehiclePart:
                        objectDatabase.Data.allVehicleParts.Remove(objectTarget as VehiclePart);

                        break;

                    case ObjectEditor_TypeList.SmartphoneApp:
                        objectDatabase.Data.allSmartphoneApps.Remove(objectTarget as SmartphoneApp);

                        break;

                    case ObjectEditor_TypeList.Trivia:
                        objectDatabase.Data.allTrivias.Remove(objectTarget as Trivia);

                        break;

                    case ObjectEditor_TypeList.TemplateAilment:
                        objectDatabase.Data.allTemplateAilments.Remove(objectTarget as TemplateAilment);

                        break;

                    case ObjectEditor_TypeList.NaturePopulatorObject:
                        objectDatabase.Data.allNatureObjects.Remove(objectTarget as NaturalPopulatorObject);

                        break;

                    case ObjectEditor_TypeList.InteriorMat:
                        objectDatabase.Data.allInteriorMaterials.Remove(objectTarget as InteriorMaterial);

                        break;

                    case ObjectEditor_TypeList.WindowDoor:
                        objectDatabase.Data.allWindowDoorObject.Remove(objectTarget as WindowDoorObject);

                        break;

                    case ObjectEditor_TypeList.GridObject:
                        objectDatabase.Data.allGridObjects.Remove(objectTarget as GridObject);

                        break;

                    case ObjectEditor_TypeList.Faction:
                        objectDatabase.Data.allFactions.Remove(objectTarget as Faction);

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
                    PickableScript pickable = createdObject.GetComponent<PickableScript>();

                    if (pickable == null)
                    {
                        if (createdObject.GetComponent<WorldObjectScript>() != null)
                        {
                            WorldObjectScript spawnable = createdObject.GetComponent<WorldObjectScript>();
                            DestroyImmediate(spawnable);
                        }

                        pickable = createdObject.AddComponent<PickableScript>();
                        pickable.pickableData.formID.BaseID = objectTarget.ID;
                        pickable.pickableData.formID.DatabaseID = objectDatabase.Data.name;
                        pickable.pickableData.formID.ObjectType = MainUtility.Check_ObjectType(objectTarget);
                        pickable.pickableData.formID.ReferenceID = objectTarget.ID + "_" + MainUtility.GenerateSpawnableID(6);

                        pickable.pickableData.itemData.DatabaseName = objectDatabase.Data.name;
                        pickable.pickableData.itemData.ID = objectTarget.ID;
                        pickable.pickableData.itemData.item_Type = MainUtility.Check_ItemType(objectTarget as Item);
                    }
                    else
                    {
                        pickable.pickableData.formID.BaseID = objectTarget.ID;
                        pickable.pickableData.formID.DatabaseID = objectDatabase.Data.name;
                        pickable.pickableData.formID.ObjectType = MainUtility.Check_ObjectType(objectTarget);
                        pickable.pickableData.formID.ReferenceID = objectTarget.ID + "_" + MainUtility.GenerateSpawnableID(6);

                        pickable.pickableData.itemData.DatabaseName = objectDatabase.Data.name;
                        pickable.pickableData.itemData.ID = objectTarget.ID;
                        pickable.pickableData.itemData.item_Type = MainUtility.Check_ItemType(objectTarget as Item);
                    }
                }
                else
                {
                    GameObject createdObject = Instantiate(objectTarget.gameModel);
                    WorldObjectScript spawnable = createdObject.GetComponent<WorldObjectScript>();

                    if (spawnable == null)
                    {
                        spawnable = createdObject.AddComponent<WorldObjectScript>();
                        spawnable.Assign_ObjectRefData(new ObjectReference_Data());
                        spawnable.Get_ObjectRefData().formID.BaseID = objectTarget.ID;
                        spawnable.Get_ObjectRefData().formID.DatabaseID = objectDatabase.Data.name;
                        spawnable.Get_ObjectRefData().formID.ObjectType = MainUtility.Check_ObjectType(objectTarget);
                        spawnable.Get_ObjectRefData().formID.ReferenceID = objectTarget.ID + "_" + MainUtility.GenerateSpawnableID(9);
                    }
                    else
                    {
                        spawnable.Get_ObjectRefData().formID.BaseID = objectTarget.ID;
                        spawnable.Get_ObjectRefData().formID.DatabaseID = objectDatabase.Data.name;
                        spawnable.Get_ObjectRefData().formID.ObjectType = MainUtility.Check_ObjectType(objectTarget);
                        spawnable.Get_ObjectRefData().formID.ReferenceID = objectTarget.ID + "_" + MainUtility.GenerateSpawnableID(9);

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