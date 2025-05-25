using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField] private float speed=5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float height = 5f;
    [SerializeField] private float currentSpeed = 0f;
    [SerializeField] private Transform viewDirection;
    public bool isRewinding = false;
    private float cosin = 0;
    private Rigidbody rigidbody;
    private PlayerInputs playerInputs;
    private void Awake()
    {
        //playerInputs = GetComponent<PlayerInputs>();
        rigidbody = GetComponent<Rigidbody>();
        playerInputs=new PlayerInputs();
        playerInputs.Default.Enable();
        playerInputs.Default.Jump.performed += Jump;
        playerInputs.Default.Rewind.performed += Rewind;
    }
    private void Update()
    {
        Movement();

    }
    private void Movement()
    {
        Vector2 inputVector = playerInputs.Default.WASD.ReadValue<Vector2>();
        Vector3 movement =
            //normalize sum of normalized horizontal view directions
            (NormalizeHorizontalVector(viewDirection.transform.forward) * inputVector.y
            + NormalizeHorizontalVector(viewDirection.transform.right) * inputVector.x).normalized;
        Vector3 horizontalMovement=HorizontalVector(rigidbody.velocity);
        Vector3 normalizedHorizontalMovement=Vector3.Normalize(horizontalMovement);
        cosin=Vector3.Dot(movement, normalizedHorizontalMovement);
        currentSpeed= horizontalMovement.magnitude;
        if (currentSpeed <= speed)
        {
            rigidbody.AddForce(movement * acceleration*(1+Mathf.Clamp01(-cosin)), ForceMode.Force);
        }
        if ((currentSpeed > 0.1) && (movement.magnitude < 0.9)) 
        {
            rigidbody.AddForce(normalizedHorizontalMovement * (-1) * acceleration, ForceMode.Force);
        }
    }
    private void Jump(InputAction.CallbackContext context)
    {
        rigidbody.AddForce(new Vector3(0,height,0)*speed,ForceMode.Impulse);
        //Debug.Log(inputVector);
        //Debug.Log(context);

    }
    private void Rewind(InputAction.CallbackContext context)
    {
        isRewinding = (playerInputs.Default.Rewind.ReadValue<float>()>0);
        //Debug.Log(playerInputs.Default.Rewind.ReadValue<float>());
    }
    private Vector3 NormalizeHorizontalVector(Vector3 input)
    {
        return Vector3.Normalize(HorizontalVector(input));
    }
    private Vector3 HorizontalVector(Vector3 input)
    {
        return new Vector3(input.x,0,input.z);
    }

}
