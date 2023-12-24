using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ObjectPoll : NetworkBehaviour
{
    public static ObjectPoll Instance;
    private void Awake()
    {
        Instance = this;
    }
    [SerializeField]
    private int pollAmount;
    [SerializeField]
    private GameObject objectToPoll;
    private List<GameObject> polledGameObjects;

    public void Start()
    {
        polledGameObjects = new List<GameObject>();
        if (!isLocalPlayer)
        {
            return;
        }
        PollOnServerRpc();
    }
    private void PollOnServerRpc()
    {
        Pool();
    }
    private void Pool()
    {

        GameObject temp;
        for (int i = 0; i < pollAmount; i++)
        {
            temp = Instantiate(objectToPoll);
            temp.SetActive(false);
            polledGameObjects.Add(temp);
        }
    }

    public GameObject GetPolledObject()
    {
        for (int i = 0; i < pollAmount; i++)
        {
            if (!polledGameObjects[i].activeInHierarchy)
            {
                return polledGameObjects[i];
            }
        }
        return null;
    }
}