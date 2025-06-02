using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.Utility;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private CinemachineVirtualCamera playerCam;
    [SerializeField] private CinemachineVirtualCamera shadowCam;
    [SerializeField] private bool isShadow;
    private void Awake()
    {
        playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
        shadowCam = GameObject.Find("ShadowCam").GetComponent<CinemachineVirtualCamera>();
    }
    public void SwapCamera(bool input)
    {
        isShadow = input;
        if (isShadow)
        {
            playerCam.Priority--;
            shadowCam.Priority++;
        }
        else
        {
            playerCam.Priority++;
            shadowCam.Priority--;
        }
    }
}
