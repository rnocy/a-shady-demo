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
    public bool isRewinding=false;
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
            //normalize horizontal view direction
            NormalizeHorizontalVector(viewDirection.transform.forward) * inputVector.y
            + NormalizeHorizontalVector(viewDirection.transform.right) * inputVector.x;
        GetComponent<Rigidbody>().AddForce(movement * speed, ForceMode.Force);
    }
    private void Jump(InputAction.CallbackContext context)
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(0,height,0)*speed,ForceMode.Impulse);
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
        return Vector3.Normalize(new Vector3(input.x, 0, input.z));
    }

}
