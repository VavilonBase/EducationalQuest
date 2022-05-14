using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        transform.parent.gameObject.GetComponent<Board>().IsStandingOnThePlatform = true;
    }

    private void OnTriggerExit(Collider other)
    {
        transform.parent.gameObject.GetComponent<Board>().IsStandingOnThePlatform = false;
        DataHolder.ChangeMessageTemporary();
    }
}
