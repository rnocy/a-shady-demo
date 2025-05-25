using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] List<TimeDataManager> ControlledObject=new List<TimeDataManager>();
    [SerializeField] private int rewindSpeed = 2;
    // Start is called before the first frame update
    [SerializeField] private bool isRewinding=false;
    [SerializeField] private int timeCount = 0;
    private PlayerMovements playerMovements;
    private void Awake()
    {
        ControlledObject = FindTimeManager();
        playerMovements = GetComponent<PlayerMovements>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        TimeUpdate();
    }
    private void TimeUpdate()
    {
        if (!isRewinding) timeCount++;
        else timeCount=timeCount-rewindSpeed;
        foreach (TimeDataManager timeData in ControlledObject)
        {
            timeData.TimeManagement(isRewinding,rewindSpeed);
        }
        isRewinding = playerMovements.isRewinding;
        if (timeCount <= rewindSpeed-1) isRewinding = false;
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