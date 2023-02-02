using UnityEngine;
using UnityEditor;
public enum WeaponType { FireWeapon, Knife, Granade }
[CreateAssetMenu(menuName = "Weapon Data/New Weapon Data", fileName = "New Weapon Data")]
public class WeaponData : ScriptableObject
{
    //All Weapons
    public WeaponType weaponType;
    [HideInInspector] public float damage;
    [HideInInspector] public Sprite mainSprite;
    [HideInInspector] public Sprite selectedSprite;


    //FireWeapons
    [HideInInspector] public float fireRate;
    [HideInInspector] public float bulletSpeed;
    [HideInInspector] public Bullet bulletPrefab;

    //Granades
    [HideInInspector] public float throwForce = 10;
    [HideInInspector] public float explosionForce = 600;
    [HideInInspector] public float explosionRadius = 3;
}

[CustomEditor(typeof(WeaponData))]
public class WeaponDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginVertical();

        WeaponData weaponData = (WeaponData)target;

        EditorGUILayout.LabelField("Damage");
        weaponData.damage = EditorGUILayout.FloatField(weaponData.damage, GUILayout.MaxWidth(50));

        weaponData.mainSprite = (Sprite)EditorGUILayout.ObjectField("Main Sprite", weaponData.mainSprite, typeof(Sprite), false);

        weaponData.selectedSprite = (Sprite)EditorGUILayout.ObjectField("Selected Sprite", weaponData.selectedSprite, typeof(Sprite), false);

        if (weaponData.weaponType == WeaponType.FireWeapon) FireWeapons(weaponData);
        else if (weaponData.weaponType == WeaponType.Granade) Granades(weaponData);

        EditorGUILayout.EndVertical();
    }

    void FireWeapons(WeaponData weaponData)
    {
        EditorGUILayout.LabelField("Fire Rate");
        weaponData.fireRate = EditorGUILayout.FloatField(weaponData.fireRate, GUILayout.MaxWidth(50));

        EditorGUILayout.LabelField("Bullet Speed");
        weaponData.bulletSpeed = EditorGUILayout.FloatField(weaponData.bulletSpeed, GUILayout.MaxWidth(50));

        weaponData.bulletPrefab = (Bullet)EditorGUILayout.ObjectField("Bullet Prefab", weaponData.bulletPrefab, typeof(Bullet), false);
    }
    void Granades(WeaponData weaponData)
    {
        EditorGUILayout.LabelField("Throw Force");
        weaponData.throwForce = EditorGUILayout.FloatField(weaponData.throwForce, GUILayout.MaxWidth(50));

        EditorGUILayout.LabelField("Explosion Force");
        weaponData.explosionForce = EditorGUILayout.FloatField(weaponData.explosionForce, GUILayout.MaxWidth(50));

        EditorGUILayout.LabelField("Explosion Radius");
        weaponData.explosionRadius = EditorGUILayout.FloatField(weaponData.explosionRadius, GUILayout.MaxWidth(50));
    }
}
