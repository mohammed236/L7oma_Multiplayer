using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;
public class ShootingSystem : NetworkBehaviour
{
    [SerializeField] private Transform startPointTransform;
    [SerializeField] private List<Weapon_SO> weapon_SOs = new List<Weapon_SO>();
    [SerializeField] private float rangeModefier = 100;
    
    public static int weaponIndex = 0;
    [Range(0f,1f)]
    private float holdTime = 0;
    
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
            SpawnAndMovePrefabOnServerRpc(startPointTransform.position, startPointTransform.rotation);

            holdTime = 0;
        }
    }
    [ServerRpc]
    private void SpawnAndMovePrefabOnServerRpc(Vector3 pos, Quaternion rot)
    {
        GameObject InstantiatedObject = Instantiate(weapon_SOs[weaponIndex].projectilePrefab, pos, rot);
        NetworkObject networkObject = InstantiatedObject.GetComponent<NetworkObject>();
        networkObject?.Spawn();

        if (networkObject.IsSpawned)
            Destroy(networkObject.gameObject, 3f);
    }
}
