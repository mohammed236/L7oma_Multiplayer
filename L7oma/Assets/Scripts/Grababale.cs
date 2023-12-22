using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grababale : NetworkBehaviour, IGrababale
{
    private Rigidbody rb;
    private Transform grabPointTransform = null;
    public static bool canInteract = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Interacte(Transform grabPoint)
    {
        if (canInteract)
        {
            this.grabPointTransform = grabPoint;
            rb.useGravity = false;
            canInteract = false;
        }
    }
    public void Disinteracte()
    {
        grabPointTransform = null;
        rb.useGravity = true;
        canInteract = true;
    }
    [ServerCallback]
    private void SetObjectInPoint(Vector3 grabPoint)
    {
        float lerpTime = 10f;
        Vector3 lerpedPos = Vector3.Lerp(transform.position, grabPoint, lerpTime * Time.deltaTime);
        rb.MovePosition(lerpedPos);
    }

    private void Update()
    {
        if (grabPointTransform != null)
        {
            SetObjectInPoint(grabPointTransform.position);
        }
    }
}
