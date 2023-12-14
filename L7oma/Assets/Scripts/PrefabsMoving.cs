using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
public class PrefabsMoving : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        ShootingSystem.Instance.OnObjectSpawned += Move;
    }
    public void Move(Transform cameraPosition,List<Weapon_SO> weapon_SOs,int weaponIndex,float rangeModefier,float holdTime)
    {
        Vector3 moveDirection = (cameraPosition.up * weapon_SOs[weaponIndex].upModefier + cameraPosition.forward * weapon_SOs[weaponIndex].forwadModefier)
            * weapon_SOs[weaponIndex].range * rangeModefier * holdTime;

        MoveOnServerRpc(moveDirection);
    }
    [ServerRpc]
    private void MoveOnServerRpc(Vector3 moveDirection)
    {
        GetComponent<Rigidbody>().AddForce(moveDirection);
        ShootingSystem.Instance.OnObjectSpawned -= Move;
    }
}
