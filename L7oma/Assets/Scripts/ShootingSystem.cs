using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;
public class ShootingSystem : NetworkBehaviour
{
    public static ShootingSystem Instance { get; private set; }

    [SerializeField] private Transform startPointTransform;
    [SerializeField] private List<Weapon_SO> weapon_SOs = new List<Weapon_SO>();
    [SerializeField] private float rangeModefier = 10;
    
    private int weaponIndex = 0;
    [Range(0f,1f)]
    private float holdTime = 0;

    public event Action<Transform, List<Weapon_SO> , int , float , float > OnObjectSpawned;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
            Instance = this;
    }

    //shoot projectiles
    private void Shoot()
    {
        //spawn it on the network on the host and move it
        SpawnPrefabOnServerRpc(startPointTransform.position, Quaternion.identity);
    }   

    [ServerRpc]
    private void SpawnPrefabOnServerRpc(Vector3 pos,Quaternion rot)
    {
        GameObject InstantiatedObject = Instantiate(weapon_SOs[weaponIndex].projectilePrefab, pos, rot);

        NetworkObject networkObject = InstantiatedObject.GetComponent<NetworkObject>();
        networkObject.Spawn();

        Transform cameraPosition = GetComponentInChildren<Camera>().transform;

        OnObjectSpawned?.Invoke(cameraPosition, weapon_SOs, weaponIndex, rangeModefier, holdTime);

        Destroy(InstantiatedObject,3f);
    }
    private void Update()
    {
        if (!IsOwner) return;
        if (InputsData.IsShootingStarted())
        {
            //start counting the holding time
            if (holdTime <= 1)
                holdTime += Time.deltaTime;

        }else if (InputsData.IsShootingReleased())
        {
            //shoot
            Shoot();

            holdTime = 0;
        }
    }
}
