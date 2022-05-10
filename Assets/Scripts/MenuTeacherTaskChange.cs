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
        public string questionImagePath;
        public bool isQuestionText;
        public Texture questionTexture;

        public string[] answersTexts;
        public string[] answersImagePath;
        public bool[] isAnswersTexts;
        public bool[] isAnswersRight;
        public Texture[] answersTexture;

        public bool isNew; //флаг, является ли вопрос новым или происходит редактирование
        bool[] isFieldSet;

        public int questionId;
        public int[] answersId;

        public QuestionWithAnswersInfo()
        {
            AllocateMemory();
            isNew = true;
        }

        public QuestionWithAnswersInfo(ResponseQuestionWithAnswers question)
        {
            AllocateMemory();
            answersId = new int[3];
            isNew = false;

            WWW www;            
            int count = 0;            

            isQuestionText = question.isText;
            if (isQuestionText)
                questionText = question.question;
            else
            {
                questionText = "Вопрос в виде картинки";                
                questionImagePath = question.question;

                www = new WWW(questionImagePath);
                while (!www.isDone) count++;             
                questionTexture = www.texture; 
            }
            questionId = question.questionId;
            isFieldSet[0] = true;

            for (int i=0; i<3; i++)
            {
                isAnswersRight[i] = question.answers[i].isRightAnswer;
                isAnswersTexts[i] = question.answers[i].isText;
                if (isAnswersTexts[i])
                    answersTexts[i] = question.answers[i].answer;
                else
                {
                    answersTexts[i] = "Ответ в виде картинки";                    
                    answersImagePath[i] = question.answers[i].answer;

                    www = new WWW(answersImagePath[i]);
                    while (!www.isDone) count++;
                    answersTexture[i] = www.texture;
                }
                answersId[i] = question.answers[i].answerId;
                isFieldSet[i+1] = true;
            }         
        }       

        void AllocateMemory()
        {
            answersTexts = new string[3];
            answersImagePath = new string[3];
            isAnswersTexts = new bool[3];
            isAnswersRight = new bool[3];
            answersTexture = new Texture[3];
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
            questionText = "Вопрос в виде картинки";
        }

        /// <summary>
        /// Порядковые номера вопросов от 1 до 3
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
            answersTexts[number] = "Ответ в виде картинки";
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

        public string GetQuestionString()
        {
            if (isQuestionText)
                return questionText;
            else
                return questionImagePath;
        }

        public string GetAnswerString(byte num)
        {
            if (isAnswersTexts[num])
                return answersTexts[num];
            else
                return answersImagePath[num];
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

        buttonChoosePicture.GetComponent<Button>().onClick.AddListener(delegate
        {
            OpenExplorer();
        });
    }

    static public Texture LoadTexture(string path)
    {
        WWW www = new WWW(path);
        return www.texture;
    }

    private void OpenExplorer()
    {        
        OpenFileName openFileName = new OpenFileName();        
        if (LocalDialog.GetOpenFileName(openFileName))
        {
            Texture t = LoadTexture(openFileName.file);
            if (step == 0)
            {
                questionWithAnswersInfo.questionImagePath = openFileName.file;
                questionWithAnswersInfo.questionTexture = t;
            }
            else
            {
                questionWithAnswersInfo.answersImagePath[step-1] = openFileName.file;
                questionWithAnswersInfo.answersTexture[step-1] = t;
            }
            image.GetComponent<RawImage>().texture = t;
        }        
    }

    private void ButtonBack()
    {
        if (SaveFields(false))
        {
            step--;
            RefreshFields();
            SetFieldsContent();
        }
    }

    private async void ButtonNext()
    {
        if (SaveFields(true))
        {
            if (step < 3)
            {
                step++;
                RefreshFields();
                SetFieldsContent();
            }
            else
            {
                //проверка и сохранение вопроса на сервере
                if (questionWithAnswersInfo.CheckIfExistsRightAnswer())
                {
                    if (await SaveToServer())
                    {
                        this.gameObject.SetActive(false);
                        menuTasksList.SetActive(true);
                        this.gameObject.GetComponentInParent<MenuTeacherTasksEditor>().UpdateQuestionsList();
                        gl.ChangeMessageTemporary("Вопрос успешно сохранен", 5);
                    }
                }
                else
                    gl.ChangeMessageTemporary("Отметьте хотя бы один правильный ответ", 5);
            }
        }
    }

    async Task<bool> NewQuestion()
    {
        int testId = this.gameObject.GetComponentInParent<MenuTeacherTasksEditor>().GetTestId();
        var responseQuestion = await QuestionService.createQuestion(jwt, testId,
            questionWithAnswersInfo.isQuestionText, questionWithAnswersInfo.GetQuestionString(), 10);
        if (responseQuestion.isError)
        {
            switch (responseQuestion.message)
            {
                case Message.CanNotLoadFile:
                    gl.ChangeMessageTemporary("Не удалось загрузить файл вопроса", 5);
                    break;
                case Message.CanNotPublishFile:
                    gl.ChangeMessageTemporary("Не удалось сделать файл вопроса публичным", 5);
                    break;
                case Message.NotFoundRequiredData:
                    gl.ChangeMessageTemporary("Не удалось загрузить файл вопроса. Попробуйте уменьшить размер файла", 10);
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
            for (byte i = 0; i < 3; i++)
            {                
                responseAnswer = await AnswerService.createAnswer(jwt, responseQuestion.data.questionId,
                    questionWithAnswersInfo.GetAnswerString(i), questionWithAnswersInfo.isAnswersTexts[i], questionWithAnswersInfo.isAnswersRight[i]);
                if (responseAnswer.isError)
                {
                    switch (responseAnswer.message)
                    {
                        case Message.CanNotLoadFile:
                            gl.ChangeMessageTemporary("Не удалось загрузить файл (ответ " + (i + 1) + ")", 5);
                            break;
                        case Message.CanNotPublishFile:
                            gl.ChangeMessageTemporary("Не удалось сделать файл публичным (ответ " + (i + 1) + ")", 5);
                            break;
                        case Message.NotFoundRequiredData:
                            gl.ChangeMessageTemporary("Не удалось загрузить файл (ответ " + (i + 1) + "). Попробуйте уменьшить размер файла", 10);
                            break;
                        default:
                            gl.ChangeMessageTemporary(responseAnswer.message.ToString(), 5);
                            break;
                    }
                    //если что-то пошло не так хотя бы с одним из ответов, удаляем тест
                    await QuestionService.delete(jwt, responseQuestion.data.questionId);
                    return false;
                }
            }            
        }
        return true;
    }

    async Task<bool> EditQuestion()
    {        
        //пока сохранение только текстовых
        var responseQuestion = await QuestionService.updateQuestion(jwt, questionWithAnswersInfo.questionId,
                    questionWithAnswersInfo.isQuestionText, questionWithAnswersInfo.GetQuestionString(), 10);
        if (responseQuestion.isError)
        {
            switch (responseQuestion.message)
            {
                case Message.CanNotLoadFile:
                    gl.ChangeMessageTemporary("Не удалось загрузить файл вопроса", 5);
                    break;
                case Message.CanNotPublishFile:
                    gl.ChangeMessageTemporary("Не удалось сделать файл вопроса публичным", 5);
                    break;
                case Message.NotFoundRequiredData:
                    gl.ChangeMessageTemporary("Не удалось загрузить файл вопроса. Попробуйте уменьшить размер файла", 10);
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
            for (byte i = 0; i < 3; i++)
            {
                Debug.Log("answer id: " + questionWithAnswersInfo.answersId[i]);
                responseAnswer = await AnswerService.updateAnswer(jwt, questionWithAnswersInfo.answersId[i], questionWithAnswersInfo.questionId,
                    questionWithAnswersInfo.GetAnswerString(i), questionWithAnswersInfo.isAnswersTexts[i], questionWithAnswersInfo.isAnswersRight[i]);                    
                if (responseAnswer.isError)
                {
                    switch (responseAnswer.message)
                    {
                        case Message.CanNotLoadFile:
                            gl.ChangeMessageTemporary("Не удалось загрузить файл (ответ " + (i + 1) + ")", 5);
                            break;
                        case Message.CanNotPublishFile:
                            gl.ChangeMessageTemporary("Не удалось сделать файл публичным (ответ " + (i + 1) + ")", 5);
                            break;
                        case Message.NotFoundRequiredData:
                            gl.ChangeMessageTemporary("Не удалось загрузить файл (ответ " + (i + 1) + "). Попробуйте уменьшить размер файла", 10);
                            break;
                        default:
                            gl.ChangeMessageTemporary(responseAnswer.message.ToString(), 5);
                            break;
                    }                    
                    return false;
                }
            }
        }
        return true;
    }

    private async Task<bool> SaveToServer()
    {
        gl.ChangeMessageTemporary("Ждите...", 30);
        if (questionWithAnswersInfo.isNew)
            return await NewQuestion();
        else
            return await EditQuestion();
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

    private bool SaveFields(bool mandatoryInput)
    {
        if (toggleTextOrPicture.isOn)
        {
            //текстовый ввод
            string input = inputText.transform.GetComponent<InputField>().text;
            if (mandatoryInput && input == "")
            {
                gl.ChangeMessageTemporary("Введите текст, чтобы продолжить", 5);
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
            //ввод в формате картинки            
            if (mandatoryInput && image.GetComponent<RawImage>().texture == null)
            {
                gl.ChangeMessageTemporary("Загрузите изображение, чтобы продолжить", 5);
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
            //поле для ввода вопроса
            textTitleType.text = "Вопрос:";
            toggleCheckRightAnswer.transform.gameObject.SetActive(false);
            buttonBack.SetActive(false);
            buttonNext.transform.Find("Text").GetComponent<Text>().text = "Далее";
        }
        else
        {
            //поле для ввода ответа
            textTitleType.text = "Ответ "+ step + ":";
            toggleCheckRightAnswer.transform.gameObject.SetActive(true);
            buttonBack.SetActive(true);
            if (step == 3)
                buttonNext.transform.Find("Text").GetComponent<Text>().text = "Сохранить";
            else
                buttonNext.transform.Find("Text").GetComponent<Text>().text = "Далее";
        }
    }

    private void SetFieldsContent()
    {      
        if (questionWithAnswersInfo.CheckIfFieldSet(step))
        {
            //информация была сохранена, вернуть её в соответствующие поля
            if (step == 0)
            {
                //вопрос
                toggleTextOrPicture.isOn = questionWithAnswersInfo.isQuestionText;
                inputText.transform.GetComponent<InputField>().text = questionWithAnswersInfo.questionText;
                image.GetComponent<RawImage>().texture = questionWithAnswersInfo.questionTexture;


            }
            else
            {
                //ответ
                toggleTextOrPicture.isOn = questionWithAnswersInfo.isAnswersTexts[step - 1];
                toggleCheckRightAnswer.isOn = questionWithAnswersInfo.isAnswersRight[step - 1];
                inputText.transform.GetComponent<InputField>().text = questionWithAnswersInfo.answersTexts[step - 1];
                image.GetComponent<RawImage>().texture = questionWithAnswersInfo.answersTexture[step - 1];
            }         
        }
        else
        {
            //вид по умолчанию
            toggleTextOrPicture.isOn = true;
            toggleCheckRightAnswer.isOn = false;
            inputText.transform.GetComponent<InputField>().text = "";
            image.GetComponent<RawImage>().texture = null;
        }        
    }
}
