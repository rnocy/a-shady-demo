using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField] private float speed=5f;
    [SerializeField] private float height = 5f;
    [SerializeField] private Transform viewDirection;
    private Rigidbody rigidbody;
    private PlayerInputs playerInputs;
    private void Awake()
    {
        //playerInputs = GetComponent<PlayerInputs>();
        rigidbody = GetComponent<Rigidbody>();
        playerInputs=new PlayerInputs();
        playerInputs.Default.Enable();
        playerInputs.Default.Jump.performed += Jump;
    }
    private void Update()
    {
        Movement();

    }
    private void Movement()
    {
        Vector2 inputVector = playerInputs.Default.WASD.ReadValue<Vector2>();
        Vector3 movement =
            //normalize horizontal view direction
            NormalizeHorizontalVector(viewDirection.transform.forward) * inputVector.y
            + NormalizeHorizontalVector(viewDirection.transform.right) * inputVector.x;
        //gotta do something here so the movement is smooth, Idk why
        Debug.Log("");
        rigidbody.AddForce(movement * speed, ForceMode.Force);
    }
    private void Jump(InputAction.CallbackContext context)
    {
        rigidbody.AddForce(new Vector3(0,height,0)*speed,ForceMode.Impulse);
        //Debug.Log(inputVector);

    }
    private Vector3 NormalizeHorizontalVector(Vector3 input)
    {
        return Vector3.Normalize(new Vector3(input.x, 0, input.z));
    }

}
