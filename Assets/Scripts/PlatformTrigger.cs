using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        parent.GetComponent<TaskBoardWorking>().IsStandingOnPlatform = true;        
    }

    private void OnTriggerExit(Collider other)
    {
        parent.GetComponent<TaskBoardWorking>().IsStandingOnPlatform = false;
    }
}
