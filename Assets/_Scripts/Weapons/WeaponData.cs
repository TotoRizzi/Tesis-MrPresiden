using UnityEngine;
using UnityEditor;
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


    //FireWeapons
    [HideInInspector] public float bulletSpeed;

    //Knifes
    [HideInInspector] public float attackRange;
    [HideInInspector] public float knifeSpeed;
}

[CustomEditor(typeof(WeaponData))]
public class WeaponDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginVertical();

        WeaponData weaponData = (WeaponData)target;

        EditorGUILayout.LabelField("Fire Rate");
        weaponData.fireRate = EditorGUILayout.FloatField(weaponData.fireRate, GUILayout.MaxWidth(50));

        EditorGUILayout.LabelField("Damage");
        weaponData.damage = EditorGUILayout.FloatField(weaponData.damage, GUILayout.MaxWidth(50));

        weaponData.mainSprite = (Sprite)EditorGUILayout.ObjectField("Main Sprite", weaponData.mainSprite, typeof(Sprite), false);

        weaponData.selectedSprite = (Sprite)EditorGUILayout.ObjectField("Selected Sprite", weaponData.selectedSprite, typeof(Sprite), false);

        if (weaponData.weaponType == WeaponType.MainWeapon) FireWeapons(weaponData);
        else Knifes(weaponData);

        EditorGUILayout.EndVertical();

        EditorUtility.SetDirty(weaponData);
    }

    void FireWeapons(WeaponData weaponData)
    {
        EditorGUILayout.LabelField("Bullet Speed");
        weaponData.bulletSpeed = EditorGUILayout.FloatField(weaponData.bulletSpeed, GUILayout.MaxWidth(50));

        EditorGUILayout.LabelField("Max Ammo");
        weaponData.initialAmmo = EditorGUILayout.IntField(weaponData.initialAmmo, GUILayout.MaxWidth(50));
    }
    void Knifes(WeaponData weaponData)
    {
        EditorGUILayout.LabelField("Attack Range");
        weaponData.attackRange = EditorGUILayout.FloatField(weaponData.attackRange, GUILayout.MaxWidth(50));

        EditorGUILayout.LabelField("Knife Speed");
        weaponData.knifeSpeed = EditorGUILayout.FloatField(weaponData.knifeSpeed, GUILayout.MaxWidth(50));
    }
}
