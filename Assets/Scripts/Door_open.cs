using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_open : MonoBehaviour
{
    public byte numDoor;
    private CsGlobals gl;

    public bool isLocked = false;
    public int max_amplitude = 100;
    //public GameObject textUI; // <- добавить в globals

    private bool isOpen = false;
    private bool action = false;
    private bool use_key = false;

    Vector3 close_position;
    Vector3 open_position;
    Vector3 cur_position;
    private float smoothing = 2f;

    private GameObject leaf;
    // Start is called before the first frame update
    void Start()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
        
        leaf = transform.parent.transform.Find("rotation_point").gameObject;

        close_position = new Vector3(0, 0, 0);
        open_position = new Vector3(0, max_amplitude, 0);
        cur_position = leaf.transform.localEulerAngles;         
    }

    // Update is called once per frame
    void Update()
    {
        if (use_key && Input.GetKeyDown(KeyCode.F))
        {
            isLocked = false;
            gl.playerInfo.PutAwayKey(numDoor);
            gl.keyIcon.SetActive(false);
            gl.textUI_pressF.SetActive(false);
            //Сброс триггера
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        if (action && isOpen)
        {
            cur_position = Vector3.Lerp(cur_position, open_position, smoothing * Time.deltaTime);
            leaf.transform.localEulerAngles = cur_position;
            if (cur_position == open_position) action = false;
        }

        if (action && !isOpen)
        {
            cur_position = Vector3.Lerp(cur_position, close_position, Time.deltaTime);
            leaf.transform.localEulerAngles = cur_position;
            if (cur_position == close_position) action = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isLocked)
        {
            isOpen = true;
            action = true;
        }
        else
        {
            if (gl.playerInfo.ActiveKey)
            {
                use_key = true;
                gl.PrintLabel("Нажми F, чтобы использовать ключ");
            }
            else gl.PrintLabel("Дверь заперта. Нужен ключ");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isLocked)
        {
            isOpen = false;
            action = true;
        }
        use_key = false;
        gl.HideLabel();
    }
}
