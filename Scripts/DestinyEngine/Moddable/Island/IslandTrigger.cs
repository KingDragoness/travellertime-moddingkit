﻿using System.Collections;
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

        public void EnterIsland()
        {
            DestinyMainEngine.instance.Enter_Island(regionName, index, overrideGCECoord);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EnterIsland();
            }
        }
    }
}