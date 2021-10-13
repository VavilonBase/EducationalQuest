using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portrait_activation : MonoBehaviour
{
    private bool infoIsShowed = false;
    private GameObject info;

    void Start()
    {
        info = transform.parent.Find("ScientistInfo").gameObject;
        info.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!infoIsShowed)
        {
            infoIsShowed = true;
            info.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (infoIsShowed)
        {
            infoIsShowed = false;
            info.SetActive(false);
        }
    }
}
