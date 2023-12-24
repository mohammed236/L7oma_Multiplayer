using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PrefabsMoving : NetworkBehaviour
{
    [SerializeField] private List<Weapon_SO> weapon_SOs;
    [SerializeField] private float rangeModefier = 100;

    [ClientRpc]
    public void RpcMove(float holdTime,int weaponIndex)
    {
        Vector3 moveDirection = (transform.up * weapon_SOs[weaponIndex].upModefier + transform.forward * weapon_SOs[weaponIndex].forwadModefier)
            * weapon_SOs[weaponIndex].range * rangeModefier *holdTime;

        GetComponent<Rigidbody>()?.AddForce(moveDirection);
        if (gameObject != null)
            Destroy(gameObject, 3f);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (gameObject != null)
            Destroy(gameObject);
    }
}