using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question_click : MonoBehaviour
{
    CsGlobals gl;
    void Start()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;        
    }

    public void onClick()
    {        
        gl.textUI_startMessage.SetActive(true);
        gl.startMessageIsShowing = true;
        gl.textUI_question.SetActive(false);
    }
}
