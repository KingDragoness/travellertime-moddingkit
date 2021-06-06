using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using DestinyEngine;
using DestinyEngine.Utility;

namespace DestinyEngine.Editor
{

    public class DestinyWorldToolbox : EditorWindow
    {
        public class InteractClashed
        {
            public string gameobjectName = "";
            public string entityID = "";
        }

        public GameObject targetIsland;
        private Color almostwhite = new Color(1, 1, 1, 0.2f);

        [MenuItem("Destiny Engine/World Toolbox Helper")]
        static void Init()
        {
            DestinyWorldToolbox window = (DestinyWorldToolbox)EditorWindow.GetWindow(typeof(DestinyWorldToolbox), false, "World Toolbox");
            window.Show();
        }

        public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }

        void OnGUI()
        {
            GUILayout.Label("World Toolbox Helper", EditorStyles.boldLabel);
            GUILayout.Label("Toolbox to monitoring island creation.");
            GUILayout.Space(10);

            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Island Local: ");
                targetIsland = (GameObject)EditorGUILayout.ObjectField(targetIsland, typeof(GameObject), true, GUILayout.Width(250));
                EditorGUILayout.EndHorizontal();
            }

            DrawUILine(almostwhite);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical();
                {
                    GUILayout.Label("Entity System", EditorStyles.boldLabel);

                    if (GUILayout.Button("Check ID Clash", GUILayout.MaxWidth(200)))
                    {
                        DetectInteractable();
                    }
                    if (GUILayout.Button("Resolve ID by Instance", GUILayout.MaxWidth(200)))
                    {
                        DoInteractableChangeID();
                    }

                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                {
                    GUILayout.Label("World Creation", EditorStyles.boldLabel);

                    if (GUILayout.Button("AutoLOD Generate", GUILayout.MaxWidth(200)))
                    {
                        AutoLODGenerate();
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();

        }

        private void AutoLODGenerate()
        {
            AutoLODGrouper[] allLODGroupers = targetIsland.GetComponentsInChildren<AutoLODGrouper>();


            foreach (var lodgrouper in allLODGroupers)
            {
                lodgrouper.AutoLOD();
            }
        }

        private void DetectInteractable()
        {
            List<InteractClashed> List_CollideEntities = new List<InteractClashed>();
            InteractableScript[] allInteractableScripts = targetIsland.GetComponentsInChildren<InteractableScript>();

            foreach (var script in allInteractableScripts)
            {
                InteractClashed iClashed = new InteractClashed();
                iClashed.entityID = script.entityID_;
                iClashed.gameobjectName = script.gameObject.name;

                var matchedClash = List_CollideEntities.Find(x => x.entityID == iClashed.entityID);

                if (matchedClash != null)
                {
                    Debug.LogWarning($"Object: {matchedClash.gameobjectName}, ID: {matchedClash.entityID} already exist! " +
                        $"Clashed: {iClashed.gameobjectName}, {iClashed.entityID}");
                }

                List_CollideEntities.Add(iClashed);
            }

        }

        private void DoInteractableChangeID()
        {
            List<InteractClashed> List_CollideEntities = new List<InteractClashed>();
            InteractableScript[] allInteractableScripts = targetIsland.GetComponentsInChildren<InteractableScript>();

            Undo.RegisterFullObjectHierarchyUndo(targetIsland.gameObject, "Interactables ID resolve");

            foreach (var script in allInteractableScripts)
            {
                InteractClashed iClashed = new InteractClashed();
                iClashed.entityID = script.entityID_;
                iClashed.gameobjectName = script.gameObject.name;

                var matchedClash = List_CollideEntities.Find(x => x.entityID == iClashed.entityID);

                if (matchedClash != null)
                {
                    var EntityID = script.gameObject.name + "_" + script.gameObject.GetInstanceID().ToString();

                    Debug.Log($"Resolved: {matchedClash.gameobjectName}, ID: {matchedClash.entityID} to {EntityID}");
                    script.GenerateID_ByInstance();
                }

                List_CollideEntities.Add(iClashed);
            }

        }
    }

}