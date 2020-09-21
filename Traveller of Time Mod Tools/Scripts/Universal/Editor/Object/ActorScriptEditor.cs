using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DestinyEngine.Object;
using DestinyEngine.Interact;
using Sirenix.OdinInspector.Editor;
using PixelCrushers;
using PixelCrushers.DialogueSystem;

namespace DestinyEngine.Editor
{

    [CustomEditor(typeof(ActorScript), true)]
    public class ActorScriptEditor : OdinEditor
    {
        private GUISkin skin;
        private GUIStyle buttonStyle;
        private GUIStyle labelStyle;

        private ActorScript actorScript;
        private SerializedProperty property;

        void OnEnable()
        {

            skin = (GUISkin)Resources.Load("DatabaseSkin");
            buttonStyle = skin.GetStyle("Button");
            labelStyle = skin.GetStyle("Label");

            property = serializedObject.FindProperty("currentConversation");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            actorScript = (ActorScript)target;
            EditorUtility.SetDirty(target);

            //EditorGUILayout.LabelField("Actor Editor", EditorStyles.boldLabel);
            


        }
    }
}

