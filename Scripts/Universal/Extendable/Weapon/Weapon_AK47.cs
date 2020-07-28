using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TravellerTime;
using DestinyEngine;
using DestinyEngine.Object;

namespace TravellerTime.Vanilla
{

    public class Weapon_AK47 : DefaultWeapon, Weapon_Gun
    {
        public int magazineCurrent = 30;
        public int magazineCapacity = 30;

        public int MagazineCurrent { get { return magazineCurrent; } set { magazineCurrent = value; } }
        public int MagazineCapacity { get { return magazineCapacity; } set { magazineCapacity = value; } }

        private bool is_Reloading = false;
        public bool Is_Reloading { get { return is_Reloading; } set { is_Reloading = value; } }

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

        public override void Fire()
        {
            if (Is_Cooldown)
            {
                return;
            }
            else if (MagazineCurrent <= 0)
            {
                WeaponAnimator.SetBool("Fire", false);
                return;
            }

            Is_Cooldown = true;
            if (MagazineCurrent > 0)
                MagazineCurrent -= 1;

            WeaponAnimator.SetBool("Fire", true);

            SaveFlag();
            Impact();

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