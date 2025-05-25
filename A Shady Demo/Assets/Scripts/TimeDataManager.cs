using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDataManager : MonoBehaviour
{
    [SerializeField] private bool Kinematic;
    private List<KinematicData> kinematicData=new List<KinematicData>();
    public int timeCount=0;
    public int kinematicTimeCount;
    private int frameLimit = 36000;
    public void TimeManagement(bool isRewinding,int rewindSpeed)
    {
        if (isRewinding) RewindTime(rewindSpeed);
        else RecordTime();
    }
    private void RecordTime()
    {
        if (Kinematic)
        {
            if (kinematicData.Count > frameLimit) kinematicData.RemoveAt(0);

            kinematicData.Add(new KinematicData(transform.position, GetComponent<Rigidbody>().velocity,transform.rotation,GetComponent<Rigidbody>().angularVelocity));
        }
        timeCount++;
        kinematicTimeCount=kinematicData.Count;
    }
    private void RewindTime(int rewindSpeed)
    {
        if (Kinematic) KinematicRewind(rewindSpeed);
        timeCount=timeCount- rewindSpeed;
        kinematicTimeCount = kinematicData.Count;

    }
    private void KinematicRewind(int rewindSpeed)
    {
        transform.position=kinematicData[kinematicTimeCount-rewindSpeed].Position;
        GetComponent<Rigidbody>().velocity = kinematicData[kinematicTimeCount - rewindSpeed].Velocity;
        transform.rotation = kinematicData[kinematicTimeCount - rewindSpeed].Rotation;
        GetComponent<Rigidbody>().angularVelocity = kinematicData[kinematicTimeCount - rewindSpeed].AngularVelocity;
        for (int i = 0; i < rewindSpeed; i++)
        {
            kinematicData.RemoveAt(kinematicData.Count - 1);
        }
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
}