using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTeacherTaskChange : MonoBehaviour
{
    private CsGlobals gl;
    string jwt;

    private Text textTitleType;
    
    private GameObject buttonTasksList;
    private GameObject buttonNext;
    private GameObject buttonBack;
    private GameObject buttonChoosePicture;

    private GameObject image;
    private GameObject inputText;

    private Toggle toggleTextOrPicture;
    private Toggle toggleCheckRightAnswer;

    public QuestionWithAnswersInfo questionWithAnswersInfo;
    private int step;

    public class QuestionWithAnswersInfo
    {
        string questionText;
        string questionImage;
        bool isQuestionText;

        string[] answersTexts;
        string[] answersImages;
        bool[] isAnswersTexts;

        byte countRightAnswers;

        public QuestionWithAnswersInfo()
        {
            answersTexts = new string[3];
            answersImages = new string[3];
            isAnswersTexts = new bool[3];
        }

        public void SetQuestionInfo(string question)
        {
            isQuestionText = true;
            questionText = question;
        }

        /// <summary>
        /// Порядковые номера вопросов от 1 до 3
        /// </summary>
        public bool SetAnswerInfo(string answer, byte number)
        {
            if (number < 1 || number > 3) return false;
            number--;
            isAnswersTexts[number] = true;
            answersTexts[number] = answer;
            return true;
        }
    }

    private void Awake()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
        jwt = gl.playerInfo.responseUserData.jwt;

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
    }

    private void RefreshFields()
    {
        if (step == 0)
        {
            //поле для ввода вопроса
            textTitleType.text = "Вопрос:";
            toggleCheckRightAnswer.transform.gameObject.SetActive(false);
            buttonBack.SetActive(false);
            buttonNext.SetActive(true);


        }
        else
        {
            //поле для ввода ответа
            textTitleType.text = "Ответ "+ step + ":";
            toggleCheckRightAnswer.transform.gameObject.SetActive(true);
            buttonBack.SetActive(true);
            if (step == 3)
                buttonNext.SetActive(false);
            else
                buttonNext.SetActive(true);

        }
    }
}
