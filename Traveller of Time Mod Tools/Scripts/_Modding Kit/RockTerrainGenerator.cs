using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestinyEngine.Utility
{

    [System.Serializable]
    public class RockPrefabIndex
    {
        public GameObject prefabRock;
        public Material[] materialPalletes;
    }

    public class RockTerrainGenerator : MonoBehaviour
    {

        public List<RockPrefabIndex> allRockPrefabs = new List<RockPrefabIndex>();
        public Transform prefabContainer;
        public int rockSpawnAttempts = 100;
        public LayerMask collisionMasks;
        public float raycastLength = 200;
        public float terrainMinLimit = 30;
        public float terrainMaxLimit = 30;

        private void OnDrawGizmosSelected()
        {
            Vector3 positionCube = transform.position;
            positionCube.y -= (raycastLength / 2);
            Gizmos.color = new Color(1, 0, 0, 0.8f);
            Gizmos.DrawWireCube(positionCube, new Vector3(transform.localScale.x, raycastLength, transform.localScale.z));
        }

       

    }
}