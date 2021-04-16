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
            DestinyEngineController.main.LoadOverworld(fromIslandExit: true);
        }

        private void OnTriggerEnter(Collider other)
        {
            bool is_Vehicle = false;
            bool is_Player = false;

            Transform parent = other.transform.parent;
            int i = 1;
            while (parent != null)
            {
                if (DestinyEngineController.main.ActiveVehicle != null)
                {
                    if (parent.gameObject == DestinyEngineController.main.ActiveVehicle.gameObject)
                    {
                        is_Vehicle = true;
                    }
                }

                parent = parent.parent;
                ++i;
            }

            if (other.transform == DestinyEngineController.ExamplePlayer.transform)
            {
                is_Player = true;
            }

            if (other.CompareTag("Player") && is_Vehicle)
            {
                ExitIsland();
            }

            if (other.CompareTag("Player") && is_Player)
            {
                ExitIsland();
            }
        }
    }
}
