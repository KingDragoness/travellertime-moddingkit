﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DestinyEngine.Utility
{
    public class AutoLODGrouper : MonoBehaviour
    {
        public class LOD_Info
        {
            public MeshRenderer rendr;
            public LODGroup lodGroup;
            public int lodlevel = 0;
            public Vector3 pos;
        }

        public bool DEBUG_drawGizmos = true;
        [Range(0.1f, 0.8f)]
        public float LOD0_Size = 0.4f;
        [Range(0.01f, 0.4f)]
        public float LOD1_Size = 0.1f;
        [Range(0.001f, 0.2f)]
        public float LOD2_Size = 0.03f;
        List<LODGroup> allLODGroups_Created = new List<LODGroup>();

        [FoldoutGroup("Grouper LOD")] public GameObject lod0;
        [FoldoutGroup("Grouper LOD")] public GameObject lod1;
        [FoldoutGroup("Grouper LOD")] public GameObject lod2;

        public static float cubeSize = 50f;
        public static float LODSizeScreen = 36f;

        private void OnDrawGizmos()
        {
            if (!DEBUG_drawGizmos)
            {
                return;
            }

            foreach (LODGroup lodgroup in allLODGroups_Created)
            {
                if (lodgroup == null)
                    continue;

                Vector3 pos = lodgroup.transform.position;
                pos.x += cubeSize / 2;
                pos.y += cubeSize / 2;
                pos.z += cubeSize / 2;

                Gizmos.color = new Color(1, 0, 0, 0.3f);
                Gizmos.DrawCube(pos, new Vector3(cubeSize, cubeSize, cubeSize));
            }

        }

        [ContextMenu("AutoLOD")]
        public void AutoLOD()
        {
            allLODGroups_Created = GetComponentsInChildren<LODGroup>().ToList();

            foreach (LODGroup lodgroup in allLODGroups_Created)
            {
                Vector3 pos = lodgroup.transform.position;
                pos /= cubeSize;
                print(pos);

                //Prevent rounding error
                pos.x = Mathf.Round(pos.x * 10) / 10;
                pos.y = Mathf.Round(pos.y * 10) / 10;
                pos.z = Mathf.Round(pos.z * 10) / 10;

                pos.x = Mathf.Floor(pos.x);
                pos.y = Mathf.Floor(pos.y);
                pos.z = Mathf.Floor(pos.z);

                print(pos);

                pos *= cubeSize;

                lodgroup.gameObject.name = "LODRegion - " + pos.ToString();
                lodgroup.transform.SetAsFirstSibling();
                lodgroup.transform.position = pos;

                print(pos);
            }

            List<LOD_Info> all_LODInfo = new List<LOD_Info>();
            List<MeshRenderer> all_LOD0 = new List<MeshRenderer>();
            List<MeshRenderer> all_LOD1 = new List<MeshRenderer>();
            List<MeshRenderer> all_LOD2 = new List<MeshRenderer>();

            all_LOD0 = GetComponentsInChildren<MeshRenderer>().Where(x => x.name.EndsWith("LOD0")).ToList();
            all_LOD1 = GetComponentsInChildren<MeshRenderer>().Where(x => x.name.EndsWith("LOD1")).ToList();
            all_LOD2 = GetComponentsInChildren<MeshRenderer>().Where(x => x.name.EndsWith("LOD2")).ToList();

            foreach (MeshRenderer meshRenderer in all_LOD0)
            {
                LOD_Info lodinfo = CreateLODInfo(meshRenderer, 0);
                all_LODInfo.Add(lodinfo);
            }

            foreach (MeshRenderer meshRenderer in all_LOD1)
            {
                LOD_Info lodinfo = CreateLODInfo(meshRenderer, 1);
                all_LODInfo.Add(lodinfo);
            }

            foreach (MeshRenderer meshRenderer in all_LOD2)
            {
                LOD_Info lodinfo = CreateLODInfo(meshRenderer, 2);
                all_LODInfo.Add(lodinfo);
            }

            foreach (LOD_Info lodinfo in all_LODInfo)
            {
                LODGroup lodgroup = FindNearestLODGroup(allLODGroups_Created, lodinfo);

                if (lodgroup != null)
                {
                    //This is empty
                }
                else
                {
                    lodgroup = new GameObject().AddComponent<LODGroup>();
                    lodgroup.transform.parent = this.gameObject.transform;
                    lodgroup.transform.position = lodinfo.pos;
                    lodgroup.transform.SetAsFirstSibling();

                    allLODGroups_Created.Add(lodgroup);
                }

                if (lodgroup != null)
                    lodinfo.lodGroup = lodgroup;
            }

            foreach (LODGroup lodgroup in allLODGroups_Created)
            {
                LOD_Info[] currentLODInfos = all_LODInfo.Where(x => x.lodGroup == lodgroup).ToArray();
                List<LOD_Info> group_lod0 = currentLODInfos.Where(x => x.lodlevel == 0).ToList();
                List<LOD_Info> group_lod1 = currentLODInfos.Where(x => x.lodlevel == 1).ToList();
                List<LOD_Info> group_lod2 = currentLODInfos.Where(x => x.lodlevel == 2).ToList();

                List<Renderer> renderers_LOD0 = new List<Renderer>();
                List<Renderer> renderers_LOD1 = new List<Renderer>();
                List<Renderer> renderers_LOD2 = new List<Renderer>();

                foreach (LOD_Info lodinfo in group_lod0)
                {
                    renderers_LOD0.Add(lodinfo.rendr);
                }
                foreach (LOD_Info lodinfo in group_lod1)
                {
                    renderers_LOD1.Add(lodinfo.rendr);
                }
                foreach (LOD_Info lodinfo in group_lod2)
                {
                    renderers_LOD2.Add(lodinfo.rendr);
                }

                List<LOD> lods = new List<LOD>();
                LOD lod0 = new LOD();
                LOD lod1 = new LOD();
                LOD lod2 = new LOD();

                lod0.renderers = renderers_LOD0.ToArray();
                lod0.screenRelativeTransitionHeight = LOD0_Size;

                lod1.renderers = renderers_LOD1.ToArray();
                lod1.screenRelativeTransitionHeight = LOD1_Size;

                lod2.renderers = renderers_LOD2.ToArray();
                lod2.screenRelativeTransitionHeight = LOD2_Size;


                lods.Add(lod0);
                lods.Add(lod1);
                lods.Add(lod2);

                lodgroup.gameObject.name = "LODRegion - " + lodgroup.transform.position.ToString();
                lodgroup.SetLODs(lods.ToArray());
                lodgroup.size = LODSizeScreen;
            }
        }

        [ContextMenu("GroupingLOD")]
        public void GroupingLOD()
        {
            if (lod0 == null)
            {
                lod0 = new GameObject();
                lod0.name = "lod0";
                lod0.transform.SetParent(transform);
            }

            if (lod1 == null)
            {
                lod1 = new GameObject();
                lod1.name = "lod1";
                lod1.transform.SetParent(transform);
            }


            if (lod2 == null)
            {
                lod2 = new GameObject();
                lod2.name = "lod2";
                lod2.transform.SetParent(transform);
            }

            List<MeshRenderer> all_LOD0 = new List<MeshRenderer>();
            List<MeshRenderer> all_LOD1 = new List<MeshRenderer>();
            List<MeshRenderer> all_LOD2 = new List<MeshRenderer>();

            all_LOD0 = GetComponentsInChildren<MeshRenderer>().Where(x => x.name.EndsWith("LOD0")).ToList();
            all_LOD1 = GetComponentsInChildren<MeshRenderer>().Where(x => x.name.EndsWith("LOD1")).ToList();
            all_LOD2 = GetComponentsInChildren<MeshRenderer>().Where(x => x.name.EndsWith("LOD2")).ToList();

            foreach (MeshRenderer meshRenderer in all_LOD0)
            {
                meshRenderer.transform.SetParent(lod0.transform);
            }

            foreach (MeshRenderer meshRenderer in all_LOD1)
            {
                meshRenderer.transform.SetParent(lod1.transform);
            }

            foreach (MeshRenderer meshRenderer in all_LOD2)
            {
                meshRenderer.transform.SetParent(lod2.transform);
            }
        }

        private LOD_Info CreateLODInfo(MeshRenderer meshRenderer, int lodlevel)
        {
            Vector3 pos = meshRenderer.transform.position;
            pos /= cubeSize;

            if (pos.x != 0)
                pos.x = Mathf.Floor(pos.x);

            if (pos.y != 0)
                pos.y = Mathf.Floor(pos.y);

            if (pos.z != 0)
                pos.z = Mathf.Floor(pos.z);
            pos *= cubeSize;

            LOD_Info lodinfo = new LOD_Info();
            lodinfo.pos = pos;
            lodinfo.lodlevel = lodlevel;
            lodinfo.rendr = meshRenderer;

            return lodinfo;
        }

        private LODGroup FindNearestLODGroup(List<LODGroup> LODGroupLists, LOD_Info lodinfo)
        {
            foreach (LODGroup lod in LODGroupLists)
            {

                if (ValueOnRange(lodinfo.pos.x, lod.transform.position.x - 1, lod.transform.position.x + 1))
                {
                    if (ValueOnRange(lodinfo.pos.y, lod.transform.position.y - 1, lod.transform.position.y + 1))
                    {
                        if (ValueOnRange(lodinfo.pos.z, lod.transform.position.z - 1, lod.transform.position.z + 1))
                        {
                            return lod;
                        }
                    }
                }
            }

            return null;
        }

        private bool ValueOnRange(float thisValue, float value1, float value2)
        {
            return thisValue >= Mathf.Min(value1, value2) && thisValue <= Mathf.Max(value1, value2);
        }

    }
}