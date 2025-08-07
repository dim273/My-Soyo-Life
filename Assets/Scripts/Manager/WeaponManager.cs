using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : Singleton<WeaponManager>
{
    [Header("Config")]
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI weaponManaTMP;

    public void EquipWeapon(Weapon weapon)
    {
        weaponIcon.sprite = weapon.Icon;
        weaponIcon.SetNativeSize();
        weaponIcon.gameObject.SetActive(true);
        weaponManaTMP.text = weapon.RequireMana.ToString();
        weaponManaTMP.gameObject.SetActive(true);
        GameManager.instance.Player.PlayerAttack.EquipWeapon(weapon);
    }
}
