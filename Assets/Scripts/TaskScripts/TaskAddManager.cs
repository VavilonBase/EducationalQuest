using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskAddManager : MonoBehaviour
{
    public Button nextBtn;
    public Button backBtn;
    public Button addBtn;

    public GameObject directionField; //���� � ������� �������
    public GameObject addQuestionField; // ���� � ����������� �������
    public GameObject[] answersField; //���� � ����������� ������� 
    private byte step = 0; //���� �� ������� ������ ��������� ����������

    // Start is called before the first frame update
    void Start()
    {

        //�������� ��� ����
        directionField.SetActive(false);
        addQuestionField.SetActive(false);
        HideAnswersField();

        //�������� ������ �������� � ���������� ������ ����� � �����
        addBtn.gameObject.SetActive(false);
        nextBtn.gameObject.SetActive(true);
        backBtn.gameObject.SetActive(false);

        //���������� �������
        SetListenerCommonAllField();

        //��������� �������������
        //��������� ����������� ����
        RedrawWindow();
       
    }


    //---------------------������ ���������� �������-------------------------------
    void SetListenerCommonAllField()
    {
        //��������� ������� ��� ������ �����
        Button nextBtnComponent = nextBtn.GetComponent<Button>();
        nextBtnComponent.onClick.AddListener(ClickNextBtn);

        //��������� ������� ��� ������ �����
        Button backBtnComponent = backBtn.GetComponent<Button>();
        backBtnComponent.onClick.AddListener(ClickBackBtn);
    }

    //----------------------�������--------------------
    //������� ��� ������ ������
    void ClickNextBtn()
    {
        step++;
        //���� ���� ������ ��� ���-�� ������� + 1 (�� +2, ��� ��� ������ � 0), �� �������� ������ ����� � ���������� ������ ��������
        if (step >= (answersField.Length + 1))
        {
            nextBtn.gameObject.SetActive(false); //�������� ������ �����
            addBtn.gameObject.SetActive(true); //���������� ������ ��������
        }
        else
        {
            addBtn.gameObject.SetActive(false); //�������� ������ ��������
        }
        //���� ���� ������ 0, �� ���������� ������ �����
        backBtn.gameObject.SetActive(step > 0);
        RedrawWindow();
    }

    //������� ��� ������ �����
    void ClickBackBtn()
    {
        step--;
        //���� ���� ������ ��� ����� 0, �� �������� ������ �����
        if (step <= 0)
        {
            backBtn.gameObject.SetActive(false);
        }
        //���� ���� ������ ����, �� ���������� ������ ������
        nextBtn.gameObject.SetActive(step >= 0);
        RedrawWindow();
        
    }


    //---------------------�����������-------------------
    //����������� ���� � ����������� �� �������� ���������� step
    void RedrawWindow()
    {
        switch (step)
        {
            case 0:
                directionField.SetActive(true);
                addQuestionField.SetActive(false);
                break;
            case 1:
                directionField.SetActive(false);
                addQuestionField.SetActive(true);
                HideAnswersField();
                break;
            default: //��������� ����� � ��������
                addQuestionField.SetActive(false);
                HideAnswersField();
                answersField[step - 2].SetActive(true);
                break;
        }
    }

    //--------------------���. ������------------------------
    //�������� ���� ����� � ��������
    void HideAnswersField()
    {
        foreach(GameObject answerField in answersField)
        {
            answerField.SetActive(false);
        }
    }
}
