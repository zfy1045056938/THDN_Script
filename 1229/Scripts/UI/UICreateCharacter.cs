using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class UICreateCharacter : MonoBehaviour
{
    private CinemachineBrain braincamera;
    public CinemachineVirtualCamera vcamera;
    public CinemachineVirtualCamera vcamera1;
    private CinemachineFreeLook freelook;
    public Transform SelectableCharacterPos;
    void Start()
    {
        braincamera = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>();
        vcamera = GetComponent<CinemachineVirtualCamera>();
        vcamera1 = GetComponent<CinemachineVirtualCamera>();
        
        //
        vcamera.m_Priority = 10;
        vcamera1.m_Priority = 1;

    }

    public void ChangeCamera()
    {

        vcamera.LookAt = SelectableCharacterPos.transform;
    }
}
