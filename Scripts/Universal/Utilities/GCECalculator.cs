using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DestinyEngine.Object;
using System.IO;

namespace DestinyEngine
{
    public static class GCECalculator
    {

        public static Vector3 Subcoord_Adjust(Vector3 vehicleCoord)
        {
            Vector3 coordResult = vehicleCoord;
            Vector3 tempCoord = vehicleCoord;

            coordResult.x = mod(Mathf.FloorToInt(coordResult.x), 100);
            coordResult.z = mod(Mathf.FloorToInt(coordResult.z), 100);
            Debug.Log("Coord Mod: " + coordResult);
            tempCoord = coordResult;

            coordResult.x = -100 + coordResult.x;
            coordResult.z = -100 + coordResult.z;

            Debug.Log(coordResult);

            return coordResult;
        }

        static int mod(int a, int n)
        {
            int result = a % n;
            if ((result < 0 && n > 0) || (result > 0 && n < 0))
            {
                result += n;
            }
            return result;
        }

        public static Vector3 GCEPosition_ToLocal(Vector3 SubCoord, Vector3 GCECoord, Vector3 currentGCECoord)
        {
            Vector3 localGCE_Coord = GCECoord;
            localGCE_Coord.x -= currentGCECoord.x;
            localGCE_Coord.z -= currentGCECoord.z;
            localGCE_Coord *= 100;

            localGCE_Coord += SubCoord;

            return localGCE_Coord;
        }

        public static Vector3 Local_ToGCE(Vector3 localPosition)
        {
            Vector3 ResultGCE = localPosition;

            ResultGCE.x = Mathf.Ceil(ResultGCE.x / 100);
            ResultGCE.z = Mathf.Ceil(ResultGCE.z / 100);
            ResultGCE.y = 0;

            ResultGCE.x += 1;
            ResultGCE.z += 1;

            return ResultGCE;
        }
    }

    public class CoroutineWithData
    {
        public Coroutine coroutine { get; private set; }
        public object result;
        private IEnumerator target;
        public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
        {
            this.target = target;
            this.coroutine = owner.StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while (target.MoveNext())
            {
                result = target.Current;
                yield return result;
            }
        }
    }

    public static class DestinyMainUtility
    {
        public static string GetFileLocation(string relativePath)
        {
            return "file://" + Path.Combine(Application.streamingAssetsPath, relativePath);
        }

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

            else if (baseObject is Item_Misc)
            {
                return ObjectData_Type.Misc;
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

            else if (item is Item_Misc)
            {
                return ItemData_Type.Item_Misc;
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