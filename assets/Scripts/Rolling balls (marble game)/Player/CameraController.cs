using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;

    private void Start()
    {
        //I put the input name from the YAxis in cinemachine to nothing
        freeLookCamera.m_YAxis.m_InputAxisName = "";
    }

    void Update()
    {
        if (Input.GetMouseButton(1) || Input.GetButton("ControllerCamButton"))
        {
            //Sets the InputName to Mouse X
            freeLookCamera.m_XAxis.m_InputAxisName = "Mouse X";
        }
        else
        {
            //Puts the input name back to nothing
            freeLookCamera.m_XAxis.m_InputAxisName = "";
            freeLookCamera.m_XAxis.m_InputAxisValue = 0;
        }
    }
}
