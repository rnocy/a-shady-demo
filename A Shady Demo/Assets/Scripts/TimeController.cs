using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
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
    public bool isShadow=false;
    private PlayerInputs playerInputs;
    private PlayerMovements playerMovements;
    private ShadowMovements shadowMovements;
    private CameraController cameraController;
    private void Awake()
    {
        ControlledObject = FindTimeManager();
        playerInputs = new PlayerInputs();
        playerInputs.Default.Enable();
        playerMovements = GameObject.Find("PLAYER").GetComponent<PlayerMovements>();
        shadowMovements = GameObject.Find("SHADOW").GetComponent<ShadowMovements>();
        cameraController = GameObject.Find("CAMERACONTROL").GetComponent<CameraController>();
        playerInputs.Default.Rewind.performed += Rewind;
        playerInputs.Default.Swap.performed += Swap;
        SetActiveCharacter();
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
        if (timeCount <= rewindSpeed-1) isRewinding = false;
    }
    private void RampUp()
    {
        rewindSpeed = (int)Mathf.Floor(baseRewindSpeed + rewindSpeedProgress);
        if (isRewinding)    rewindSpeedProgress += Time.fixedDeltaTime * rewindSpeedRamp;        
        else rewindSpeedProgress = 0f;
    }
    private void Rewind(InputAction.CallbackContext context)
    {
        isRewinding = (playerInputs.Default.Rewind.ReadValue<float>() > 0);
        //Debug.Log(playerInputs.Default.Rewind.ReadValue<float>());
    }
    private void Swap(InputAction.CallbackContext context)
    {
        isShadow = !isShadow;
        SetActiveCharacter();
    }
    private void SetActiveCharacter()
    {
        playerMovements.isActive = !isShadow;
        shadowMovements.isActive = isShadow;
        cameraController.SwapCamera(isShadow);
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