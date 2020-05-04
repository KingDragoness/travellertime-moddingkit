using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TravellerTime
{
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
        public List<SpawnZone> all_SpawnZones = new List<SpawnZone>();
        public Vector3 exitGCECoord = new Vector3();
    }

    [System.Serializable]
    public class IslandLocal : MonoBehaviour
    {

        public IslandLocal_Data Data;
    }
}