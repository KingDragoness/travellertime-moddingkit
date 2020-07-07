using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DestinyEngine.Object;

namespace DestinyEngine
{
    public static class GCECalculator
    {

        public static Vector3 GCEPosition_ToLocal(Vector3 SubCoord, Vector3 GCECoord, Vector3 currentGCECoord)
        {
            Vector3 localGCE_Coord = GCECoord;
            localGCE_Coord.x -= currentGCECoord.x;
            localGCE_Coord.z -= currentGCECoord.z;
            localGCE_Coord *= 100;

            localGCE_Coord += SubCoord;

            return localGCE_Coord;
        }
    }

    public static class DestinyMainUtility
    {
        public static ObjectData_Type Check_ObjectType(BaseObject baseObject)
        {
            if (baseObject is Item_Ammo)
            {
                return ObjectData_Type.Ammo;
            }
            else if (baseObject is Item_Junk)
            {
                return ObjectData_Type.Junk;
            }
            else if (baseObject is Item_Weapon)
            {
                return ObjectData_Type.Weapon;
            }
            else if (baseObject is Item_Key)
            {
                return ObjectData_Type.Key;
            }
            else if (baseObject is WorldObject)
            {
                return ObjectData_Type.BaseWorldObject;
            }
            else
            {
                return ObjectData_Type.Ammo;
            }

        }

        public static ItemData_Type Check_ItemType(Item item)
        {
            if (item is Item_Ammo)
            {
                return ItemData_Type.Item_Ammo;
            }
            else if (item is Item_Junk)
            {
                return ItemData_Type.Item_Junk;
            }
            else if (item is Item_Weapon)
            {
                return ItemData_Type.Item_Weapon;
            }
            else if (item is Item_Key)
            {
                return ItemData_Type.Item_Key;
            }     
            else
            {
                return ItemData_Type.Item_Ammo;
            }

        }
        public static string GenerateStupidID(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!?";
            var stringChars = new char[length];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[Random.Range(0, chars.Length)];
            }

            return new string(stringChars);
        }
    }
}