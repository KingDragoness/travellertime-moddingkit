using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DestinyEngine.Utility;
using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(RockTerrainGenerator))]
public class RockTerrainGeneratorEditor : OdinEditor
{

    RockTerrainGenerator rockGen;

    public override void OnInspectorGUI()
    {
        rockGen = (RockTerrainGenerator)target;
        base.OnInspectorGUI();
        // Add another property to inspector:

        if (GUILayout.Button("Build rocks"))
        {
            BuildRocks();
        }
    }

    public void BuildRocks()
    {

        if (rockGen.allRockPrefabs.Count == 0)
        {
            Debug.Log("Rock Prefab Index is empty! Assign a new rock prefab!");
            return;
        }

        int totalRockCount = rockGen.rockSpawnAttempts;

        for (int x = 0; x < totalRockCount; x++)
        {
            float randomPosX = Random.Range(-rockGen.transform.localScale.x / 2, rockGen.transform.localScale.x / 2);
            float randomPosZ = Random.Range(-rockGen.transform.localScale.z / 2, rockGen.transform.localScale.z / 2);
            randomPosX += rockGen.transform.position.x;
            randomPosZ += rockGen.transform.position.z;

            RockPrefabIndex rockPrefabIndex = rockGen.allRockPrefabs[Random.Range(0, rockGen.allRockPrefabs.Count - 1)];

            Vector3 raycastPosition = new Vector3(randomPosX, rockGen.transform.position.y, randomPosZ);

            RaycastHit hit;
            if (Physics.Raycast(raycastPosition, rockGen.transform.TransformDirection(Vector3.down), out hit, rockGen.raycastLength, rockGen.collisionMasks))
            {
                Vector3 targetPosition = hit.point;

                if (targetPosition.y > rockGen.terrainMinLimit && targetPosition.y < rockGen.terrainMaxLimit)
                    SpawnRock(rockPrefabIndex, targetPosition);
            }
            else
            {

            }

        }
    }

    private void SpawnRock(RockPrefabIndex rock, Vector3 position)
    {
        Vector3 eulerAngleRandomized = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        var gameObjectRock = (GameObject)PrefabUtility.InstantiatePrefab(rock.prefabRock as GameObject);

        gameObjectRock.transform.position = position;
        gameObjectRock.transform.eulerAngles = eulerAngleRandomized;

        if (rockGen.prefabContainer != null)
            gameObjectRock.transform.SetParent(rockGen.prefabContainer);

        Material selectedMaterial = rock.materialPalletes[Random.Range(0, rock.materialPalletes.Length - 1)];
        MeshRenderer[] allRenderers = gameObjectRock.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer meshRenderer in allRenderers)
        {
            meshRenderer.material = selectedMaterial;
        }

    }
}
