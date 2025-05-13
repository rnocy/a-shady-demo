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
            viewDirection.transform.forward * inputVector.y
            + viewDirection.transform.right * inputVector.x;
        rigidbody.AddForce(movement * speed, ForceMode.Force);
    }
    private void Jump(InputAction.CallbackContext context)
    {
        rigidbody.AddForce(new Vector3(0,height,0)*speed,ForceMode.Impulse);
        //Debug.Log(inputVector);

    }

}
