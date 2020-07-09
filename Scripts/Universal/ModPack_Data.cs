using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestinyEngine
{
    [System.Serializable]
    public class WorldBundle_Setting
    {
        public string objectName;
        public List<ModPack_Data> packDatas = new List<ModPack_Data>();
    }

    [System.Serializable]
    public class ModPack_Data
    {
        public string modName = "";
        public bool loadImmediately = false;
        public Vector3 GCECoord = new Vector3(0, 0, 0);
    }
}