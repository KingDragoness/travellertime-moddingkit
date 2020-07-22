using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestinyEngine
{
    public class IslandExit : MonoBehaviour
    {

        public bool overrideGCECoord = true;

        private void ExitIsland()
        {
            print("Exitting Island");
            DestinyMainEngine.main.LoadOverworld(fromIslandExit: true);
        }

        private void OnTriggerEnter(Collider other)
        {
            bool is_Vehicle = false;

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

            if (other.CompareTag("Player") && is_Vehicle)
            {
                ExitIsland();
            }
        }
    }
}
