﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TravellerTime;
using DestinyEngine;
using DestinyEngine.Object;

namespace TravellerTime.Vanilla
{

    [RequireComponent(typeof(WeaponComponentAnimation))]
    public class Weapon_AK47 : WeaponScript, Weapon_Gun
    {
        public int magazineCurrent = 30;
        public int magazineCapacity = 30;
        public float ak47_cooldown = 0.15f;
        public AudioSource gunFireSource;
        public AudioSource noAmmoSource;

        public int MagazineCurrent { get { return magazineCurrent; } set { magazineCurrent = value; } }
        public int MagazineCapacity { get { return magazineCapacity; } set { magazineCapacity = value; } }
        public float CooldownFire { get { return ak47_cooldown; } set { ak47_cooldown = value; } }
        public bool IsReloading { get { return isReloading; } set { isReloading = value; } }


        private bool isReloading = false;
        private bool isSprinting = false;
        private WeaponComponentAnimation componentAnimation;

        private float internalCooldownTimer = 0.15f;

        private void Awake()
        {
            componentAnimation = GetComponent<WeaponComponentAnimation>();
        }

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
            if (DestinyEngineController.ExamplePlayer.IsPlayerWalking())
            {
                componentAnimation.WeaponAnimator.SetFloat("Moving", 1f);

            }
            else
            {
                componentAnimation.WeaponAnimator.SetFloat("Moving", 0f);

            }

            if (DestinyEngineController.ExamplePlayer.IsPlayerSprinting())
            {
                isSprinting = true;

            }
            else
            {
                isSprinting = false;

            }

            if (isSprinting)
            {
                componentAnimation.WeaponAnimator.SetBool("isSprinting", true);

            }
            else
            {
                componentAnimation.WeaponAnimator.SetBool("isSprinting", false);

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

        private bool doFire = false;

        public override void Fire()
        {

            if (doFire == false)
            {
                return;
            }
            if (isSprinting)
            {
                return;
            }
            if (IsReloading)
            {
                return;
            }
            if (Is_Cooldown)
            {
                return;
            }
            else if (MagazineCurrent <= 0)
            {
                componentAnimation.WeaponAnimator.SetBool("Fire", false);

                if (noAmmoSource != null)
                {
                    if (noAmmoSource.isPlaying == false)
                    {
                        noAmmoSource.Play();
                    }

                }
                return;
            }

            internalCooldownTimer = CooldownFire;
            Is_Cooldown = true;
            if (MagazineCurrent > 0)
                MagazineCurrent -= 1;

            componentAnimation.WeaponAnimator.SetBool("Fire", true);
            gunFireSource.pitch = 1 + Random.Range(-0.01f, 0.01f);
            gunFireSource.PlayOneShot(gunFireSource.clip, gunFireSource.volume);

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
            doFire = true;
        }

        public override void FireUp()
        {
            doFire = false;
        }

        public override void Inaction()
        {
            componentAnimation.WeaponAnimator.SetBool("Fire", false);
        }

        public override void Reload()
        {
            if (IsReloading)
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
            IsReloading = true;

            componentAnimation.WeaponAnimator.SetTrigger("Reload");
        }

        public void Set_ReloadOff()
        {
            IsReloading = false;

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

        public override void IdleReady()
        {

        }
    }
}