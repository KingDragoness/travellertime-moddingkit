using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CleanupMissingScriptsHelper
{
    [MenuItem("Edit/Cleanup Missing Scripts")]
    static void CleanupMissingScripts()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(Selection.gameObjects[i]);
        }
    }


    [MenuItem("Edit/Recursive Cleanup Missing Scripts")]
    static void RecursiveCleanupMissingScripts()
    {
        Transform[] allTransforms = Selection.gameObjects[0].GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < allTransforms.Length; i++)
        {
            var gameObject = allTransforms[i].gameObject;

            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);

        }
    }
}