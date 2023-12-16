using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;
public class ShootingSystem : NetworkBehaviour
{
    [SerializeField] private Transform startPointTransform;
    [SerializeField] private List<Weapon_SO> weapon_SOs = new List<Weapon_SO>();
    [SerializeField] private float rangeModefier = 10;
    
    private int weaponIndex = 0;
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
            SpawnAndMovePrefabOnServerRpc(startPointTransform.position, Quaternion.identity);

            holdTime = 0;
        }
    }
    [ServerRpc]
    private void SpawnAndMovePrefabOnServerRpc(Vector3 pos, Quaternion rot)
    {
        GameObject InstantiatedObject = Instantiate(weapon_SOs[weaponIndex].projectilePrefab, pos, rot);
        NetworkObject networkObject = InstantiatedObject.GetComponent<NetworkObject>();
        networkObject?.Spawn();

        Move(networkObject.NetworkObjectId);
        Destroy(InstantiatedObject, 3f);
    }
    void Move(ulong id)
    {

        Transform cameraPosition = GetComponentInChildren<Camera>().transform;
        Vector3 moveDirection = (cameraPosition.up * weapon_SOs[weaponIndex].upModefier + cameraPosition.forward * weapon_SOs[weaponIndex].forwadModefier)
            * weapon_SOs[weaponIndex].range * rangeModefier * holdTime;

        GetNetworkObject(id)?.GetComponent<Rigidbody>().AddForce(moveDirection);
    }
}
