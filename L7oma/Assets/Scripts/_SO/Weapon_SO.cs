using UnityEngine;
[CreateAssetMenu(menuName = "_SO/Weapons")]
public class Weapon_SO : ScriptableObject
{
    [Range(0f, 1f)]
    public float upModefier = .1f;
    [Range(0f, 1f)]
    public float forwadModefier = 1f;

    public float range = 10f;
    public float fireRate = .5f;

    public new string name;
    public Sprite sprite;
    public GameObject prefab;
    public GameObject projectilePrefab;
}
