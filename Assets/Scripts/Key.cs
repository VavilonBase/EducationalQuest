using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private Transform thisTransform;

    public void ChangeKeyPosition(Vector3 newPosition)
    {
        thisTransform.position = newPosition;
    }
    
    private Vector3 keyPosition;
    // Start is called before the first frame update
    void Start()
    {
        thisTransform = this.transform;
    }
}
