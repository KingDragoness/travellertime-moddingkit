using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace DestinyEngine.Object
{

    public class AssetHandler
    {
        [OnOpenAsset()]
        public static bool OpenEditor(int instanceId, int line)
        {
            ObjectDatabase obj = EditorUtility.InstanceIDToObject(instanceId) as ObjectDatabase;
            if (obj != null)
            {
                ObjectDatabaseEditorWindow.OpenWindow(obj);
                return true;
            }

            return false;
        }
    }

    public class ObjectDatabaseEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            ObjectDatabaseEditorWindow.OpenWindow((ObjectDatabase)target);
        }
    }
}