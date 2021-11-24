using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TaskAddManager : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] private GameObject _directionField; //���� � ������� �������
    [SerializeField] private TaskWindowComponent _addQuestionWindow; // ���� � ����������� �������
    [SerializeField] private TaskWindowComponent[] _addAnswersWindows; //���� � ����������� ������� 
    [Space]
    [Header("Buttons")]
    [SerializeField] private Button _nextBtn; //������ �����
    [SerializeField] private Button _backBtn; //������ �����
    [SerializeField] private Button _addBtn; //������ ��������
    [SerializeField] private Button _taskListBtn; //������ ��� �������� � ������ �������
    [Header("Components")]
    [SerializeField] private Text _errorText; // ����� � �������
    [SerializeField] private Toggle[] _answerTrueToggles; //������������� ������ ����������� ������
    [Space]
    [Header("Task Manager")]
    [SerializeField] private TaskManager _taskManager;

    private TaskBoardInformation _taskBoardInformation;
    private CsGlobals _globalOptions;
    private Dropdown _directionDropdown; //���������� ���� ��� ������ �����������
    private byte _step = 0; //���� �� ������� ������ ��������� ����������

    // Start is called before the first frame update
    void Awake()
    {

        //��������� �������������
        //�������� ������� ���� �����������
        GetObjectsFromDirectionField();
        //��������� ������������� ������ ����������� ������
        OffAnswerTrueToggles();
        //����� ����������� �������
        _globalOptions = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;

        //���������� �������
        SetListenerCommonAllField();
        SetListenerAnswerTrueToggles();

        //�������� ��� ����
        _directionField.SetActive(false);
        _addQuestionWindow.SetInactive();
        HideAnswersField();
        _errorText.gameObject.SetActive(false);

        //�������� ������ �������� � ���������� ������ ����� � �����
        _addBtn.gameObject.SetActive(false);
        _nextBtn.gameObject.SetActive(true);
        _backBtn.gameObject.SetActive(false);
        //��������� ����������� ����
        RedrawWindow();
       
    }

    //---------------------������ ��������� �������� �����---------------------------
    //��������� �������� �� ���� �����������
    void GetObjectsFromDirectionField()
    {
        //��������� �����������
        _directionDropdown = _directionField.GetComponentInChildren<Dropdown>();
    }

    //---------------------������ ���������� �������-------------------------------
    void SetListenerCommonAllField()
    {
        //��������� ������� ��� ������ �����
        Button nextBtnComponent = _nextBtn.GetComponent<Button>();
        nextBtnComponent.onClick.AddListener(ClickNextBtn);

        //��������� ������� ��� ������ �����
        Button backBtnComponent = _backBtn.GetComponent<Button>();
        backBtnComponent.onClick.AddListener(ClickBackBtn);

        //��������� ������� ��� ������ ��������
        Button addBtnComponent = _addBtn.GetComponent<Button>();
        addBtnComponent.onClick.AddListener(ClickAddBtn);

        //��������� ������� ��� ������ � ������ �������
        Button taskListBtnComponent = _taskListBtn.GetComponent<Button>();
        taskListBtnComponent.onClick.AddListener(ClickTaskListBtn);
    }

    //���������� ������� ��� �������������� ����������� ������
    void SetListenerAnswerTrueToggles()
    {
        foreach (Toggle answerTrueToggle in _answerTrueToggles)
        {
            answerTrueToggle.onValueChanged.AddListener(delegate { ChangedValueAnswerTrueToggle(answerTrueToggle); });
        }
    }

    //----------------------�������� �������------------------
    //��������� ��������� ������� � �������������� ����������� ������
    void OffListnerAnswerTrueToggles()
    {
        foreach (Toggle answerTrueToggle in _answerTrueToggles)
        {
            answerTrueToggle.onValueChanged.RemoveAllListeners();
        }
    }


    //----------------------�������--------------------
    //������� ��� ������ ������
    void ClickNextBtn()
    {
        _step++;
        //���� ���� ������ ��� ���-�� ������� + 1 (�� +2, ��� ��� ������ � 0), �� �������� ������ ����� � ���������� ������ ��������
        if (_step >= (_addAnswersWindows.Length + 1))
        {
            _nextBtn.gameObject.SetActive(false); //�������� ������ �����
            _addBtn.gameObject.SetActive(true); //���������� ������ ��������
        }
        else
        {
            _addBtn.gameObject.SetActive(false); //�������� ������ ��������
        }
        //���� ���� ������ 0, �� ���������� ������ �����
        _backBtn.gameObject.SetActive(_step > 0);
        RedrawWindow();
    }

    //������� ��� ������ �����
    void ClickBackBtn()
    {
        _step--;
        //���� ���� ������ ��� ����� 0, �� �������� ������ �����
        if (_step <= 0)
        {
            _backBtn.gameObject.SetActive(false);
        }
        //���� ���� ������ ����, �� ���������� ������ ������
        _nextBtn.gameObject.SetActive(_step >= 0);
        RedrawWindow();
        
    }

    //������� ��� ������ � ������ �������
    void ClickTaskListBtn()
    {
        ViewTaskList();
    }

    //������� ����� ��� ������ ��������
    void ClickAddBtn()
    {
        if (CheckExistTrueToggle())
        {
            //�������� �����������
            string direction = _directionDropdown.options[_directionDropdown.value].text;
            //�������� ���� �� ����� ������� �����������
            string pathFolder = GetOrCreateDirectionDirectory(direction);
            //���� ����� ����, �� ��������� �����
            if (pathFolder != "")
            {
                //�������� ������ ��������
                List<int> questionNumbersList = GetTasksNumbersListFromDirectory(pathFolder);

                int nextQuestionNumber = 1;//����� ���������� �������, � ������ ���� �������� �� ����� � �����, �� ��� ������, ��� ��� ������ ������

                //�������, ���� �� ������ �������
                if (questionNumbersList.Count != 0)
                {
                    //��������� ������ �� �����������
                    questionNumbersList.Sort();
                    //�������� ����� ���������� �������
                    nextQuestionNumber = questionNumbersList[questionNumbersList.Count - 1] + 1;
                }
                //������� ���� �� ����� �� ��������� ��������
                string pathAnswersFolder = pathFolder + "/Q" + nextQuestionNumber.ToString();
                //������� ��� �����, ���� �� ���
                if (!Directory.Exists(pathAnswersFolder)) Directory.CreateDirectory(pathAnswersFolder);
                //���������� png ��� �������
                _addQuestionWindow.GeneratePng(_taskManager.textureGenerator, pathFolder + "/Q" + nextQuestionNumber.ToString() + ".png");
                //���������� png ��� �������
                for (int i = 0; i < _addAnswersWindows.Length; i++)
                {
                   _addAnswersWindows[i].GeneratePng(_taskManager.textureGenerator, pathAnswersFolder + "/" + (i + 1).ToString() + ".png");
                }
            }
            byte rightAnswer = this.GetRightAnswer();
            if (this._taskBoardInformation != null)
            {
                if (!this._taskBoardInformation.AddQuestion(rightAnswer))
                {
                    Debug.Log("������ �� ����������!");
                }
            }
            ViewTaskList();
        }
        else
        {
            //������� ����� � �������
            ViewErrorText("�������� ���������� �����!");
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
            //������� ����� � �������
            ViewErrorText("�������� ���������� �����!");
        }
        else
        {
            //����� ������� ����� � �������
            HideErrorText();
        }
        //������� ��������� ������� ��������� �������� ��������������
        SetListenerAnswerTrueToggles();
    }

    //---------------------�����������-------------------
    //����������� ���� � ����������� �� �������� ���������� _step
    void RedrawWindow()
    {
        switch (this._step)
        {
            case 0:
                _directionField.SetActive(true);
                _addQuestionWindow.SetInactive();
                break;
            case 1:
                _directionField.SetActive(false);
                _addQuestionWindow.SetActive();
                HideAnswersField();
                break;
            default: //��������� ����� � ��������
                _addQuestionWindow.SetInactive();
                HideAnswersField();
                _addAnswersWindows[_step - 2].SetActive();
                break;
        }
    }

    //--------------------���. ������------------------------
    //�������� ���� ����� � ��������
    void HideAnswersField()
    {
        foreach (TaskWindowComponent _addAnswerWindow in _addAnswersWindows)
        {
            _addAnswerWindow.SetInactive();
        }
    }
    
    //������ ������� � �������������� ����������� ������
    void OffAnswerTrueToggles()
    {
        foreach (Toggle answerTrueToggle in _answerTrueToggles)
        {
            answerTrueToggle.isOn = false;
        }
    }
    
    byte GetRightAnswer()
    {
        for (byte i = 0; i < this._answerTrueToggles.Length; i++)
        {
            if (this._answerTrueToggles[i].isOn)
            {
                return i;
            }
        }
        return 0;
    }

    //�������� ����� �� ������� ������
    bool CheckExistTrueToggle()
    {
        foreach (Toggle answerTrueToggle in _answerTrueToggles)
        {
            if (answerTrueToggle.isOn)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// ����� ���� �����
    /// </summary>
    void ResetAllFields()
    {
        //����� �������� �����
        _step = 0;
        //����� directionField
        _directionDropdown.value = 0;

        //����� QuestionField
        _addQuestionWindow.ResetWindow();
        
        //����� AnswersField
        foreach (TaskWindowComponent _addAnswerWindow in _addAnswersWindows)
        {
            _addAnswerWindow.ResetWindow();
        }
        //�������� ��� ������������� ����������� ������
        //�������� ��������� ������� ��������� �������� ��������������
        OffListnerAnswerTrueToggles();
        //��������� ��� �������������
        OffAnswerTrueToggles();
        //������� ��������� ������� ��������� �������� ��������������
        SetListenerAnswerTrueToggles();

        //�������� ������ �������� � ����� � ���������� ������ �����
        _addBtn.gameObject.SetActive(false);
        _nextBtn.gameObject.SetActive(true);
        _backBtn.gameObject.SetActive(false);

        //����������� ����
        RedrawWindow();
    }

    /// <summary>
    /// ���������� ��� ���� � ��������� � ���� ������ �������
    /// </summary>
    void ViewTaskList()
    {
        ResetAllFields();
        this._taskManager.ViewTaskList();
    }

    /// <summary>
    /// �������� ����� � �������
    /// </summary>
    void HideErrorText()
    {
        _errorText.text = "";
        _errorText.gameObject.SetActive(false);
    }

    /// <summary>
    /// ���������� ����� � �������
    /// </summary>
    /// <param name="errorText">����� ������</param>
    void ViewErrorText(string errorText)
    {
        _errorText.text = errorText;
        _errorText.gameObject.SetActive(true);
    }

   /// <summary>
   /// �� �������� ����������� ��������� ���� �� ��� ���� ����� � ���� ��� ������� ��
   /// </summary>
   /// <param name="direction">�����������</param>
   /// <returns>���������� ���� �� �����</returns>
    string GetOrCreateDirectionDirectory(string direction)
    {
        //���������� ��� �������� ����� ����������� �����������
        string pathFolder;
        //����� ����� ����������� ����������� ��� ��������� png, ���� �� ���, �� �������
        switch (direction)
        {
            case "��������":
                pathFolder = Application.dataPath + "/Resources/Mechanics";
                if (!Directory.Exists(pathFolder)) Directory.CreateDirectory(pathFolder);
                this._taskBoardInformation = this._globalOptions.boardsInfo[0];
                break;
            case "������������ ������":
                pathFolder = Application.dataPath + "/Resources/Molecular";
                if (!Directory.Exists(pathFolder)) Directory.CreateDirectory(pathFolder);
                this._taskBoardInformation = this._globalOptions.boardsInfo[2];
                break;
            case "�������������":
                pathFolder = Application.dataPath + "/Resources/Electricity";
                if (!Directory.Exists(pathFolder)) Directory.CreateDirectory(pathFolder);
                this._taskBoardInformation = this._globalOptions.boardsInfo[1];
                break;
            default:
                pathFolder = "";
                break;
        }
        return pathFolder;
    }

    /// <summary>
    /// ��������� ��� ������ ������� �� ����������
    /// </summary>
    /// <param name="pathFolder">���� �� ���������� � ���������</param>
    /// <returns>���������� ������ ������� ������� ��� ������ ����������</returns>
    List<int> GetTasksNumbersListFromDirectory(string pathFolder)
    {
        //�������� ��� �����
        string[] allFiles = Directory.GetFiles(pathFolder);
        List<int> questionNumbersList = new List<int>();
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
                //����� ������ �������
                int numberQuestion = Convert.ToInt32(file.Substring(indexSlash + 2, indexDote - indexSlash - 2));
                questionNumbersList.Add(numberQuestion);
            }
        }
        return questionNumbersList;
    }
}
