using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStartInteractions : MonoBehaviour
{
    private CsGlobals gl;
    private GameObject buttonLeaveSession;
    void OnEnable()
    {
        if (gl.playerInfo.isAuthorized)
        {
            gl.ChangeMessageDurable(true, "����� ����������, " + gl.playerInfo.responseUserData.user.firstName);
            buttonLeaveSession.SetActive(true);
        }
        else
        {
            gl.ChangeMessageDurable(true, "����� ����������, �����");
            buttonLeaveSession.SetActive(false);
        }            
    }
    // Start is called before the first frame update
    void Awake()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
        buttonLeaveSession = this.transform.Find("But_Out").gameObject;
    }
}
