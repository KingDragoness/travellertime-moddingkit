﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestinyEngine
{
    public class IslandTrigger : MonoBehaviour
    {
        public int index = 0;
        public bool overrideGCECoord = true;
        public bool isDungeonEnter = false;
        public string regionName = "";
        public Collider triggerCollider;

        private void Awake()
        {
            triggerCollider.enabled = false;
        }

        private void Start()
        {
            Invoke("ReenableCollider", 1f);
        }

        private void ReenableCollider()
        {
            triggerCollider.enabled = true;
        }

        private void EnterIsland()
        {
            if (DestinyEngineController.IsGameFreezing())
            {
                return;
            }

            EnterLocalIsland_Token token = new EnterLocalIsland_Token();
            token.RegionName = regionName;
            token.SpawnMarkerIndex = index;
            token.isEnterFromOverworld = !isDungeonEnter;

            DestinyEngineController.main.LoadIsland(token);

            if (overrideGCECoord)
            {
                DestinyEngineController.main.OverrideGCECoord(regionName);
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
                EnterIsland();
            }

            if (other.CompareTag("Player") && is_Player)
            {
                EnterIsland();
            }

        }
    }
}
