using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDataManager : MonoBehaviour
{
    [SerializeField] private bool Kinematic;
    [SerializeField] private bool Shadow;
    [SerializeField] private int timeCount=0;  //time index
    [SerializeField] private int kinematicTimeCount;
    [SerializeField] private int shadowTimeCount;
    private const int frameLimit = 36000; 
    private ShadowsPhysics shadowsPhysics;
    private List<KinematicData> kinematicData = new List<KinematicData>();
    private List<ShadowData> shadowData=new List<ShadowData>();
    private Rigidbody rigidbody;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        if (TryGetComponent<ShadowsPhysics>(out ShadowsPhysics shadowP)) {
            shadowsPhysics = shadowP;
        }
    }
    public void TimeManagement(bool isRewinding,int rewindSpeed)
    {
        if (isRewinding) RewindTime(rewindSpeed);
        else RecordTime();
    }
    private void RecordTime()
    {
        if (Kinematic) KinematicRecord();
        if (Shadow) ShadowRecord();
        timeCount++;
    }
    private void RewindTime(int rewindSpeed)
    {
        if (Kinematic) KinematicRewind(rewindSpeed);
        if (Shadow) ShadowRewind(rewindSpeed);
        timeCount =timeCount- rewindSpeed;

    }
    private void KinematicRecord()
    {
        if (kinematicData.Count > frameLimit) kinematicData.RemoveAt(0);
        kinematicData.Add(new KinematicData(transform.position, rigidbody.velocity, transform.rotation, rigidbody.angularVelocity));
        kinematicTimeCount = kinematicData.Count;
    }
    private void KinematicRewind(int rewindSpeed)
    {
        transform.position=kinematicData[kinematicTimeCount-rewindSpeed].Position;
        rigidbody.velocity = kinematicData[kinematicTimeCount - rewindSpeed].Velocity;
        transform.rotation = kinematicData[kinematicTimeCount - rewindSpeed].Rotation;
        rigidbody.angularVelocity = kinematicData[kinematicTimeCount - rewindSpeed].AngularVelocity;
        for (int i = 0; i < rewindSpeed; i++)
        {
            kinematicData.RemoveAt(kinematicData.Count - 1);
        }
        kinematicTimeCount = kinematicData.Count;
    }
    private void ShadowRecord()
    {
        if (shadowData.Count > frameLimit) shadowData.RemoveAt(0);
        shadowData.Add(new ShadowData(shadowsPhysics.gravityVector,shadowsPhysics.targetRotation));
        shadowTimeCount = shadowData.Count;
    }
    private void ShadowRewind(int rewindSpeed)
    {
        shadowsPhysics.gravityVector = shadowData[shadowData.Count - 1].gravityVector;
        shadowsPhysics.targetRotation = shadowData[shadowData.Count-1].targetRotation;
        for (int i = 0; i < rewindSpeed; i++)
        {
            shadowData.RemoveAt(shadowData.Count-1);
        }
        shadowTimeCount=shadowData.Count;
    }

    private class KinematicData
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public Quaternion Rotation;
        public Vector3 AngularVelocity;
        public KinematicData(Vector3 currentPosition, Vector3 currentVelocity, Quaternion currentRotation, Vector3 currentAngularVelocity)
        {
            Position = currentPosition;
            Velocity = currentVelocity;
            Rotation = currentRotation;
            AngularVelocity= currentAngularVelocity;
        }
    }
    private class ShadowData
    {
        public Vector3 gravityVector;
        public Quaternion targetRotation;
        public ShadowData(Vector3 currentGravity, Quaternion currentRotation)
        {
            gravityVector=currentGravity;
            targetRotation=currentRotation;
        }
    }
}