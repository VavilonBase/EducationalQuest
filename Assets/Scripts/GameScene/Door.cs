using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isUnlocked; // --- ����� �� ������� �����

    private bool isNeedAction; // --- ����� �� ����������������� � ������
    private enum Action { toOpen, toClose } // --- ��� ������ ����������������� � ������
    private Action action;

    public void Unlock() { isUnlocked = true; }
    public void Lock() { isUnlocked = false; }

    public int maxAmplitude = 100;
    private Vector3 closePosition;
    private Vector3 openPosition;
    private Vector3 curPosition;
    private float smoothing = 2f;

    private GameObject leaf;

    private void Awake()
    {
        leaf = this.transform.parent.Find("rotation_point").gameObject;
        closePosition = new Vector3(0, 0, 0);
        openPosition = new Vector3(0, maxAmplitude, 0);
        curPosition = leaf.transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (isNeedAction)
        {
            switch (action)
            {
                case Action.toOpen:
                    //������ ������� �����
                    curPosition = Vector3.Lerp(curPosition, openPosition, smoothing * Time.deltaTime);
                    leaf.transform.localEulerAngles = curPosition;
                    if (curPosition == openPosition) isNeedAction = false;
                    break;
                case Action.toClose:
                    //������ ������� �����
                    curPosition = Vector3.Lerp(curPosition, closePosition, Time.deltaTime);
                    leaf.transform.localEulerAngles = curPosition;
                    if (curPosition == closePosition) isNeedAction = false;
                    break;
                default:
                    break;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isUnlocked)
        {
            //���� ����� �� �������, �������
            action = Action.toOpen;
            isNeedAction = true;
        }
        else
            DataHolder.ChangeMessageTemporary("����� �������");
    }

    private void OnTriggerExit(Collider other)
    {
        //������� �����
        action = Action.toClose;
        isNeedAction = true;
        DataHolder.ChangeMessageTemporary();
    }
}
