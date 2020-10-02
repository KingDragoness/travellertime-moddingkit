using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DestinyEngine;
using DestinyEngine.Object;
using TravellerTime;

namespace TestingTesting
{
    public class AutoTurretTest : Interactables
    {
        public bool toggleTurret = false;
        public float speed = 10f;
        public Transform target;
        public Transform outCannon;
        public GameObject muzzle;

        public float TurretFire = 0.2f;
        public float FireError = 0.04f;
        public float TurretDamage = 5;
        private float _currentCooldown = 0.2f;

        public override void CommandExecute(ActionCommand command)
        {
            if (command.commandID == "toggle")
            {
                toggleTurret = !toggleTurret;
            }
        }

        public override void LoadState()
        {

        }

        public override void SaveState()
        {

        }

        void Start()
        {
            target = DestinyMainEngine.main.ExamplePlayer.transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (toggleTurret)
            {
                _currentCooldown -= Time.deltaTime;

                if (_currentCooldown < 0)
                {
                    FireWeapon();
                    _currentCooldown = TurretFire + Random.Range(0, FireError);
                }

                Vector3 aimVector = target.position - transform.position;

                aimVector.y = 0f;

                Quaternion newRotation = Quaternion.LookRotation(aimVector, transform.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * speed * 0.1f);
            }
        }

        void FireWeapon()
        {
            Impact();
            muzzle.gameObject.SetActive(true);
        }

        private RaycastHit GetRaycastHit()
        {
            RaycastHit hit;

            if (Physics.Raycast(outCannon.position, outCannon.forward, out hit, 1000f, ~0, QueryTriggerInteraction.Ignore))
            {
                return hit;
            }
            else
            {
                return hit;
            }
        }

        private void Impact()
        {
            RaycastHit hit = GetRaycastHit();

            if (hit.collider == null)
            {
                return;
            }

            string impactName = "PFX_BImpact_Ground";

            if (hit.collider.sharedMaterial != null)
            {
                string physicsMaterialName = hit.collider.sharedMaterial.name;

                if (physicsMaterialName != "")
                {
                    if (physicsMaterialName == "Ground")
                    {
                        impactName = "PFX_BImpact_Ground";
                    }
                    else if (physicsMaterialName == "Concrete")
                    {
                        impactName = "PFX_BImpact_Concrete";
                    }
                    else if (physicsMaterialName == "Wood")
                    {
                        impactName = "PFX_BImpact_Wood";
                    }
                    else if (physicsMaterialName == "Metal")
                    {
                        impactName = "PFX_BImpact_Metal";
                    }
                }
            }

            FormID formID = new FormID();
            formID.BaseID = impactName;
            formID.DatabaseID = "vanilla";
            formID.ObjectType = ObjectData_Type.BaseWorldObject;

            GameObject particle = DestinyInternalCommand.instance.Spawnable_Create(formID);

            particle.transform.position = hit.point;
            particle.transform.up = -outCannon.forward;

            MonoBehaviour[] list = hit.collider.gameObject.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour mb in list)
            {
                if (mb is IDamageable)
                {
                    IDamageable damageable = (IDamageable)mb;
                    damageable.TriggerDamage(TurretDamage);
                }
            }
        }

        public override void ExecuteFunction(string functionName)
        {
            throw new System.NotImplementedException();
        }
    }
}