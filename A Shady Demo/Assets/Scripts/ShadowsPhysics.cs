using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowsPhysics : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rigidBody;
    public Transform controlRoot;
    public Vector3 gravityVector;
    [SerializeField] private float gravityMultiplier=1;
    public Quaternion targetRotation;
    private Vector3 targetPosition;
    [SerializeField] private Transform debugger;
    void Awake()
    {
        rigidBody=GetComponent<Rigidbody>();
        Vector3 startingUp = GetComponent<Transform>().up*-1;
        gravityVector =new Vector3 (startingUp.x,startingUp.y,startingUp.z);
        controlRoot= GetComponent<Transform>();
        Quaternion startingRotation = Quaternion.LookRotation(GetComponent<Transform>().forward, GetComponent<Transform>().up);
        controlRoot.rotation = startingRotation;
        targetRotation=startingRotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        const float smoothing = 0.5f;
        controlRoot.rotation =Quaternion.Lerp(controlRoot.rotation,targetRotation,0.05f);
        controlRoot.position +=targetPosition*smoothing;
        targetPosition -= targetPosition * smoothing;
        rigidBody.position = controlRoot.position;
        rigidBody.rotation = controlRoot.rotation;
        Gravity();
        debugger.rotation=Quaternion.LookRotation(gravityVector,Vector3.up);
    }
    private void Gravity()
    {
        rigidBody.AddForce(gravityVector*gravityMultiplier);
    }
    void OnTriggerStay(Collider other)
    {

        if (other.TryGetComponent<GravityModifier>(out GravityModifier gravityModifier))
        {
            Vector3 tempGravity = new Vector3(0, 0, 0);
            if (gravityModifier.isRadial) tempGravity = Vector3.Normalize(transform.position - other.transform.position);
            else tempGravity = gravityModifier.transform.up * -1;
            if (gravityModifier.isReversed) tempGravity *= -1;
            gravityVector = tempGravity;
            Vector3 newForwardVector = -Vector3.Cross(other.transform.right, gravityVector).normalized;
            targetRotation = Quaternion.LookRotation(newForwardVector, -gravityVector);
            //Debug.Log(tempGravity);
            MoveShadow(other.transform);
        }
        MoveShadow(other.transform);
    }
    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<GravityModifier>(out GravityModifier gravityModifier))
        {
            Vector3 newForwardVector=Vector3.ProjectOnPlane(controlRoot.forward,other.transform.right).normalized;
            Vector3 newUpVector = Vector3.ProjectOnPlane(controlRoot.up, other.transform.right).normalized;
            targetRotation = Quaternion.LookRotation(newForwardVector, newUpVector);
            MoveShadow(other.transform);
            ConvertVelocityAcrossDimensions(other.transform.right);
        }
    }
    private void MoveShadow(Transform other)
    {
        Vector3 reverseTargetVector = other.transform.position - controlRoot.position;
        Vector3 moveVector =HorizontalVector(reverseTargetVector,other.transform.right);
        targetPosition = moveVector;
        ConvertGravity(other.right);
    }
    private void ConvertGravity(Vector3 normal)
    {
        Vector3 tempGravity = Vector3.ProjectOnPlane(gravityVector, normal).normalized;
        gravityVector = tempGravity;
    }
    private void ConvertVelocityAcrossDimensions(Vector3 normal)
    {
        Vector3 horizontalVelocity = HorizontalVector(rigidBody.velocity, controlRoot.forward);
        Vector3 verticalVelocity = HorizontalVector(rigidBody.velocity, gravityVector);
        Vector3 newHorizontalVelocity= NormalizeHorizontalVector(horizontalVelocity, targetRotation*Vector3.forward)*horizontalVelocity.magnitude;
        //Vector3 newVerticalVelocity=NormalizeHorizontalVector(rigidBody.velocity, -gravityVector)*verticalVelocity.magnitude;
        Vector3 newVelocityVector=newHorizontalVelocity+verticalVelocity;
        rigidBody.velocity = newVelocityVector;
    }
    
    private Vector3 NormalizeHorizontalVector(Vector3 input, Vector3 normal)
    {
        return Vector3.Normalize(HorizontalVector(input,normal));
    }
    private Vector3 HorizontalVector(Vector3 input, Vector3 normal)
    {
        return Vector3.Project(input,normal);
    }
}
