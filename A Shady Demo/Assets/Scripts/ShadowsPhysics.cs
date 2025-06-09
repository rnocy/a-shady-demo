using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowsPhysics : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rigidBody;
    public Transform controlRoot;
    [SerializeField] private Vector3 gravityVector;
    [SerializeField] private float gravityMultiplier=1;
    void Awake()
    {
        rigidBody=GetComponent<Rigidbody>();
        gravityVector=GetComponent<Transform>().up*-1;
        controlRoot= GetComponent<Transform>();
        controlRoot.rotation = Quaternion.LookRotation(GetComponent<Transform>().forward, GetComponent<Transform>().up);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Gravity();
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
            controlRoot.rotation = Quaternion.LookRotation(newForwardVector, -gravityVector);
            //Debug.Log(tempGravity);
            MoveShadow(other.transform);

        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<GravityModifier>(out GravityModifier gravityModifier))
        {
            Vector3 newForwardVector=Vector3.ProjectOnPlane(controlRoot.forward,other.transform.right).normalized;
            Vector3 newUpVector = Vector3.ProjectOnPlane(controlRoot.up, other.transform.right).normalized;
            Vector3 newRightVector = Vector3.Cross(newForwardVector, newUpVector);
            controlRoot.rotation = Quaternion.LookRotation(newForwardVector, newUpVector);

            MoveShadow(other.transform);
        }
    }
    private void MoveShadow(Transform other)
    {
        Vector3 reverseTargetVector = other.transform.position - controlRoot.position;
        Vector3 colliderRightHorizontalVector = NormalizeHorizontalVector(other.transform.right);
        float signedDistance = Vector3.Dot(colliderRightHorizontalVector, HorizontalVector(reverseTargetVector));
        Vector3 moveVector = colliderRightHorizontalVector * signedDistance;
        controlRoot.position += moveVector;
        Vector3 velocityVector = Vector3.ProjectOnPlane(rigidBody.velocity, other.transform.right);
        velocityVector = velocityVector.normalized*rigidBody.velocity.magnitude;
        rigidBody.velocity = velocityVector;
    }
    private Vector3 NormalizeHorizontalVector(Vector3 input)
    {
        return Vector3.Normalize(HorizontalVector(input));
    }
    private Vector3 HorizontalVector(Vector3 input)
    {
        return new Vector3(input.x, 0, input.z);
    }
}
