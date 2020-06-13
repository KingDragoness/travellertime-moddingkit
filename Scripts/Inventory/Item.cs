using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestinyEngine.Object
{

    public abstract class Object
    {
        public string ID = "";
        public string name = "";
    }
    public abstract class Item : Object
    {
        public float value      = 100;
        public short maxStack   = 32767; // property
       
    }

    [System.Serializable]
    public class Item_Ammo : Item
    {

        public static Item_Ammo Copy(Item_Ammo origin)
        {
            Item_Ammo newItem = new Item_Ammo();
            newItem.ID          = origin.ID;
            newItem.name        = origin.name;
            newItem.value       = origin.value;
            newItem.maxStack    = origin.maxStack;

            return newItem;
        }
    }

    [System.Serializable]
    public class Item_Junk : Item
    {
        public static Item_Junk Copy(Item_Junk origin)
        {
            Item_Junk newItem = new Item_Junk();
            newItem.ID = origin.ID;
            newItem.name = origin.name;
            newItem.value = origin.value;
            newItem.maxStack = origin.maxStack;

            return newItem;
        }
    }

    [System.Serializable]
    public class Item_Key : Item
    {
        public static Item_Key Copy(Item_Key origin)
        {
            Item_Key newItem = new Item_Key();
            newItem.ID = origin.ID;
            newItem.name = origin.name;
            newItem.value = origin.value;
            newItem.maxStack = origin.maxStack;

            return newItem;
        }
    }

    [System.Serializable]
    public class Item_Weapon : Item
    {
        public GameObject weaponHandModel;

        [Tooltip("Leave it blank or assign invalid ID if it doesn't use ammo.")]
        public string ammoID = ""; 
        public int baseDamage = 5;

        public static Item_Weapon Copy(Item_Weapon origin)
        {
            Item_Weapon newItem = new Item_Weapon();
            newItem.ID = origin.ID;
            newItem.name = origin.name;
            newItem.value = origin.value;
            newItem.maxStack = origin.maxStack;

            newItem.weaponHandModel = origin.weaponHandModel;
            newItem.ammoID = origin.ammoID;
            newItem.baseDamage = origin.baseDamage;

            return newItem;
        }
    }
}