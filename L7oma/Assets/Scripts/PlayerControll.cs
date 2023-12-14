using Unity.Netcode;
using UnityEngine;

public class PlayerControll : NetworkBehaviour
{
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float lookSpeed = 2f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float minLookLimite = -70f;
    [SerializeField] private float maxLookLimite = 80f;

    private new Rigidbody rigidbody;
    private Camera cameraTransform;

    private Vector2 movementInput;
    private Vector2 turnInput;

    private float Y;
    private float X;
    private bool isGrounded = false;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        cameraTransform = GetComponentInChildren<Camera>();

        if (IsOwner)
        {
            // Enable the camera for the local player
            cameraTransform.enabled = true;
            cameraTransform.GetComponent<AudioListener>().enabled = true;
            
            try
            {
                GameObject camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().gameObject;
                DestroyImmediate(camera);
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
        else
        {
            // Disable the camera for remote players
            cameraTransform.enabled = false;
            cameraTransform.GetComponent<AudioListener>().enabled = false;
        }
    }

    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        ReadInput();
        Look();
        Jump();
    }

    void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }

        MovePlayer();
    }

    void ReadInput()
    {
        movementInput = InputsData.MoveDirection();
        turnInput = InputsData.TurnDirection();
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.15f,groundLayer);
    }

    void MovePlayer()
    {
        Vector3 direction = new Vector3(movementInput.x, 0, movementInput.y);
        rigidbody.velocity = (moveSpeed * Time.deltaTime * direction.x * transform.right + moveSpeed * Time.deltaTime * direction.z * transform.forward) + new Vector3(0, rigidbody.velocity.y, 0);
    }
    private void Look()
    {
        Vector3 direction = new Vector3(turnInput.x, 0, turnInput.y);
        Y += lookSpeed * Time.deltaTime * direction.x;
        X += lookSpeed * Time.deltaTime * direction.z;

        transform.localRotation = Quaternion.Euler(0f, Y, 0f);

        X = Mathf.Clamp(X, minLookLimite, maxLookLimite); // Constrain within the desired range.
        cameraTransform.transform.localRotation = Quaternion.Euler(-X, 0f, 0f);
    }

    void Jump()
    {
        if (isGrounded && InputsData.IsJumping())
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}