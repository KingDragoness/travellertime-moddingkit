using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DestinyEngine;


[CustomEditor(typeof(Destiny_LocalOcclusionManager))]
public class OcclusionManagerWindow : Editor
{
    SerializedProperty m_exampleChar;
    SerializedProperty m_DEBUG_OccludeTest;
    SerializedProperty m_AllOcclusionBounds;

    private void OnEnable()
    {
        m_exampleChar = serializedObject.FindProperty("exampleChar");
        m_DEBUG_OccludeTest = serializedObject.FindProperty("DEBUG_OccludeTest");
        m_AllOcclusionBounds = serializedObject.FindProperty("m_AllOcclusionBounds");

    }

    public override void OnInspectorGUI()
    {
        Destiny_LocalOcclusionManager Manager = (Destiny_LocalOcclusionManager)target;

        EditorGUILayout.PropertyField(m_exampleChar, new GUIContent("Target Transform"));
        EditorGUILayout.PropertyField(m_DEBUG_OccludeTest, new GUIContent("Debug [OccludeTest]"));

        if (GUILayout.Button("Refresh occlusion bounds", GUILayout.Height(42)))
        {
            Manager.Get_AllOcclusionBounds();
        }

        EditorUtility.SetDirty(target);

        foreach (OcclusionDat dat in Manager.m_AllOcclusionBounds)
        {
            EditorUtility.SetDirty(dat.occlusionBox);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
