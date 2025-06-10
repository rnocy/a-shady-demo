using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class ShadowMovements : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float acceleration = 30f;
    [SerializeField] private float height = 1f;
    [SerializeField] private float currentSpeed = 0f;
    [SerializeField] private Transform viewDirection;
    [SerializeField] public bool isActive=true;
    private ShadowsPhysics shadowsPhysics;
    private float cosin = 0;
    private Rigidbody rigidbody;
    private PlayerInputs playerInputs;
    private void Awake()
    {
        //playerInputs = GetComponent<PlayerInputs>();
        rigidbody = GetComponent<Rigidbody>();
        playerInputs = new PlayerInputs();
        playerInputs.Default.Enable();
        playerInputs.Default.Jump.performed += Jump;
        //viewDirection = GetComponent<Transform>();
        shadowsPhysics = GetComponent<ShadowsPhysics>();
    }
    private void FixedUpdate()
    {
        if (isActive) Movement();
        viewDirection.rotation = shadowsPhysics.targetRotation;

    }
    private void Movement()
    {
        Vector2 inputVector = playerInputs.Default.WASD.ReadValue<Vector2>();
        Vector3 movement =
            //normalize sum of normalized horizontal view directions
            //shadow movements vector are swapped
            (NormalizeHorizontalVector(viewDirection.transform.right,viewDirection.up) * inputVector.y*0
            + NormalizeHorizontalVector(viewDirection.transform.forward,viewDirection.up) * inputVector.x).normalized;
        Vector3 horizontalMovement = HorizontalVector(rigidbody.velocity, viewDirection.up);
        Vector3 normalizedHorizontalMovement = Vector3.Normalize(horizontalMovement);
        cosin = Vector3.Dot(movement, normalizedHorizontalMovement);
        currentSpeed = horizontalMovement.magnitude;
        if (currentSpeed <= speed)
        {
            rigidbody.AddForce(movement * acceleration * (1 + Mathf.Clamp01(-cosin)), ForceMode.Force);
        }
        if ((currentSpeed > 0.01) && (movement.magnitude < 0.9))
        {
            rigidbody.AddForce(normalizedHorizontalMovement * (-1) * acceleration, ForceMode.Force);
        }
    }
    private void Jump(InputAction.CallbackContext context)
    {
        if (isActive) rigidbody.AddForce(viewDirection.up * height, ForceMode.Impulse);
        //Debug.Log(inputVector);
        //Debug.Log(context);

    }
    private Vector3 NormalizeHorizontalVector(Vector3 input, Vector3 normal)
    {
        return Vector3.Normalize(HorizontalVector(input,normal));
    }
    private Vector3 HorizontalVector(Vector3 input, Vector3 normal)
    {

        return Vector3.ProjectOnPlane(input,normal);
    }

}
