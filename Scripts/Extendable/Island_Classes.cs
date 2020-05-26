using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestinyEngine
{
    [System.Flags]
    public enum IslandLocal_Flags
    {
        Interior,
        DisableWater
    }

    [System.Serializable]
    public class SpawnZone
    {
        public Transform spawn_Airplane;
        public Transform spawn_Boat;
        public Transform spawn_Player;
    }

    [System.Serializable]
    public class IslandLocal_Data
    {
        public string RegionName = "";
        public List<MonoBehaviour> all_Interactables = new List<MonoBehaviour>();
        public List<IslandLocal_Flags> all_Flags = new List<IslandLocal_Flags>();
        public List<SpawnZone> all_SpawnZones = new List<SpawnZone>();
        public Transform Vehicle;

        public bool CheckFlag(IslandLocal_Flags Flag)
        {
            if (all_Flags.Contains(Flag))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    [System.Serializable]
    public class IslandGlobal_Data
    {
        public string RegionName = "";

        [Tooltip("When entered, this island will overwrite current GCE Coord.")]
        public bool overrideGCECoord = true;

    }

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