using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpBlock : MonoBehaviour
{
    public float height = 0.2f;
    public GameObject textUI;
    public int pointer = 0;
    public Material[] materialArray;
    private bool isUp = false;
    public bool IsUp {get => isUp;}
    private GameObject parent;
    private GameObject answer;
    // Start is called before the first frame update
    void Start()
    {
        textUI.SetActive(false);
        parent = transform.parent.gameObject;
        answer = parent.transform.Find("Answer").gameObject;
    }

    void Update() {
        if (isUp) {
            if (Input.GetKey(KeyCode.F)) { 
                if(pointer >= materialArray.Length){
                    Debug.Log("Error!!! Watch for fucking pointer!!!");
                    pointer = 0;
                } 
                else {
                    gameObject.GetComponent<Renderer>().material = materialArray[pointer];
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) { 
        if (!isUp) {
            Vector3 newPosition = transform.parent.position;
            newPosition.y += height;
            transform.parent.transform.position = newPosition;
            isUp = true;
            textUI.SetActive(true);
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (isUp) {
            Vector3 newPosition = transform.parent.position;
            newPosition.y -= height;
            transform.parent.transform.position =newPosition;
            isUp = false;
            textUI.SetActive(false);
        }
    }

}

