using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class ShootingSystem : NetworkBehaviour
{
    [SerializeField] private Transform startPointTransform;
    [SerializeField] private List<Weapon_SO> weapon_SOs = new List<Weapon_SO>();

    [Range(0f, 1f)]
    private float holdTime = 0;
    private int weaponIndex = 0;

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (InputsData.IsShootingStarted())
        {
            //start counting the holding time
            if (holdTime <= 1)
                holdTime += Time.deltaTime;
        }
        else if (InputsData.IsShootingReleased())
        {
            //shoot
            CmdSpawnPrefab(startPointTransform.position, startPointTransform.rotation,holdTime,weaponIndex);
            holdTime = 0;
        }

    }
    [Command]
    private void CmdSpawnPrefab(Vector3 pos, Quaternion rot,float holdtime,int weaponindex)
    {
        GameObject InstantiatedObject = Instantiate(weapon_SOs[weaponIndex].projectilePrefab, pos, rot);
        NetworkServer.Spawn(InstantiatedObject, this.gameObject);
        InstantiatedObject.GetComponent<PrefabsMoving>().RpcMove(holdtime, weaponindex);
    }
}