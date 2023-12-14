using Unity.Netcode;
using UnityEngine;

public class InteactionSystem : NetworkBehaviour
{
    [SerializeField] private Transform GrabPoint;
    [SerializeField] private LayerMask interactedLayer;
    [SerializeField] private float intercationDistance = 1f;

    private Transform camTransform;
    private IGrababale interactableObject;

    private void Start()
    {
        camTransform = GetComponentInChildren<Camera>().transform;
    }

    private bool IsThereInteraction()
    {
        var camPos = camTransform.position;
        Physics.Raycast(camPos, camTransform.forward, out RaycastHit hit,intercationDistance,interactedLayer);

        if (!hit.transform)
        {
            return false;
        }
        else if(hit.transform.TryGetComponent(out interactableObject))
        {
            return true;
        }

        return false;
    }
    private void Update()
    {
        if (!IsOwner) return;

        if (Grababale.canInteract) // make raycast stop searching for object when holding one - just for performance
        {
            if (IsThereInteraction())
            {
                //DisActivate the PRESS E to interacte button/image
                GameUI.Instance.DisplayInteraction();

                //Interacte if interacte button got pressed 
                if (InputsData.IsInteracting())
                {
                    //Interacte with Object
                    interactableObject?.Interacte(GrabPoint);
                }
            }
            else
            {
                //Activate the PRESS E to interacte button/image
                GameUI.Instance.HideInteraction();
            }
        }
        else
        {
            if (InputsData.IsInteracting())
            {
                //Disinteracte
                interactableObject?.Disinteracte();
            }
        }
    }
}
