﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using DestinyEngine.Object;
using DestinyEngine.Interact;
using Sirenix.OdinInspector.Editor;

namespace DestinyEngine.Editor
{
    [CustomEditor(typeof(ChestContainer), true)]
    public class ChestContainerEditor : OdinEditor
    {
        public ItemData_Type itemType;
        public ObjectDatabase currentDatabase;

        private GUISkin skin;
        private GUIStyle buttonStyle;
        private GUIStyle labelStyle;

        public int selectedItem = 0;
        Item currentItem = null;

        private ChestContainer chestContainer;
        private ItemContainer tempContainer;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            chestContainer = (ChestContainer)target;

            var stagePrefab = chestContainer.gameObject.scene;
            var EditorScenes = EditorSceneManager.GetActiveScene();

            if (stagePrefab == null)
            {
                return;
            }


            if (EditorScenes.name != stagePrefab.name)
            {
                return;
            }



            EditorUtility.SetDirty(target);


            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Container Editor", EditorStyles.boldLabel);
            currentDatabase = (ObjectDatabase)EditorGUILayout.ObjectField(currentDatabase, typeof(ObjectDatabase), false, GUILayout.MaxWidth(200));

            tempContainer = chestContainer.itemContainer;

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginVertical("Box");
            {

                List<string> options = new List<string>();
                List<Item> items = new List<Item>();

                if (currentDatabase != null)
                {
                    itemType = (ItemData_Type)EditorGUILayout.EnumPopup("Item type", itemType);

                    if (itemType == ItemData_Type.Ammo)
                    {
                        items.AddRange(currentDatabase.Data.allItemAmmo);
                    }
                    else if (itemType == ItemData_Type.Junk)
                    {
                        items.AddRange(currentDatabase.Data.allItemJunk);
                    }
                    else if (itemType == ItemData_Type.Key)
                    {
                        items.AddRange(currentDatabase.Data.allItemKey);
                    }
                    else if (itemType == ItemData_Type.Weapon)
                    {
                        items.AddRange(currentDatabase.Data.allItemWeapon);
                    }
                    else if (itemType == ItemData_Type.Misc)
                    {
                        items.AddRange(currentDatabase.Data.allItemMiscs);
                    }

                    items.OrderBy(z => z);

                    foreach (Item item in items)
                    {
                        options.Add(item.ID);
                    }
                }
                selectedItem = EditorGUILayout.Popup("Item", selectedItem, options.ToArray());

                if (items.Count > selectedItem)
                {
                    if (items[selectedItem] != null)
                    {
                        currentItem = items[selectedItem];
                    }
                }
                if (GUILayout.Button("Add Item", GUILayout.Width(100)))
                {
                    if (currentItem != null)
                    {
                        ItemData newItem = new ItemData();
                        newItem.ID = currentItem.ID;
                        newItem.DatabaseName = currentDatabase.Data.name;
                        newItem.count = 1;
                        newItem.item_Type = MainUtility.Check_ItemType(currentItem);

                        tempContainer.all_InventoryItem.Add(newItem);
                    }
                    else
                    {
                        Debug.LogError("No item!");
                    }
                }
            }
            EditorGUILayout.EndVertical();


            EditorGUILayout.BeginVertical("Box");
            {
                foreach(ItemData itemDat in tempContainer.all_InventoryItem)
                {
                    EditorGUILayout.BeginHorizontal("Box");
                    {
                        GenericMenu menu = new GenericMenu();
                        itemDat.count = EditorGUILayout.IntField(itemDat.count, GUILayout.Width(40));

                        if (GUILayout.Button(itemDat.DatabaseName + " | " + itemDat.ID, buttonStyle))
                        {
                            menu.AddItem(new GUIContent("Empty"), false, EmptyFunc);
                            menu.AddItem(new GUIContent("Duplicate"), false, Context_Duplicate, itemDat);
                            menu.AddItem(new GUIContent("Delete"), false, Context_Delete, itemDat);
                            menu.ShowAsContext();

                        }
                    }
                    EditorGUILayout.EndHorizontal();

                }
            }
            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Chest Container");
                chestContainer.itemContainer = tempContainer;
            }
        }

        void EmptyFunc()
        {

        }

        private void Context_Duplicate(object objectItem)
        {
            EditorGUI.BeginChangeCheck();

            ItemData itemData = objectItem as ItemData;

            if (itemData == null)
            {
                return;
            }

            ItemData newItem = new ItemData();
            newItem.ID = itemData.ID;
            newItem.DatabaseName = itemData.DatabaseName;
            newItem.flagData = itemData.flagData;
            newItem.count = itemData.count;
            newItem.item_Type = itemData.item_Type;

            tempContainer.all_InventoryItem.Add(newItem);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Chest Container Dupe");
                chestContainer.itemContainer = tempContainer;
            }
        }

        private void Context_Delete(object objectItem)
        {
            EditorGUI.BeginChangeCheck();

            ItemData itemData = objectItem as ItemData;

            if (itemData == null)
            {
                return;
            }

            tempContainer.all_InventoryItem.Remove(itemData);


            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Chest Container Delete");
                chestContainer.itemContainer = tempContainer;
            }
        }

        protected void OnEnable()
        {
            skin = (GUISkin)Resources.Load("DatabaseSkin");
            buttonStyle = skin.GetStyle("Button");
            labelStyle = skin.GetStyle("Label");

            var data = EditorPrefs.GetString("Destiny.ChestContainerEditor", JsonUtility.ToJson(this, false));
            JsonUtility.FromJsonOverwrite(data, this);
        }

        protected void OnDisable()
        {
            var data = JsonUtility.ToJson(this, false);
            EditorPrefs.SetString("Destiny.ChestContainerEditor", data);
        }
    }
}

