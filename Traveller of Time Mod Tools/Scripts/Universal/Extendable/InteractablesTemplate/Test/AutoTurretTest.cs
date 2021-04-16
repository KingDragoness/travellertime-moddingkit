using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DestinyEngine;
using DestinyEngine.Object;
using TravellerTime;

namespace TestingTesting
{
    public class AutoTurretTest : InteractableScript
    {
        public bool toggleTurret = false;
        public bool allowYFree = true;
        public float speed = 10f;

        public GameObject muzzle;
        public AudioSource gunSound;

        [Header("Weapon system")]
        public Transform target;
        public Transform outCannon;
        public float AttackRange = 100f;
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

                Vector3 aimVector = target.position - transform.position;

                if (allowYFree == false)
                {
                    aimVector.y = 0f;
                }

                Quaternion newRotation = Quaternion.LookRotation(aimVector, transform.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * speed * 0.1f);
            }
        }

        #endregion

        #region Weapon
        private RaycastHit currentHit;

        void FireWeapon()
        {
            currentHit = GetRaycastHit();
            bool shouldFire = false;

            bool hasDamagableComponent = false;

            if (currentHit.collider == null)
            {
                return;
            }

            if (currentHit.distance >= AttackRange)
            {
                return;
            }

            if (currentHit.collider.CompareTag("Damagable") | currentHit.collider.CompareTag("Player"))
            {
                hasDamagableComponent = true;
            }

            if (hasDamagableComponent == false)
            {
                int damageChance = Random.Range(0, 19);

                if (damageChance < 17)
                {
                    return;
                }
            }

            Impact();
            muzzle.gameObject.SetActive(true);

        }

        private RaycastHit GetRaycastHit()
        {
            RaycastHit hit;

            if (Physics.Raycast(outCannon.position, outCannon.forward, out hit, 200f, ~0, QueryTriggerInteraction.Ignore))
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
            RaycastHit hit = currentHit;


            GameObject particle = DestinyInternalCommand.instance.ImpactBullet(outCannon, hit);

            if (particle == null)
            {
                return;
            }

            gunSound?.Play();
            particle.transform.position = hit.point;
            particle.transform.up = -outCannon.forward;
            DamageAnyNPC(hit);
        }

        void DamageAnyNPC(RaycastHit hit)
        {
            bool hasDamagableComponent = false;

            if (hit.collider.CompareTag("Damagable") | hit.collider.CompareTag("Player"))
            {
                hasDamagableComponent = true;
            }

            if (hasDamagableComponent == false)
            {
                return;
            }

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

        public override void CommandExecute(string functionName)
        {
            throw new System.NotImplementedException();
        }
    }
}