using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DestinyEngine;

public class debug_Weapon : MonoBehaviour
{
    public DefaultWeapon defaultWeapon;

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            defaultWeapon.Fire();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            defaultWeapon.FireUp();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            defaultWeapon.FireDown();
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            defaultWeapon.Reload();

            Weapon_Gun weapongun = defaultWeapon as Weapon_Gun;
            if (weapongun != null)
            {
                weapongun.MagazineCurrent = weapongun.MagazineCapacity;
            }
        }
    }


}
