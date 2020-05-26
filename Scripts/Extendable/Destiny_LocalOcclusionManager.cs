using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestinyEngine
{
    [System.Serializable]
    public class OcclusionDat
    {
        public Destiny_OcclusionBox occlusionBox;
        public Bounds bound;
    }

    public class Destiny_LocalOcclusionManager : MonoBehaviour
    {
        public Transform exampleChar;
        public bool DEBUG_OccludeTest = false;

        [HideInNormalInspector] public List<OcclusionDat> m_AllOcclusionBounds = new List<OcclusionDat>();

        private void Start()
        {

        }

        private void Update()
        {
            if (DEBUG_OccludeTest)
            {
                Occluding();
            }
        }

        void Occluding()
        {
            List<Destiny_OcclusionBox> activatedBox = new List<Destiny_OcclusionBox>();

            foreach(OcclusionDat dat in m_AllOcclusionBounds)
            {
                if (!dat.bound.Contains(exampleChar.position) && !activatedBox.Find(x => x == dat.occlusionBox))
                {
                    dat.occlusionBox.OccludeObject();
                }
                else if (!activatedBox.Find(x => x == dat.occlusionBox))
                {
                    dat.occlusionBox.DeoccludeObject();
                    activatedBox.Add(dat.occlusionBox);
                }

                foreach(Destiny_OcclusionBox box in dat.occlusionBox.linkedBox)
                {
                    if (activatedBox.Find(x => x == box))
                        continue;

                    OcclusionDat targetDat = m_AllOcclusionBounds.Find(x => x.occlusionBox == box);

                    if (targetDat == null)
                        continue;

                    if (dat.bound.Contains(exampleChar.position))
                    {
                        targetDat.occlusionBox.DeoccludeObject();
                        activatedBox.Add(targetDat.occlusionBox);
                    }
                }
            }
        }

        private void OnDisable()
        {
            foreach (OcclusionDat dat in m_AllOcclusionBounds)
            {
                dat.occlusionBox.DeoccludeObject();
            }
        }

        public void Get_AllOcclusionBounds()
        {
            m_AllOcclusionBounds.Clear();
            Destiny_OcclusionBox[] occlusionBoxes = GetComponentsInChildren<Destiny_OcclusionBox>();

            foreach(Destiny_OcclusionBox box in occlusionBoxes)
            {
                Bounds bound = new Bounds(box.transform.position, box.transform.localScale);

                OcclusionDat occlusionDat = new OcclusionDat();
                occlusionDat.bound = bound;
                occlusionDat.occlusionBox = box;

                m_AllOcclusionBounds.Add(occlusionDat);
            }

            //Check all intersecting bounds

            foreach(OcclusionDat dat in m_AllOcclusionBounds)
            {
                dat.occlusionBox.linkedBox.Clear();

                foreach (OcclusionDat dat1 in m_AllOcclusionBounds)
                {
                    if (dat == dat1)
                        continue;

                    if (dat.bound.Intersects(dat1.bound))
                    {
                        dat.occlusionBox.linkedBox.Add(dat1.occlusionBox);
                    }
                }
            }
        }


    } 
}
