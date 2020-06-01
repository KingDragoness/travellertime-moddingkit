using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestinyEngine
{



    [System.Serializable]
    public class IslandData
    {
        public string NameDisplay = "";
        public string RegionName = "";
        public string ModPackName = "vanilla";
        public Vector3 GCECoord = new Vector3(0, 0, 0);

        [Tooltip("IslandSize Y axis will not affect the scale.")]
        public Vector3 IslandSize = new Vector3(1, 1, 1);

    }

    [System.Serializable]
    public class Overworld_MapData
    {
        public List<IslandData> GlobalIslands = new List<IslandData>();

        public Overworld_MapData(List<IslandData> globalIslands)
        {
            GlobalIslands = globalIslands;
        }
    }

}