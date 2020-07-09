using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if (UNITY_EDITOR) 
    using UnityEditor;
#endif

//DON'T USE THIS SCRIPT DURING BUILD!
namespace DestinyEngine
{
    [ExecuteInEditMode]
    public class OverworldEdit : MonoBehaviour
    {

        #if (UNITY_EDITOR)
        public bool showVanillaWorld = false;
        public Overworld_MapData mapData;
        public GameObject islandPrefab;

        private List<OverworldEditor_Island> pooledMinimapIsland = new List<OverworldEditor_Island>();
        private Overworld_MapData vanillaIslandData;

        [HideInNormalInspector] public bool disableUpdate = false;

        private void Awake()
        {
            pooledMinimapIsland.Clear();
            OverworldEditor_Island[] islands = this.GetComponentsInChildren<OverworldEditor_Island>();

            foreach(OverworldEditor_Island island in islands)
            {
                pooledMinimapIsland.Add(island);
            }
        }

        private void OnDrawGizmos()
        {
            foreach(OverworldEditor_Island editor_Island in pooledMinimapIsland)
            {
                if (editor_Island == null)
                {
                    continue;
                }
                Gizmos.color = new Color(0.1f, 0.9f, 0.1f, 0.7f);
                Gizmos.DrawWireSphere(editor_Island.transform.position, editor_Island.RadiusEnter);

                Handles.Label(editor_Island.transform.position, editor_Island.gameObject.name);
            }
        }


        public void Update_WorldMap()
        {
            string filePath = Application.streamingAssetsPath + "/OverworldData.worldDat";
            vanillaIslandData = JsonUtility.FromJson<Overworld_MapData>(File.ReadAllText(filePath));

            int i = 0;
            pooledMinimapIsland.Clear();
            OverworldEditor_Island[] islands = this.GetComponentsInChildren<OverworldEditor_Island>();

            foreach (OverworldEditor_Island island in islands)
            {
                pooledMinimapIsland.Add(island);
            }

            foreach (IslandData islandData in mapData.GlobalIslands)
            {
                bool valid = false;

                if (pooledMinimapIsland.Count > i)
                {
                    if (pooledMinimapIsland[i] != null)
                    {
                        ReloadIsland(islandData, i);
                        valid = true;
                    }
                    else
                    {

                    }
                }
                
                if (!valid)
                {
                    if (pooledMinimapIsland.Find(z => z.RegionName == islandData.RegionName) != null)
                    {
                        continue;
                    }

                    OverworldEditor_Island Island = Instantiate(islandPrefab, islandData.GCECoord, Quaternion.identity).GetComponent<OverworldEditor_Island>();
                    Island.RegionName = islandData.RegionName;
                    Island.transform.localScale = new Vector3(islandData.IslandSize.x, 1, islandData.IslandSize.z);

                    Island.transform.SetParent(this.transform);
                    Island.gameObject.name = islandData.RegionName;

                    pooledMinimapIsland.Add(Island);
                }

                i++;
            }

            if (showVanillaWorld)
            foreach(IslandData islandData in vanillaIslandData.GlobalIslands)
            {
                bool valid = false;

                if (pooledMinimapIsland.Count > i)
                {
                    if (pooledMinimapIsland[i] != null)
                    {
                        ReloadIsland(islandData, i);
                        valid = true;
                    }
                    else
                    {

                    }
                }

                if (!valid)
                {
                    if (pooledMinimapIsland.Find(z => z.RegionName == islandData.RegionName) != null)
                    {
                        continue;
                    }

                    OverworldEditor_Island Island = Instantiate(islandPrefab, islandData.GCECoord, Quaternion.identity).GetComponent<OverworldEditor_Island>();
                    Island.RegionName = islandData.RegionName;
                    Island.transform.localScale = new Vector3(islandData.IslandSize.x, 1, islandData.IslandSize.z);

                    Island.transform.SetParent(this.transform);
                    Island.gameObject.name = islandData.RegionName;

                    pooledMinimapIsland.Add(Island);
                }

                i++;
            }

            pooledMinimapIsland.RemoveAll(item => item == null);

            Debug.Log("Overworld Map data updated.");
        }

        void ReloadIsland(IslandData islandData, int i)
        {
            pooledMinimapIsland[i].RegionName = islandData.RegionName;
            pooledMinimapIsland[i].transform.localScale = new Vector3(islandData.IslandSize.x, 1, islandData.IslandSize.z);

            pooledMinimapIsland[i].transform.position = islandData.GCECoord;
            pooledMinimapIsland[i].transform.rotation = Quaternion.identity;
            pooledMinimapIsland[i].gameObject.name = islandData.RegionName;
        }

        public void Save_WorldMap(string fileName)
        {
            string mapDat = JsonUtility.ToJson(mapData, true);
            File.WriteAllText(fileName, mapDat);

            Debug.Log("Overworld Map data has been saved.");
        }

        public void Load_WorldMap()
        {
            string filePath = EditorUtility.OpenFilePanel ("Load World Data", Application.dataPath, "json");

            mapData = JsonUtility.FromJson<Overworld_MapData>(File.ReadAllText(filePath));
            Debug.Log("Overworld Map data has been loaded.");

            Update_WorldMap();
        }

        private void OnValidate()
        {
            Update_WorldMap();
        }

        private void LateUpdate()
        {
            if (Selection.activeGameObject != null)
            {
                if (Selection.activeGameObject.GetComponent<OverworldEditor_Island>() != null)
                {
                    disableUpdate = false;
                }
                else
                {
                    disableUpdate = true;
                }
            }
            else
            {
                disableUpdate = true;
            }

            if (disableUpdate)
            {
                return;
            }
            foreach (OverworldEditor_Island editor_Island in pooledMinimapIsland)
            {
                if (editor_Island == null)
                {
                    continue;
                }
                else if (mapData.GlobalIslands.Find(x => x.RegionName == editor_Island.RegionName) == null)
                {
                    continue;
                }

                Vector3 GCE_Coord = editor_Island.transform.position;
                GCE_Coord.x = Mathf.Floor(GCE_Coord.x);
                GCE_Coord.y = Mathf.Floor(GCE_Coord.y);
                GCE_Coord.z = Mathf.Floor(GCE_Coord.z);

                IslandData islandData = mapData.GlobalIslands.Find(x => x.RegionName == editor_Island.RegionName);
                islandData.GCECoord = GCE_Coord;
            }
        }
        #endif
    }
}