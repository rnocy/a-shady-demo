using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] private List<TimeDataManager> ControlledObject=new List<TimeDataManager>();
    [SerializeField] private int baseRewindSpeed = 1;
    [SerializeField] private int rewindSpeed = 1;
    [SerializeField] private float rewindSpeedRamp = 1;
    [SerializeField] private float rewindSpeedProgress = 0f;
    [SerializeField] private bool isRewinding=false;
    [SerializeField] private int timeCount = 0;
    private PlayerMovements playerMovements;
    private void Awake()
    {
        ControlledObject = FindTimeManager();
        playerMovements = GetComponent<PlayerMovements>();
    }
    private void FixedUpdate()
    {
        TimeUpdate();
    }
    private void TimeUpdate()
    {
        RampUp();
        if (!isRewinding) timeCount++;
        else timeCount=timeCount-rewindSpeed;
        foreach (TimeDataManager timeData in ControlledObject)
        {
            timeData.TimeManagement(isRewinding,rewindSpeed);
        }
        isRewinding = playerMovements.isRewinding;
        if (timeCount <= rewindSpeed-1) isRewinding = false;
    }
    private void RampUp()
    {
        rewindSpeed = (int)Mathf.Floor(baseRewindSpeed + rewindSpeedProgress);
        if (isRewinding)    rewindSpeedProgress += Time.fixedDeltaTime * rewindSpeedRamp;        
        else rewindSpeedProgress = 0f;
    }
    private static List<TimeDataManager> FindTimeManager()
    {
        GameObject[] goArray = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        List<TimeDataManager> goList = new List<TimeDataManager>();

        foreach (GameObject go in goArray)
        {
                if (go.GetComponent<TimeDataManager>() != null)
                {
                    goList.Add(go.GetComponent<TimeDataManager>());
                }
        }

        if (goList.Count == 0)
        {
            return null;
        }

        return goList;
    }
}