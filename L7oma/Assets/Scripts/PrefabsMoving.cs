using System.Collections.Generic;
using Mirror;
using UnityEngine;
public class PrefabsMoving : NetworkBehaviour
{
    [SerializeField] private List<Weapon_SO> weapon_SOs;
    [SerializeField] private float rangeModefier = 100;

    public void Start()
    {
        Move();
    }
    public void Move()
    {
        //Transform cameraPosition = connectionToClient.identity.gameObject.GetComponentInChildren<Camera>().transform;
         
        Vector3 moveDirection = (transform.up * weapon_SOs[ShootingSystem.weaponIndex].upModefier + transform.forward * weapon_SOs[ShootingSystem.weaponIndex].forwadModefier)
            * weapon_SOs[ShootingSystem.weaponIndex].range * rangeModefier;

        GetComponent<Rigidbody>()?.AddForce(moveDirection);
        if(gameObject!=null)
            Destroy(gameObject, 3f);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (gameObject != null)
            Destroy(gameObject);
    }
}
