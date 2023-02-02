using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Data/New Weapon Data", fileName = "New Weapon Data")]
public class WeaponData : ScriptableObject
{
    public WeaponType weaponType;
    public float fireRate;
    public float damage;
    public float bulletSpeed;
    public Bullet bulletPrefab;
    public Sprite mainSprite;
    public Sprite selectedSprite;
}
