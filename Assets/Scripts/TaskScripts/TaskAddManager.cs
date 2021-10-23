using Assets.Scripts.TaskScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TaskAddManager : MonoBehaviour
{
    [Header("Components")]
    public Button nextBtn; //������ �����
    public Button backBtn; //������ �����
    public Button addBtn; //������ ��������
    public Text errorText; // ����� � �������
    [Space]
    public GameObject directionField; //���� � ������� �������
    public GameObject addQuestionField; // ���� � ����������� �������
    public GameObject[] answersField; //���� � ����������� ������� 
    public Toggle[] answerTrueToggles; //������������� ������ ����������� ������
    private Dropdown _directionDropdown; //���������� ���� ��� ������ �����������
    [Space]
    [Header("Scripts")]
    [SerializeField] private TaskManager _scriptTaskManager;
    private TaskGenerateImage _scriptGenerateQuestionImage; //������ ��� ��������� ������� 
    private TaskGenerateImage[] _scriptsGenerateAnswerImage; //������ ��� ��������� ������
    private byte step = 0; //���� �� ������� ������ ��������� ����������

    // Start is called before the first frame update
    void Start()
    {

        //��������� �������������
        //�������� ������� ���� �����������
        GetObjectsFromDirectionField();
        //��������� ������������� ������ ����������� ������
        OffAnswerTrueToggles();

        //��������� ������� ��� ��������� �������
        GetScriptFromQuestionAddField();

        //��������� �������� ��� ��������� �������
        GetScriptsFromAnswersAddFields();

        //���������� �������
        SetListenerCommonAllField();
        SetListenerAnswerTrueToggles();

        //�������� ��� ����
        directionField.SetActive(false);
        addQuestionField.SetActive(false);
        HideAnswersField();
        errorText.gameObject.SetActive(false);

        //�������� ������ �������� � ���������� ������ ����� � �����
        addBtn.gameObject.SetActive(false);
        nextBtn.gameObject.SetActive(true);
        backBtn.gameObject.SetActive(false);
        //��������� ����������� ����
        RedrawWindow();
       
    }

    //---------------------������ ��������� �������� �����---------------------------
    //��������� �������� �� ���� �����������
    void GetObjectsFromDirectionField()
    {
        //��������� �����������
        _directionDropdown = directionField.GetComponentInChildren<Dropdown>();
    }
    //��������� �������� �� ���� ���������� �������
    void GetScriptFromQuestionAddField()
    {
        _scriptGenerateQuestionImage = addQuestionField.GetComponent<TaskGenerateImage>();
    }

    //��������� �������� �� ����� ���������� ������
    void GetScriptsFromAnswersAddFields()
    {
        _scriptsGenerateAnswerImage = new TaskGenerateImage[answersField.Length];
        for (int i = 0; i < answersField.Length; i++)
        {
            _scriptsGenerateAnswerImage[i] = answersField[i].GetComponent<TaskGenerateImage>();
        }
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

        //��������� ������� ��� ������ ��������
        Button addBtnComponent = addBtn.GetComponent<Button>();
        addBtnComponent.onClick.AddListener(ClickAddBtn);
    }
    
    //----------------------���������� � � �������� �������------------------
    //���������� ������� ��� �������������� ����������� ������
    void SetListenerAnswerTrueToggles()
    {
        foreach (Toggle answerTrueToggle in answerTrueToggles)
        {
            answerTrueToggle.onValueChanged.AddListener(delegate { ChangedValueAnswerTrueToggle(answerTrueToggle); });
        }
    }

    //��������� ��������� ������� � �������������� ����������� ������
    void OffListnerAnswerTrueToggles()
    {
        foreach (Toggle answerTrueToggle in answerTrueToggles)
        {
            answerTrueToggle.onValueChanged.RemoveAllListeners();
        }
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

    //������� ����� ��� ������ ��������
    void ClickAddBtn()
    {
        if (CheckExistTrueToggle())
        {
            //�������� ����� � �������
            errorText.text = "";
            errorText.gameObject.SetActive(false);
            //�������� �����������
            string direction = _directionDropdown.options[_directionDropdown.value].text;
            //���������� ��� �������� ����� ����������� �����������
            string pathFolder = "";
            //����� ����� ����������� ����������� ��� ��������� png, ���� �� ���, �� �������
            switch (direction)
            {
                case "��������":
                    pathFolder = Application.dataPath + "/Resources/Mechanics";
                    if (!Directory.Exists(pathFolder)) Directory.CreateDirectory(pathFolder);
                    break;
                case "������������ ������":
                    pathFolder = Application.dataPath + "/Resources/Molecular";
                    if (!Directory.Exists(pathFolder)) Directory.CreateDirectory(pathFolder);
                    break;
                case "�������������":
                    pathFolder = Application.dataPath + "/Resources/Electricity";
                    if (!Directory.Exists(pathFolder)) Directory.CreateDirectory(pathFolder);
                    break;
                default:
                    break;
            }
            //���� ����� ����, �� ��������� �����
            if (pathFolder != "")
            {
                //�������� ����� �������
                //�������� ��� �����
                string[] allFiles = Directory.GetFiles(pathFolder);
                List<int> questionNumbers = new List<int>();
                //������ ������ ������ ����� �� ������ �������
                foreach (string file in allFiles)
                {
                    //���� ���� ��������� �� .png
                    if (file.Substring(file.Length - 3, 3) == "png")
                    {
                        //����� ���������� ��������� ��������� ����� \
                        int indexSlash = file.LastIndexOf("\\", file.Length - 1, file.Length);
                        //����� ���������� ��������� ����� .
                        int indexDote = file.LastIndexOf(".", file.Length - 1, file.Length);
                        questionNumbers.Add(Convert.ToInt32(file.Substring(indexSlash + 2, indexDote - indexSlash - 2)));
                    }
                }
                int nextQuestionNumber = 1;//����� ���������� �������, � ������ ���� �������� �� ����� � �����, �� ��� ������, ��� ��� ������ ������
                                           //�������, ���� �� ������ �������
                if (questionNumbers.Count != 0)
                {
                    //��������� ������ �� �����������
                    questionNumbers.Sort();
                    //�������� ����� ���������� �������
                    nextQuestionNumber = questionNumbers[questionNumbers.Count - 1] + 1;
                }
                //������� ���� �� ����� �� ��������� ��������
                string pathAnswersFolder = pathFolder + "/Q" + nextQuestionNumber.ToString();
                //������� ��� �����, ���� �� ���
                if (!Directory.Exists(pathAnswersFolder)) Directory.CreateDirectory(pathAnswersFolder);
                //���������� png ��� �������
                _scriptGenerateQuestionImage.GeneratePng(pathFolder + "/Q" + nextQuestionNumber.ToString() + ".png");
                //���������� png ��� �������
                for (int i = 0; i < _scriptsGenerateAnswerImage.Length; i++)
                {
                    _scriptsGenerateAnswerImage[i].GeneratePng(pathAnswersFolder + "/answer" + (i + 1).ToString() + ".png");
                }
            }
            this._scriptTaskManager.ViewTaskList();
        }
        else
        {
            errorText.text = "�������� ���������� �����!";
            errorText.gameObject.SetActive(true);
        }
        
    }

    //��������� �������� ������������� ����������� ������
    void ChangedValueAnswerTrueToggle(Toggle toggle)
    {
        //��������� ������� ��������� �������������
        bool isOn = toggle.isOn;
        //�������� ��������� ������� ��������� �������� ��������������
        OffListnerAnswerTrueToggles();
        //��������� ��� �������������
        OffAnswerTrueToggles();
        //��������� ������������� ��� �������� ���������
        toggle.isOn = isOn;
        //���� ����������� ���������, ����������� ������, ��� ���� ������� ���������� �����
        if (!isOn)
        {
            errorText.text = "�������� ���������� �����!";
            errorText.gameObject.SetActive(true);
        }
        else
        {
            //����� ������� ����� � �������
            errorText.text = "";
            errorText.gameObject.SetActive(false);
        }
        //������� ��������� ������� ��������� �������� ��������������
        SetListenerAnswerTrueToggles();
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
        foreach (GameObject answerField in answersField)
        {
            answerField.SetActive(false);
        }
    }
    
    //������ ������� � �������������� ����������� ������
    void OffAnswerTrueToggles()
    {
        foreach (Toggle answerTrueToggle in answerTrueToggles)
        {
            answerTrueToggle.isOn = false;
        }
    }
    
    //�������� ����� �� ������� ������
    bool CheckExistTrueToggle()
    {
        foreach (Toggle answerTrueToggle in answerTrueToggles)
        {
            if (answerTrueToggle.isOn)
            {
                return true;
            }
        }
        return false;
    }
}
