using System;
using UnityEngine;


namespace UnityStandardAssets.Utility
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 7.5f, 0f);
        public bool lockY = false;
        public float YCoord = 0;

        private void LateUpdate()
        {
            if (target != null)
            {
                if (!lockY)
                    transform.position = target.position + offset;
                else
                {
                    Vector3 v = target.position + offset;
                    v.y = YCoord;

                    transform.position = v;
                }
            }
        }
    }
}
