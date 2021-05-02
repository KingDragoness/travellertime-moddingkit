using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DestinyEngine;
using DestinyEngine.Object;

namespace TestingTesting
{

    public class NPC_TurretGunTest : DestinyScript, ICommand
    {
        public ActorScript actorScript;

        [Header("Turret System")]
        public bool toggleTurret = false;
        public Transform Gun_Cannon;
        public Transform Gun_Base;

        public Transform target;
        public Transform outCannon;
        public float speed = 10f;
        public float speedCannon = 10f;

        public float TurretFire = 0.2f;
        public float FireError = 0.04f;
        public float TurretDamage = 5;
        private float _currentCooldown = 0.2f;

        // Start is called before the first frame update
        void Start()
        {
            target = DestinyEngineController.ExamplePlayer.transform;
        }

        // Update is called once per frame
        void Update()
        {
            Update_Turret();
        }

        #region Generic Weapon Fire module
        private void Update_Turret()
        {
            if (toggleTurret)
            {
                _currentCooldown -= Time.deltaTime;

                if (_currentCooldown < 0)
                {
                    FireWeapon();
                    _currentCooldown = TurretFire + Random.Range(0, FireError);
                }

                Vector3 aimVector = target.position - Gun_Base.position;
                aimVector.y = 0f;
                Quaternion newRotation = Quaternion.LookRotation(aimVector, Gun_Base.up);
                Gun_Base.rotation = Quaternion.Slerp(Gun_Base.rotation, newRotation, Time.deltaTime * speed * 0.1f);

                Vector3 aimVector1 = target.position - Gun_Cannon.position;
                Quaternion rot = Quaternion.Slerp(Gun_Cannon.rotation, Quaternion.LookRotation(aimVector1), speedCannon * Time.deltaTime);

                Gun_Cannon.rotation = rot;

                // put 0 on the axys you do not want for the rotation object to rotate
                Gun_Cannon.eulerAngles = new Vector3(Gun_Cannon.eulerAngles.x, Gun_Cannon.eulerAngles.y, Gun_Cannon.eulerAngles.z);
                Gun_Cannon.localEulerAngles = new Vector3(Gun_Cannon.localEulerAngles.x, 0, 0);
            }
        }

        void FireWeapon()
        {
            Impact();
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

            GameObject particle = DestinyInternalCommand.instance.ImpactBullet(outCannon, hit);

            #region Obselete
            /*
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
            */
            #endregion

            if (particle == null)
            {
                return;
            }

            particle.transform.position = hit.point;
            particle.transform.up = -outCannon.forward;
            DamageAnyNPC(hit);
        }

        void DamageAnyNPC(RaycastHit hit)
        {
            MonoBehaviour[] list = hit.collider.gameObject.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour mb in list)
            {
                if (mb is IDamageable)
                {
                    IDamageable damageable = (IDamageable)mb;
                    Damageable_Token token = new Damageable_Token();
                    token.baseDamage = TurretDamage;
                    token.damageOrigin = Damageable_Token.DamageOrigin.Player;

                    damageable.TriggerDamage(token);
                }
            }
        }
        #endregion



        public List<ActionCommand_Parent> CommandListRetrieveAll()
        {
            throw new System.NotImplementedException();
        }

        public void CommandExecute(ActionCommand command)
        {
            throw new System.NotImplementedException();
        }

        public void CommandExecute(string functionName)
        {
            throw new System.NotImplementedException();
        }

        public bool ConditionalCheck(string conditionName)
        {
            throw new System.NotImplementedException();
        }

        public void InsertNewCommand(ActionCommand command)
        {
            throw new System.NotImplementedException();
        }
    }

}