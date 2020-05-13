using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestinyEngine
{
    [System.Flags] public enum IslandLocal_Flags
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
        public List<Interactables> all_Interactables = new List<Interactables>();
        public List<IslandLocal_Flags> all_Flags = new List<IslandLocal_Flags>();
        public List<SpawnZone> all_SpawnZones = new List<SpawnZone>();

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
    public class IslandLocal : MonoBehaviour
    {

        public IslandLocal_Data Data;

        int i = 0;

        private void Start()
        {
        }

    }
}
