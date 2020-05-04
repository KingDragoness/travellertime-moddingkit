using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TravellerTime
{

    [System.Serializable]
    public class ModPack_Data
    {
        public string objectName;
        public string modName = "";
        public bool loadImmediately = false;
        public Vector3 GCECoord = new Vector3(0, 0, 0);
    }
}