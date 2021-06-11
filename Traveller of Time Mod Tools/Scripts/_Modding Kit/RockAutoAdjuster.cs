using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestinyEngine.Utility
{

    public class RockAutoAdjuster : MonoBehaviour
    {

        public Terrain targetTerrain;
        public List<Transform> allRocks = new List<Transform>();

        [ContextMenu("TerrainSetRocks")]
        public void TerrainSetRocks()
        {
            foreach(var rock in allRocks)
            {
                if (rock == null)
                {
                    continue;
                }

                Vector3 realCoord = rock.position;
                float height = GetTerrainHeightAtPos(realCoord);

                rock.transform.position = new Vector3(rock.transform.position.x, height, rock.transform.position.z);
            }
        }

        private float GetTerrainHeightAtPos(Vector3 worldPos)
        {
            return targetTerrain.SampleHeight(worldPos);
        }

    }

}