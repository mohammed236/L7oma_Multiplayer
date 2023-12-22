using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
public class PrefabsMoving : NetworkBehaviour
{
    [SerializeField] private float rangeModefier = 100;
    [SerializeField] private List<Weapon_SO> weapon_SOs = new List<Weapon_SO>();

    public override void OnNetworkSpawn()
    {
        Vector3 moveDirection = (transform.up * weapon_SOs[ShootingSystem.weaponIndex].upModefier + transform.forward * weapon_SOs[ShootingSystem.weaponIndex].forwadModefier)
            * weapon_SOs[ShootingSystem.weaponIndex].range * rangeModefier;

        GetComponent<Rigidbody>().AddForce(moveDirection);

    }
    private void OnCollisionEnter(Collision collision)
    {
        DestroyPrefabServerRpc();
    }
    [ServerRpc]
    private void DestroyPrefabServerRpc()
    {
        NetworkObject netObj = GetComponent<NetworkObject>();
        if (netObj.IsSpawned)
            netObj?.Despawn(true);
    }
}
