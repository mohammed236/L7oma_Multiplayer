using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;
public class ShootingSystem : NetworkBehaviour
{
    [SerializeField] private Transform startPointTransform;
    [SerializeField] private List<Weapon_SO> weapon_SOs = new List<Weapon_SO>();
    [SerializeField] private float rangeModefier = 100;

    public static int weaponIndex = 0;
    [Range(0f, 1f)]
    private float holdTime = 0;

    private GameObject InstantiatedObject;
    private NetworkIdentity networkIdentity;

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
            CmdSpawnAndMovePrefab(startPointTransform.position, startPointTransform.rotation);
            holdTime = 0;
        }

    }
    [Command]
    private void CmdSpawnAndMovePrefab(Vector3 pos, Quaternion rot)
    {
        InstantiatedObject = Instantiate(weapon_SOs[weaponIndex].projectilePrefab, pos, rot);
        NetworkServer.Spawn(InstantiatedObject, this.gameObject);
        networkIdentity = InstantiatedObject.GetComponent<NetworkIdentity>();
        //MoveClientRpc();
    }
    private void MoveClientRpc()
    {
        Transform cameraPosition = GetComponentInChildren<Camera>().transform;

        Vector3 moveDirection = (cameraPosition.up * weapon_SOs[weaponIndex].upModefier + cameraPosition.forward * weapon_SOs[weaponIndex].forwadModefier)
            * weapon_SOs[weaponIndex].range * rangeModefier * holdTime;

        networkIdentity.GetComponent<Rigidbody>().AddForce(moveDirection);

        Destroy(networkIdentity.gameObject, 3f);
    }
}