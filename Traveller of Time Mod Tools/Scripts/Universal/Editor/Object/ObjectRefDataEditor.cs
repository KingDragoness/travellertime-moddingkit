using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DestinyEngine;
using UnityEditor;

namespace DestinyEngine.Editor
{

    [CustomEditor(typeof(ObjectReference_Data), true)]
    public class ObjectRefDataEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

        }

    }

}