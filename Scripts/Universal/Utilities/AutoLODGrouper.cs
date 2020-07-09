using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Utility
{
    public class AutoLODGrouper : MonoBehaviour
    {
        public class LOD_Info
        {
            public MeshRenderer     rendr;
            public LODGroup         lodGroup;
            public int              lodlevel = 0;
            public Vector3          pos;
        }

        [Range(0.1f,0.8f)]
        public float LOD0_Size = 0.4f;
        [Range(0.01f, 0.4f)]
        public float LOD1_Size = 0.1f;
        [Range(0.001f, 0.2f)]
        public float LOD2_Size = 0.03f;
        List<LODGroup> allLODGroups_Created = new List<LODGroup>();

        private void OnDrawGizmos()
        {
            foreach (LODGroup lodgroup in allLODGroups_Created)
            {
                if (lodgroup == null)
                    continue;

                Vector3 pos = lodgroup.transform.position;
                pos.x += 50;
                pos.y += 50;
                pos.z += 50;

                Gizmos.color = new Color(1, 0, 0, 0.3f);
                Gizmos.DrawCube(pos, new Vector3(100, 100, 100));
            }

        }

        [ContextMenu("AutoLOD")]
        public void AutoLOD()
        {
            allLODGroups_Created = GetComponentsInChildren<LODGroup>().ToList();

            foreach (LODGroup lodgroup in allLODGroups_Created)
            {
                Vector3 pos = lodgroup.transform.position;
                pos /= 100;
                pos.x = Mathf.Floor(pos.x);
                pos.y = Mathf.Floor(pos.y);
                pos.z = Mathf.Floor(pos.z);
                pos *= 100;

                lodgroup.gameObject.name = "LODRegion - " + pos.ToString();
                lodgroup.transform.position = pos;
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

            //Setting lodinfo's lodgroup
            foreach (LOD_Info lodinfo in all_LODInfo)
            {
                LODGroup lodgroup;

                if (allLODGroups_Created.Find(vec => 
                vec.transform.position.x == lodinfo.pos.x &&
                vec.transform.position.y == lodinfo.pos.y &&
                vec.transform.position.z == lodinfo.pos.z) != null) 
                {
                    lodgroup = allLODGroups_Created.Find(vec =>
                    vec.transform.position.x == lodinfo.pos.x &&
                    vec.transform.position.y == lodinfo.pos.y &&
                    vec.transform.position.z == lodinfo.pos.z);
                }
                else
                {
                    lodgroup = new GameObject().AddComponent<LODGroup>();
                    lodgroup.transform.parent = this.gameObject.transform;
                    lodgroup.transform.position = lodinfo.pos;

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

                lodgroup.gameObject.name = "LODRegion - " +lodgroup.transform.position.ToString();
                lodgroup.SetLODs(lods.ToArray());
                lodgroup.size = 75f;
            }
        }

        private LOD_Info CreateLODInfo(MeshRenderer meshRenderer, int lodlevel)
        {
            Vector3 pos = meshRenderer.transform.position;
            pos /= 100;
            pos.x = Mathf.Floor(pos.x);
            pos.y = Mathf.Floor(pos.y);
            pos.z = Mathf.Floor(pos.z);
            pos *= 100;

            LOD_Info lodinfo = new LOD_Info();
            lodinfo.pos = pos;
            lodinfo.lodlevel = lodlevel;
            lodinfo.rendr = meshRenderer;

            return lodinfo;
        }

    }
}