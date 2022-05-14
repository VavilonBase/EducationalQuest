using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpBlock : MonoBehaviour
{
    private GameObject parent;
    public bool isPlateUp;
    public bool IsPlateUp { get { return isPlateUp; } }

    public float height = 0.2f;

    private void Awake()
    {
        parent = this.transform.parent.gameObject;
        isPlateUp = false;
    }

    private void ChangePosition()
    {        
        Vector3 newPosition = parent.transform.position;
        //если табличка поднята, опустить, и наоборот
        if (isPlateUp)
            newPosition.y -= height;
        else
            newPosition.y += height;
        parent.transform.position = newPosition;
        isPlateUp = !isPlateUp;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (!isPlateUp)
        {
            ChangePosition();
            DataHolder.ChangeMessageTemporary("Нажми F, чтобы выбрать");
        }        
    }
    
    private void OnTriggerExit(Collider other) 
    {
        if (isPlateUp)
        {
            ChangePosition();
            DataHolder.ChangeMessageTemporary();
        }            
    }
    private void OnDisable()
    {
        if (isPlateUp)
        {
            ChangePosition();
            DataHolder.ChangeMessageTemporary();
        }
    }
}

