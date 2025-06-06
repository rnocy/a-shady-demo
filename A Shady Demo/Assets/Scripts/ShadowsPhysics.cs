using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowsPhysics : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rigidBody;
    [SerializeField] private Transform controlRoot;
    [SerializeField] private Vector3 gravityVector;
    [SerializeField] private float gravityMultiplier=1;
    void Awake()
    {
        rigidBody=GetComponent<Rigidbody>();
        gravityVector=GetComponent<Transform>().up*-1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Gravity();
        rigidBody.velocity=controlRoot.transform.forward;
    }
    private void Gravity()
    {
        rigidBody.AddForce(gravityVector*gravityMultiplier);
    }
    void OnTriggerStay(Collider other)
    {
        
        if (other.TryGetComponent<GravityModifier>(out GravityModifier gravityModifier))
        {
            Vector3 tempGravity=new Vector3(0,0,0);
            if (gravityModifier.isRadial) tempGravity=Vector3.Normalize(transform.position-other.transform.position);
            else tempGravity=gravityModifier.transform.up*-1;
            if (gravityModifier.isReversed) tempGravity*=-1;
            gravityVector=tempGravity;
            //Debug.Log(tempGravity);
        }        
    }
    void OnTriggerEnter(Collider other){
        if (other.TryGetComponent<DimensionWarper>(out DimensionWarper dimensionWarper)){
            Vector3 newForwardVector=-Vector3.Cross(controlRoot.up,other.transform.right);
            Vector3 newDownVector=-Vector3.Cross(controlRoot.forward,other.transform.right);
            controlRoot.rotation=Quaternion.LookRotation(newForwardVector,newDownVector);
        }
    }
}
