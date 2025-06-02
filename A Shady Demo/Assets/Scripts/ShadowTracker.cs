using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.Utility;

public class ShadowTracker : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private CinemachineSmoothPath path;
    [SerializeField] private CinemachineDollyCart cart;
    private int searchRadius = -1;
    private int searchResolution = 10;
    // Start is called before the first frame update
    private void Awake()
    {
        path = GetComponentInChildren<CinemachineSmoothPath>();
        cart = GetComponentInChildren<CinemachineDollyCart>();
        target=GameObject.Find("SHADOW").GetComponent<Transform>();
        MoveCart();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        MoveCart();
        MoveShadow();
    }
    private void MoveCart()
    {
        float closetPoint = path.FindClosestPoint(target.position, 0, searchRadius, searchResolution);
        cart.m_Position=closetPoint;
    }
    private void MoveShadow()
    {
        Vector3 reverseTargetVector= cart.transform.position-target.position;
        Vector3 cartRightHorizontalVector = NormalizeHorizontalVector(cart.transform.right);
        float signedDistance=Vector3.Dot(cartRightHorizontalVector,HorizontalVector(reverseTargetVector));
        Vector3 moveVector = cartRightHorizontalVector * signedDistance;
        target.position += moveVector;
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
