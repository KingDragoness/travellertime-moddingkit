using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestinyEngine
{
    public class Destiny_OcclusionBox : MonoBehaviour
    {
        public LODGroup lodGroup;
        private float lodSize = 0;

        private void Awake()
        {
            lodSize = lodGroup.size;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }


    }
}