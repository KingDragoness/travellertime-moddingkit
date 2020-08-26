using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestinyEngine
{
    public class IslandTrigger : MonoBehaviour
    {
        public int index = 0;
        public bool overrideGCECoord = true;
        public string regionName = "";
        public Collider triggerCollider;

        private void EnterIsland()
        {
            DestinyMainEngine.main.LoadIsland
                (
                    regionName:         regionName, 
                    Index:              index, 
                    fromOverworld:      true
                );

            if (overrideGCECoord)
            {
                DestinyMainEngine.main.OverrideGCECoord(regionName);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            bool is_Vehicle = false;
            bool is_Player = false;

            Transform parent = other.transform.parent;
            int i = 1;
            while (parent != null)
            {
                if (DestinyMainEngine.main.ActiveVehicle != null)
                {
                    if (parent.gameObject == DestinyMainEngine.main.ActiveVehicle.gameObject)
                    {
                        is_Vehicle = true;
                    }
                }

                parent = parent.parent;
                ++i;
            }

            if (other.transform == DestinyMainEngine.main.ExamplePlayer.transform)
            {
                is_Player = true;
            }

            if (other.CompareTag("Player") && is_Vehicle)
            {
                EnterIsland();
            }

            if (other.CompareTag("Player") && is_Player)
            {
                EnterIsland();
            }

        }
    }
}
