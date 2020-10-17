using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TravellerTime;
using DestinyEngine;
using DestinyEngine.Object;

namespace TravellerTime.Vanilla
{

    public class Weapon_AK47 : WeaponScript, Weapon_Gun
    {
        public int magazineCurrent = 30;
        public int magazineCapacity = 30;
        public float ak47_cooldown = 0.15f;

        public int MagazineCurrent { get { return magazineCurrent; } set { magazineCurrent = value; } }
        public int MagazineCapacity { get { return magazineCapacity; } set { magazineCapacity = value; } }
        public float cooldownFire { get { return ak47_cooldown; } set { ak47_cooldown = value; } }
        public bool Is_Reloading { get { return isReloading; } set { isReloading = value; } }


        private bool isReloading = false;
        private bool isSprinting = false;

        private float internalCooldownTimer = 0.15f;

        public override void Initialize_Weapon()
        {
            if (ConnectedItemData.flagData.ContainsKey("CurrentAmmo"))
            {
                int i = 0;
                if (int.TryParse(ConnectedItemData.flagData["CurrentAmmo"], out i))
                {
                    magazineCurrent = int.Parse(ConnectedItemData.flagData["CurrentAmmo"]);
                }
            }
            base.Initialize_Weapon();
        }

        public override void SaveScriptAsJSON()
        {
            //Implement something
        }
        private void Update()
        {
            if (DestinyMainEngine.main.ExamplePlayer.IsPlayerWalking())
            {
                WeaponAnimator.SetFloat("Moving", 1f);

            }
            else
            {
                WeaponAnimator.SetFloat("Moving", 0f);

            }

            if (DestinyMainEngine.main.ExamplePlayer.IsPlayerSprinting())
            {
                isSprinting = true;

            }
            else
            {
                isSprinting = false;

            }

            if (isSprinting)
            {
                WeaponAnimator.SetBool("isSprinting", true);

            }
            else
            {
                WeaponAnimator.SetBool("isSprinting", false);

            }

            if (Is_Cooldown)
            {
                internalCooldownTimer -= 1 * Time.deltaTime;

                if (internalCooldownTimer < 0)
                {
                    Is_Cooldown = false;
                }

            }
        }

        public override void Fire()
        {
            if (isSprinting)
            {
                return;
            }
            if (Is_Reloading)
            {
                return;
            }
            if (Is_Cooldown)
            {
                return;
            }
            else if (MagazineCurrent <= 0)
            {
                WeaponAnimator.SetBool("Fire", false);
                return;
            }

            internalCooldownTimer = cooldownFire;
            Is_Cooldown = true;
            if (MagazineCurrent > 0)
                MagazineCurrent -= 1;

            WeaponAnimator.SetBool("Fire", true);

            SaveFlag();
            Impact();
            Recoil();
        }

        public void Recoil()
        {
            DestinyInternalCommand.instance.Camera_Recoil(1.2f);
        }

        private void SaveFlag()
        {
            if (!ConnectedItemData.flagData.ContainsKey("CurrentAmmo"))
            {
                ConnectedItemData.flagData.Add("CurrentAmmo", magazineCurrent.ToString());
            }
            else
            {
                ConnectedItemData.flagData["CurrentAmmo"] = magazineCurrent.ToString();
            }
        }

        public override void FireDown()
        {

        }

        public override void FireUp()
        {
        }

        public override void Inaction()
        {
            WeaponAnimator.SetBool("Fire", false);
        }

        public override void Reload()
        {
            if (Is_Reloading)
            {
                return;
            }
            if (MagazineCurrent >= MagazineCapacity)
            {
                return;
            }
            else if (ItemDataAmmo != null)
            {
                if (ItemDataAmmo.count == 0)
                {
                    return;
                }
            }
            else if (ItemDataAmmo == null)
            {

            }
            Is_Reloading = true;

            WeaponAnimator.SetTrigger("Reload");
        }

        public void Set_ReloadOff()
        {
            Is_Reloading = false;

            if (ItemDataAmmo != null)
            {
                int ammoToFill = MagazineCapacity - MagazineCurrent;

                if (ammoToFill > ItemDataAmmo.count)
                {
                    ammoToFill = ItemDataAmmo.count;
                    ItemDataAmmo.count -= ItemDataAmmo.count;
                }
                else
                {
                    ItemDataAmmo.count -= ammoToFill;
                }

                MagazineCurrent += ammoToFill;
            }

            Is_Cooldown = false;
            DestinyInternalCommand.instance.GameUI_RefreshAmmoCounter();
            SaveFlag();

        }

        public override void CommandExecute(string functionName)
        {
            throw new System.NotImplementedException();
        }
    }
}