using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_appearance : MonoBehaviour
{
    private Key parentKey;
    CsGlobals gl;
    
    private bool playerIsCloseEnough = false; // ��������� �� ����� ���������� ������, ����� �����������������

    void Start()
    {
        parentKey = this.GetComponentInParent<Key>();
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsCloseEnough && Input.GetKeyDown(KeyCode.F))
        {
            gl.playerInfo.active_key = true; // ������ ���� � ���������
            gl.keyIcon.SetActive(true); // ���������� ������, ����� ���������� ������� �����
            parentKey.ChangeKeyPosition(gl.null_position); // ������� ���� �� ������� ��������� ������
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == gl.player)
        {
            gl.textUI_pressF.SetActive(true); // ������� ���������: �������� ��������������
            playerIsCloseEnough = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == gl.player)
        {
            gl.textUI_pressF.SetActive(false); // ������� ���������: �������������� ������ ����������
            playerIsCloseEnough = false;
        }
    }
}
