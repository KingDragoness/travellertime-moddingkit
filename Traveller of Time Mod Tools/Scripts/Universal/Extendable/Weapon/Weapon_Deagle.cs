﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TravellerTime;
using DestinyEngine.Object;
using DestinyEngine;

namespace TravellerTime.Vanilla {

    public class Weapon_Deagle : DefaultWeapon, Weapon_Gun
    {

        public int magazineCurrent = 10;
        public int magazineCapacity = 10;
        public float deagle_cooldown = 1f;

        public int MagazineCurrent { get { return magazineCurrent;  } set { magazineCurrent = value; } }
        public int MagazineCapacity { get { return magazineCapacity; } set { magazineCapacity = value; } }
        public float cooldownFire { get { return deagle_cooldown; } set { deagle_cooldown = value; } }
        public bool Is_Reloading { get { return is_Reloading; } set { is_Reloading = value; } }


        private bool is_Reloading = false;

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

        private void Update()
        {
            if (DestinyMainEngine.main.ExamplePlayer.IsPlayerWalking() | DestinyMainEngine.main.ExamplePlayer.IsPlayerSprinting())
            {
                WeaponAnimator.SetFloat("Moving", 1f);

            }
            else
            {
                WeaponAnimator.SetFloat("Moving", 0f);

            }
        }

        public override void Fire()
        {

        }

        public override void FireDown()
        {

        }

        public override void FireUp()
        {
            if (Is_Cooldown | MagazineCurrent <= 0 | Is_Reloading)
            {
                return;
            }

            Is_Cooldown = true;
            if (MagazineCurrent > 0)
                MagazineCurrent -= 1;

            WeaponAnimator.SetTrigger("Fire");

            SaveFlag();
            Impact();
            Recoil();
        }

        public void Recoil()
        {
            DestinyInternalCommand.instance.Camera_Recoil(2f);
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

        public override void Reload()
        {

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

    }
}