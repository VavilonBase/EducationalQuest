using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpBlock : MonoBehaviour
{
    GameObject parentBoard;
    private bool isPlateUp = false;
    public bool IsPlateUp { get { return isPlateUp; } }

    private CsGlobals gl;

    public GameObject board; // --- не нужно
    public float height = 0.2f;    
    public Material[] materialArray; // --- не нужно
    public bool isUp = false; // --- не нужно

    void Start()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
        GameObject _parent = transform.parent.gameObject;
        parentBoard = _parent.transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other) { 
        if (!isPlateUp) {
            Vector3 newPosition = transform.parent.position;
            newPosition.y += height;
            transform.parent.transform.position = newPosition;
            isPlateUp = true;
            parentBoard.GetComponent<TaskBoardWorking>().IsAnswerUp = isPlateUp;

            gl.PrintLabel("Нажми F, чтобы выбрать ответ");
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (isPlateUp) {
            Vector3 newPosition = transform.parent.position;
            newPosition.y -= height;
            transform.parent.transform.position =newPosition;
            isPlateUp = false;
            parentBoard.GetComponent<TaskBoardWorking>().IsAnswerUp = isPlateUp;

            gl.HideLabel();
        }
    }
    private void OnDisable()
    {
        if (isPlateUp)
        {
            Vector3 newPosition = transform.parent.position;
            newPosition.y -= height;
            transform.parent.transform.position = newPosition;
            isPlateUp = false;
            parentBoard.GetComponent<TaskBoardWorking>().IsAnswerUp = isPlateUp;

            gl.HideLabel();
        }
    }
}

