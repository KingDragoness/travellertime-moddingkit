using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;


namespace DestinyEngine.Object
{
    [System.Serializable]
    public class ObjectDatabaseDat
    {
        public string name = "";

        public List<Item_Ammo> allItemAmmo = new List<Item_Ammo>();
        public List<Item_Key> allItemKey = new List<Item_Key>();
        public List<Item_Junk> allItemJunk = new List<Item_Junk>();
        public List<Item_Weapon> allItemWeapon = new List<Item_Weapon>();

        

    }

    [CreateAssetMenu(fileName = "Database Objects", menuName = "DestinyEngine/ObjectDatabase", order = 1)]
    public class ObjectDatabase : ScriptableObject
    {
        public ObjectDatabaseDat Data;
    }
}