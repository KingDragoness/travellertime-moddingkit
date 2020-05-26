using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestinyEngine
{
    public class Destiny_OcclusionBox : MonoBehaviour
    {
        public LODGroup lodGroup;
        public List<Destiny_OcclusionBox> linkedBox = new List<Destiny_OcclusionBox>();
        private float lodSize = 0;

        private void Awake()
        {
            lodSize = lodGroup.size;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 1f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, transform.localScale);

            Gizmos.color = Color.red;

            foreach (Destiny_OcclusionBox group in linkedBox)
            {
                if (group != null)
                    Gizmos.DrawLine(transform.position, group.transform.position);
            }
        }

        public void OccludeObject()
        {
            lodGroup.size = 0;
        }

        public void DeoccludeObject()
        {
            lodGroup.size = lodSize;
        }


    }
}