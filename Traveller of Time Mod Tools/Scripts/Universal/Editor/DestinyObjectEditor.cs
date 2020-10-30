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
        public BaseObject objectTarget;

        private ObjectEditor_TypeList typeList = ObjectEditor_TypeList.None;
        private ObjectDatabase objectDatabase = null;
        private bool isCreateNewObjectMode = false;
        private List<int>       indexes = new List<int>();
        private List<bool>      booleans = new List<bool>();
        private List<string>    string_cache = new List<string>();

        private Vector2 scrollPos;

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

        protected override void OnGUI()
        {
            EditorUtility.SetDirty(objectDatabase);
            base.OnGUI();
        }


        /*
        private void OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            if (isCreateNewObjectMode)
            {
                GUILayout.Label("Create New Object");
            }

            DrawInspector();

            EditorGUILayout.EndScrollView();
        }

        void DrawInspector()
        {
            serializedObject.Update();

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

                case ObjectEditor_TypeList.Armor:
                    index = objectDatabase.Data.allItemArmors.FindIndex(x => x == objectTarget);
                    listName = "allItemArmors";
                    break;


                case ObjectEditor_TypeList.Consume:
                    index = objectDatabase.Data.allItemConsumables.FindIndex(x => x == objectTarget);
                    listName = "allItemConsumables";
                    break;

                case ObjectEditor_TypeList.Misc:
                    index = objectDatabase.Data.allItemMiscs.FindIndex(x => x == objectTarget);
                    listName = "allItemMiscs";
                    break;

                case ObjectEditor_TypeList.BaseWorldObject:
                    index = objectDatabase.Data.allBaseWorldObjects.FindIndex(x => x == objectTarget);
                    listName = "allBaseWorldObjects";
                    break;

                case ObjectEditor_TypeList.Actors:
                    index = objectDatabase.Data.allBaseActors.FindIndex(x => x == objectTarget);
                    listName = "allBaseActors";
                    break;

                case ObjectEditor_TypeList.Quests:
                    index = objectDatabase.Data.allBaseQuests.FindIndex(x => x == objectTarget);
                    listName = "allBaseQuests";
                    break;

                case ObjectEditor_TypeList.Music:
                    index = objectDatabase.Data.allMusics.FindIndex(x => x == objectTarget);
                    listName = "allMusics";
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

            }
            else
            {

            }

            serializedObject.ApplyModifiedProperties();
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
                    propNames.AddRange(Get_PropNames(objectTarget));
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

                case ObjectEditor_TypeList.Armor:
                    DrawProperties(serializedList, true);

                    break;

                case ObjectEditor_TypeList.Consume:
                    DrawProperties(serializedList, true);

                    break;

                case ObjectEditor_TypeList.Misc:
                    DrawProperties(serializedList, true);

                    break;

                case ObjectEditor_TypeList.BaseWorldObject:
                    DrawProperties(serializedList, true);

                    break;

                case ObjectEditor_TypeList.Actors:
                    propNames.AddRange(Get_PropNames(objectTarget));

                    foreach (string s in propNames)
                    {
                        DrawField(s, true);

                        if (s == "itemContainer")
                        {                            
                            booleans[0] = GUILayout.Toggle(booleans[0], "Item Container");

                            if (booleans[0])
                            {
                                Actor actor = objectTarget as Actor;
                                ItemContainer itemContainer = actor.itemContainer;

                                EditorGUILayout.BeginVertical("Box");
                                {
                                    foreach (ItemData itemDat in itemContainer.all_InventoryItem)
                                    {
                                        EditorGUILayout.BeginHorizontal("Box");
                                        {
                                            GenericMenu menu = new GenericMenu();
                                            itemDat.count = EditorGUILayout.IntField(itemDat.count, GUILayout.Width(40));

                                            if (GUILayout.Button(itemDat.DatabaseName + " | " + itemDat.ID))
                                            {   

                                            }
                                        }
                                        EditorGUILayout.EndHorizontal();

                                    }
                                }
                                EditorGUILayout.EndVertical();
                            }
                        }

                    }
                    break;

                case ObjectEditor_TypeList.Quests:
                    propNames.AddRange(Get_PropNames(objectTarget));

                    foreach (string s in propNames)
                    {
                        bool b = false;

                        if (s == "questAlias")
                        {
                            b = true;

                            booleans[0] = GUILayout.Toggle(booleans[0], "Quest Aliases");

                            if (booleans[0])
                            {
                                Quest quest = objectTarget as Quest;

                                string propName = "questAlias";
                                SerializedProperty prop_listAlias = currentProperty.FindPropertyRelative(propName);
                                var object_listAlias = GetTargetObjectOfProperty(prop_listAlias);
                                var questAliases = object_listAlias as List<QuestAlias>;

                                EditorGUILayout.BeginHorizontal("Box");
                                {
                                    EditorGUILayout.BeginVertical("Box", GUILayout.Width(250));
                                    {
                                        if (GUILayout.Button("[NEW ALIAS]"))
                                        {
                                            questAliases.Add(new QuestAlias());
                                        }

                                        for (int x = 0; x < questAliases.Count; x++)
                                        {
                                            SerializedProperty prop_questAlias = currentProperty.FindPropertyRelative(propName).GetArrayElementAtIndex(x);
                                            var fuckingobject = GetTargetObjectOfProperty(prop_questAlias);
                                            var whatever_this_is_shit = fuckingobject as QuestAlias;


                                            if (GUILayout.Button(whatever_this_is_shit.aliasName))
                                            {
                                                indexes[0] = x;
                                                GUI.FocusControl(null);
                                            }
                                        }

                                    }
                                    EditorGUILayout.EndVertical();

                                    EditorGUILayout.BeginVertical("Box");
                                    {
                                        if (questAliases.Count > indexes[0])
                                        {
                                            Draw_QuestAliasUI(questAliases[indexes[0]], indexes[0]);

                                            if (GUILayout.Button("Delete alias"))
                                            {
                                                questAliases.Remove(questAliases[indexes[0]]);
                                            }
                                        }
                                    }
                                    EditorGUILayout.EndVertical();
                                }
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.Space();
                                EditorGUILayout.Space();

                            }
                        }
                        else if (s == "questObjectives")
                        {
                            b = true;
                            booleans[1] = GUILayout.Toggle(booleans[1], "Quest Objectives");

                            if (booleans[1])
                            {
                                Quest quest = objectTarget as Quest;
                                List<QuestObjective> questObjectives = quest.questObjectives;

                                EditorGUILayout.BeginHorizontal("Box");
                                {
                                    EditorGUILayout.BeginVertical("Box", GUILayout.Width(250));
                                    {
                                        if (GUILayout.Button("[NEW OBJECTIVE]"))
                                        {
                                            questObjectives.Add(new QuestObjective());
                                        }

                                        for (int x = 0; x < questObjectives.Count; x++)
                                        {
                                            if (GUILayout.Button(questObjectives[x].objectiveText))
                                            {
                                                indexes[1] = x;
                                                GUI.FocusControl(null);
                                            }
                                        }

                                    }
                                    EditorGUILayout.EndVertical();

                                    EditorGUILayout.BeginVertical("Box");
                                    {
                                        if (questObjectives.Count > indexes[1])
                                        {
                                            Draw_QuestObjectiveUI(questObjectives[indexes[1]], indexes[1]);

                                            if (GUILayout.Button("Delete objective"))
                                            {
                                                questObjectives.Remove(questObjectives[indexes[1]]);
                                            }
                                        }
                                    }
                                    EditorGUILayout.EndVertical();
                                }
                                EditorGUILayout.EndHorizontal();
                            }
                        }

                        if (b == false)
                        {
                            DrawField(s, true);
                        }
                    }
                    break;

                case ObjectEditor_TypeList.Music:
                    DrawProperties(serializedList, true);

                    break;

                default:
                    DrawProperties(serializedList, true);

                    break;
            }


        }

        private void Draw_QuestAliasUI(QuestAlias questAlias, int index)
        {
            string propName = "questAlias";
            SerializedProperty prop_questAlias = currentProperty.FindPropertyRelative(propName).GetArrayElementAtIndex(index);
            SerializedProperty prop_aliasName = prop_questAlias.FindPropertyRelative("aliasName");
            SerializedProperty prop_aliasType = prop_questAlias.FindPropertyRelative("aliasType");
            SerializedProperty prop_islandID = prop_questAlias.FindPropertyRelative("islandID");
            SerializedProperty prop_databaseID = prop_questAlias.FindPropertyRelative("databaseID");

            var fuckingobject = GetTargetObjectOfProperty(prop_aliasType);
            var questAlias_aliasType = (Quest_AliasFillType)fuckingobject;


            DrawUILine(color: Color.black, 1, 10);
            EditorGUILayout.PropertyField(prop_aliasName, true);
            EditorGUILayout.PropertyField(prop_aliasType, true);
            EditorGUILayout.PropertyField(prop_islandID, true);

            //questAlias.aliasName = EditorGUILayout.TextField("Alias Name: ", questAlias.aliasName);
            //questAlias.aliasType = (Quest_AliasFillType)EditorGUILayout.EnumPopup("Fill type: ", questAlias.aliasType);
            //questAlias.islandID = EditorGUILayout.TextField("Default island: ", questAlias.islandID);

            if (questAlias_aliasType == Quest_AliasFillType.UniqueActor)
            {
                SerializedProperty prop_local = prop_questAlias.FindPropertyRelative("actor_ID");

                EditorGUILayout.PropertyField(prop_local, true);
                EditorGUILayout.PropertyField(prop_databaseID, true);

            }

            if (questAlias_aliasType == Quest_AliasFillType.UniqueVehicle | questAlias_aliasType == Quest_AliasFillType.NearestVehicle)
            {
                SerializedProperty prop_local = prop_questAlias.FindPropertyRelative("vehicle_ID");

                EditorGUILayout.PropertyField(prop_local, true);

            }

            if (questAlias_aliasType == Quest_AliasFillType.UniqueObject)
            {
                SerializedProperty prop_local = prop_questAlias.FindPropertyRelative("object_ID");
                SerializedProperty prop_objectType = prop_questAlias.FindPropertyRelative("objectData_Type");

                EditorGUILayout.PropertyField(prop_local, true);
                EditorGUILayout.PropertyField(prop_databaseID, true);
                EditorGUILayout.PropertyField(prop_objectType, true);

            }

            if (questAlias_aliasType == Quest_AliasFillType.LocationSpecific)
            {
                SerializedProperty prop_local = prop_questAlias.FindPropertyRelative("localPositionIsland");

                EditorGUILayout.PropertyField(prop_local, true);
            }

            if (questAlias_aliasType == Quest_AliasFillType.ConsultToQuestObject)
            {
                SerializedProperty prop_consult = prop_questAlias.FindPropertyRelative("consultCommandName");

                EditorGUILayout.PropertyField(prop_consult, true);
            }
        }


        private void Draw_QuestObjectiveUI(QuestObjective questObjective, int index)
        {
            string propName = "questObjectives";
            SerializedProperty prop_questObjective = currentProperty.FindPropertyRelative(propName).GetArrayElementAtIndex(index);
            SerializedProperty prop_questIndex = prop_questObjective.FindPropertyRelative("questIndex");
            SerializedProperty prop_objectiveText = prop_questObjective.FindPropertyRelative("objectiveText");
            SerializedProperty prop_targetReference = prop_questObjective.FindPropertyRelative("targetReference");

            EditorGUILayout.PropertyField(prop_questIndex, true);
            EditorGUILayout.PropertyField(prop_objectiveText, true);
            EditorGUILayout.PropertyField(prop_targetReference, true);
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="type">Item / Weapon</param>
        /// <returns></returns>
        List<string> Get_PropNames(object object_)
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
                case ObjectEditor_TypeList.Armor:
                    {
                        Item_Armor newItem4 = objectTarget as Item_Armor;
                        objectDatabase.Data.allItemArmors.Add(newItem4);

                        break;
                    }
                case ObjectEditor_TypeList.Consume:
                    {
                        Item_Consumables newItem4 = objectTarget as Item_Consumables;
                        objectDatabase.Data.allItemConsumables.Add(newItem4);

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

        public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }


        public static object GetTargetObjectOfProperty(SerializedProperty prop)
        {
            if (prop == null) return null;

            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');
            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue_Imp(obj, elementName, index);
                }
                else
                {
                    obj = GetValue_Imp(obj, element);
                }
            }
            return obj;
        }

        private static object GetValue_Imp(object source, string name)
        {
            if (source == null)
                return null;
            var type = source.GetType();

            while (type != null)
            {
                var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (f != null)
                    return f.GetValue(source);

                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p != null)
                    return p.GetValue(source, null);

                type = type.BaseType;
            }
            return null;
        }

        private static object GetValue_Imp(object source, string name, int index)
        {
            var enumerable = GetValue_Imp(source, name) as System.Collections.IEnumerable;
            if (enumerable == null) return null;
            var enm = enumerable.GetEnumerator();
            //while (index-- >= 0)
            //    enm.MoveNext();
            //return enm.Current;

            for (int i = 0; i <= index; i++)
            {
                if (!enm.MoveNext()) return null;
            }
            return enm.Current;
        }
        */
    }
}