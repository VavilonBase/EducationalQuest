using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MenuTeacherTaskChange : MonoBehaviour
{
    private CsGlobals gl;
    string jwt;

    private Text textTitleType;

    private GameObject menuTasksList;

    private GameObject buttonTasksList;
    private GameObject buttonNext;
    private GameObject buttonBack;
    private GameObject buttonChoosePicture;

    private GameObject image;
    private GameObject inputText;

    private Toggle toggleTextOrPicture;
    private Toggle toggleCheckRightAnswer;

    public QuestionWithAnswersInfo questionWithAnswersInfo;
    private byte step;

    public class QuestionWithAnswersInfo
    {
        public string questionText;
        public string questionImage;
        public bool isQuestionText;

        public string[] answersTexts;
        public string[] answersImages;
        public bool[] isAnswersTexts;
        public bool[] isAnswersRight;

        bool[] isFieldSet;

        public QuestionWithAnswersInfo()
        {
            answersTexts = new string[3];
            answersImages = new string[3];
            isAnswersTexts = new bool[3];
            isAnswersRight = new bool[3];
            isFieldSet = new bool[4];
        }

        public void SetQuestionInfo(string question)
        {
            isFieldSet[0] = true;
            isQuestionText = true;
            questionText = question;            
        }

        public void SetQuestionInfo()
        {
            isFieldSet[0] = true;
            isQuestionText = false;
            questionText = "������ � ���� ��������";
        }

        /// <summary>
        /// ���������� ������ �������� �� 1 �� 3
        /// </summary>
        public bool SetAnswerInfo(string answer, byte number, bool isRight)
        {
            if (number < 1 || number > 3) return false;
            isFieldSet[number] = true;
            number--;
            isAnswersTexts[number] = true;
            isAnswersRight[number] = isRight;
            answersTexts[number] = answer;            
            return true;
        }

        public bool SetAnswerInfo(byte number, bool isRight)
        {
            if (number < 1 || number > 3) return false;
            isFieldSet[number] = true;
            number--;
            isAnswersTexts[number] = false;
            isAnswersRight[number] = isRight;
            answersTexts[number] = "����� � ���� ��������";
            return true;
        }

        public bool CheckIfFieldSet(byte step)
        {
            return isFieldSet[step];
        }

        public bool CheckIfExistsRightAnswer()
        {
            for (int i=0; i<3; i++)
                if (isAnswersRight[i]) return true;
            return false;
        }
    }

    private void Awake()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
        jwt = gl.playerInfo.responseUserData.jwt;

        menuTasksList = this.transform.parent.Find("UI Task List").gameObject;

        textTitleType = this.transform.Find("textTitleType").GetComponent<Text>();

        buttonTasksList = this.transform.Find("buttonTasksList").gameObject;
        buttonNext = this.transform.Find("buttonNext").gameObject;
        buttonBack = this.transform.Find("buttonBack").gameObject;
        buttonChoosePicture = this.transform.Find("buttonChoosePicture").gameObject;

        image = this.transform.Find("image").gameObject;
        inputText = this.transform.Find("inputText").gameObject;
        toggleTextOrPicture = this.transform.Find("toggleTextOrPicture").GetComponent<Toggle>();
        toggleCheckRightAnswer = this.transform.Find("toggleCheckRightAnswer").GetComponent<Toggle>();

        toggleTextOrPicture.onValueChanged.AddListener(delegate
        {
            ToggleTextOrPictureChanged();
        });

        buttonNext.GetComponent<Button>().onClick.AddListener(delegate
        {
            ButtonNext();
        });

        buttonBack.GetComponent<Button>().onClick.AddListener(delegate
        {
            ButtonBack();
        });
    }

    private void ButtonBack()
    {
        if (SaveFields())
        {
            step--;
            RefreshFields();
            SetFieldsContent();
        }
    }

    private async void ButtonNext()
    {
        if (SaveFields())
        {
            if (step < 3)
            {
                step++;
                RefreshFields();
                SetFieldsContent();
            }
            else
            {
                //�������� � ���������� ������� �� �������
                if (questionWithAnswersInfo.CheckIfExistsRightAnswer())
                {
                    if (await SaveToServer())
                    {
                        this.gameObject.SetActive(false);
                        menuTasksList.SetActive(true);
                        this.gameObject.GetComponentInParent<MenuTeacherTasksEditor>().UpdateQuestionsList();
                        gl.ChangeMessageTemporary("������ ������� ��������", 5);
                    }
                }
                else
                    gl.ChangeMessageTemporary("�������� ���� �� ���� ���������� �����", 5);
            }
        }
    }

    private async Task<bool> SaveToServer()
    {
        int testId = this.gameObject.GetComponentInParent<MenuTeacherTasksEditor>().GetTestId();
        //���� ���������� ������ ���������
        var responseQuestion = await QuestionService.createQuestion(jwt, testId, 
            questionWithAnswersInfo.isQuestionText, questionWithAnswersInfo.questionText, 10);
        if (responseQuestion.isError)
        {
            switch (responseQuestion.message)
            {
                case Message.CanNotLoadFile:
                    gl.ChangeMessageTemporary("�� ������� ��������� ���� �������", 5);
                    break;
                case Message.CanNotPublishFile:
                    gl.ChangeMessageTemporary("�� ������� ������� ���� ������� ���������", 5);
                    break;
                default:
                    gl.ChangeMessageTemporary(responseQuestion.message.ToString(), 5);
                    break;
            }            
            return false;
        }
        else
        {
            Response<Answer> responseAnswer;
            for (int i = 0; i < 3; i++)
            {
                responseAnswer = await AnswerService.createAnswer(jwt, responseQuestion.data.questionId,
                    questionWithAnswersInfo.answersTexts[i], questionWithAnswersInfo.isAnswersTexts[i], questionWithAnswersInfo.isAnswersRight[i]);
                if (responseAnswer.isError)
                {
                    switch (responseAnswer.message)
                    {
                        case Message.CanNotLoadFile:
                            gl.ChangeMessageTemporary("�� ������� ��������� ���� (����� " + (i + 1) + ")", 5);
                            break;
                        case Message.CanNotPublishFile:
                            gl.ChangeMessageTemporary("�� ������� ������� ���� ��������� (����� " + (i + 1) + ")", 5);
                            break;
                        default:
                            gl.ChangeMessageTemporary(responseAnswer.message.ToString(), 5);
                            break;
                    }
                    //���� ���-�� ����� �� ��� ���� �� � ����� �� �������, ������� ����
                    await QuestionService.delete(jwt, responseQuestion.data.questionId);
                    return false;
                }                               
            }
            return true;
        }
    }

    private void ToggleTextOrPictureChanged()
    {
        inputText.SetActive(toggleTextOrPicture.isOn);
        image.SetActive(!toggleTextOrPicture.isOn);
        buttonChoosePicture.SetActive(!toggleTextOrPicture.isOn);
    }

    private void OnEnable()
    {
        step = 0;
        toggleTextOrPicture.isOn = true;
        RefreshFields();
        SetFieldsContent();
    }

    private bool SaveFields()
    {
        if (toggleTextOrPicture.isOn)
        {
            //��������� ����
            string input = inputText.transform.GetComponent<InputField>().text;
            if (input == "")
            {
                gl.ChangeMessageTemporary("������� �����, ����� ����������", 5);
                return false;
            }
            else
            {
                if (step==0)
                    questionWithAnswersInfo.SetQuestionInfo(input);
                else
                    questionWithAnswersInfo.SetAnswerInfo(input, step, toggleCheckRightAnswer.isOn);
            }
        }
        else
        {
            //���� � ������� ��������
            string input = inputText.transform.GetComponent<InputField>().text;
            if (false)
            {
                gl.ChangeMessageTemporary("��������� �����������, ����� ����������", 5);
                return false;
            }
            else
            {
                if (step == 0)
                    questionWithAnswersInfo.SetQuestionInfo();
                else
                    questionWithAnswersInfo.SetAnswerInfo(step, toggleCheckRightAnswer.isOn);
            }
        }
        return true;
    }

    private void RefreshFields()
    {        
        if (step == 0)
        {
            //���� ��� ����� �������
            textTitleType.text = "������:";
            toggleCheckRightAnswer.transform.gameObject.SetActive(false);
            buttonBack.SetActive(false);
            buttonNext.transform.Find("Text").GetComponent<Text>().text = "�����";
        }
        else
        {
            //���� ��� ����� ������
            textTitleType.text = "����� "+ step + ":";
            toggleCheckRightAnswer.transform.gameObject.SetActive(true);
            buttonBack.SetActive(true);
            if (step == 3)
                buttonNext.transform.Find("Text").GetComponent<Text>().text = "���������";
            else
                buttonNext.transform.Find("Text").GetComponent<Text>().text = "�����";
        }
    }

    private void SetFieldsContent()
    {      
        if (questionWithAnswersInfo.CheckIfFieldSet(step))
        {
            //���������� ���� ���������, ������� � � ��������������� ����
            if (step == 0)
            {
                //������
                toggleTextOrPicture.isOn = questionWithAnswersInfo.isQuestionText;
                inputText.transform.GetComponent<InputField>().text = questionWithAnswersInfo.questionText;
                //����������� ��������
                
            }
            else
            {
                //�����
                toggleTextOrPicture.isOn = questionWithAnswersInfo.isAnswersTexts[step - 1];
                toggleCheckRightAnswer.isOn = questionWithAnswersInfo.isAnswersRight[step - 1];
                inputText.transform.GetComponent<InputField>().text = questionWithAnswersInfo.answersTexts[step - 1];
                //����������� ��������
            }         
        }
        else
        {
            //��� �� ���������
            toggleTextOrPicture.isOn = true;
            toggleCheckRightAnswer.isOn = false;
            inputText.transform.GetComponent<InputField>().text = "";
            //����������� ��������
        }        
    }
}
