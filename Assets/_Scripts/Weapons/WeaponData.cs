using UnityEngine;
public enum WeaponType { MainWeapon, SecundaryWeapon }
[CreateAssetMenu(menuName = "Weapon Data/New Weapon Data", fileName = "New Weapon Data")]
public class WeaponData : ScriptableObject
{
    //All Weapons
    public WeaponType weaponType;
    [HideInInspector] public float fireRate;
    [HideInInspector] public float damage;
    [HideInInspector] public int initialAmmo;
    [HideInInspector] public Sprite mainSprite;
    [HideInInspector] public Sprite selectedSprite;
    [HideInInspector] public string weaponSoundName;


    //FireWeapons
    [HideInInspector] public float bulletSpeed;

    //Knifes
    [HideInInspector] public float attackRange;
    [HideInInspector] public float knifeSpeed;
}