using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    private string jwt;
    private int groupID;
    public int GroupID { set { groupID = value; } }

    private enum Info { text, image } // --- в каком виде выводится информация на элемент

    Renderer attachedFrontPlane;
    TextMeshPro attachedBoardText;

    List<GameObject> attachedAnswerPlates;
    List<Renderer> attachedAnswerPlates_FrontPlates;    
    List<TextMeshPro> attachedAnswerPlates_Texts;
    List<UpBlock> attachedAnswerPlates_UpBlocks;

    Material defaultBoardMaterial;
    Material defaultPlateMaterial;

    List<Test> tests;
    List<bool> testsCheckIfResultExists;
    ResponseTestWithQuestion testWithQuestion;

    private int boardMode;
    private bool isStandingOnThePlatform;
    public bool IsStandingOnThePlatform { set { isStandingOnThePlatform = value; } get { return isStandingOnThePlatform; } }
    private bool isWaitingAction; // --- ожидается ли действие (выбор таблички) со стороны игрока
    private int cursor;
    private List<int> chosenAnswers;

    private void Awake()
    {
        jwt = DataHolder.PlayerInfo.responseUserData.jwt;
        attachedFrontPlane = this.transform.Find("frontPlane").gameObject.GetComponent<Renderer>();
        attachedBoardText = this.transform.Find("BoardText").gameObject.GetComponent<TextMeshPro>();

        attachedAnswerPlates = new List<GameObject>();
        attachedAnswerPlates_FrontPlates = new List<Renderer>();
        attachedAnswerPlates_Texts = new List<TextMeshPro>();
        attachedAnswerPlates_UpBlocks = new List<UpBlock>();
        for (byte i = 0; i < 3; i++)
        {
            attachedAnswerPlates.Add(this.transform.Find("AnswerPlate" + (i + 1)).gameObject);
            attachedAnswerPlates_FrontPlates.Add(attachedAnswerPlates[i].transform.Find("FrontPlate").GetComponent<Renderer>());
            attachedAnswerPlates_Texts.Add(attachedAnswerPlates[i].transform.Find("frontPlateText").GetComponent<TextMeshPro>());
            attachedAnswerPlates_UpBlocks.Add(attachedAnswerPlates[i].transform.Find("plateTrigger").GetComponent<UpBlock>());            
        }
        LoadDefaultMaterials();
        SwitchModeTo(0); // доска неактивна                
    }

    public async void LoadGroupTests(int Id)
    {
        groupID = Id;
        var response = await TestService.getAllGroupTests(jwt, groupID);
        if (response.data != null)
        {
            tests = new List<Test>();
            testsCheckIfResultExists = new List<bool>();
            foreach (Test t in response.data)
            {
                if (t.isOpened) tests.Add(t);
                var checkResult = await TestService.getStudentTestResult(jwt, DataHolder.PlayerInfo.responseUserData.user.userId, t.testId);
                testsCheckIfResultExists.Add(!checkResult.isError);
            }
                
            if (tests.Count > 0)
            {
                WriteOnBoard(Info.text, "Доступных тестов: " + tests.Count + "\nВстань на платформу, чтобы начать");
                SwitchModeTo(1); // выбор теста
                return;
            }
        }

        SwitchModeTo(0); // доска неактивна
        WriteOnBoard(Info.text, "В группе нет активных тестов.\nПриходи в следующий раз!");
    }

    private void WriteOnBoard(Info type, string text)
    {
        switch (type)
        {
            case Info.text:
                attachedFrontPlane.material = defaultBoardMaterial;
                attachedBoardText.text = text;
                break;
            case Info.image:
                attachedBoardText.text = "";
                attachedFrontPlane.material.mainTexture = LoadTexture(text);
                break;
            default:
                break;
        }
    }

    private void WriteOnPlate(byte num, Info type, string text)
    {
        switch (type)
        {
            case Info.text:
                attachedAnswerPlates_FrontPlates[num].material = defaultPlateMaterial;
                attachedAnswerPlates_Texts[num].text = text;
                break;
            case Info.image:
                attachedAnswerPlates_Texts[num].text = "";
                attachedAnswerPlates_FrontPlates[num].material.mainTexture = LoadTexture(text);
                break;
            default:
                break;
        }
    }

    private void SetPlateActive(byte num, bool setActive)
    {
        attachedAnswerPlates[num].SetActive(setActive);
    }

    private void HideAllPlates()
    {
        foreach (GameObject plate in attachedAnswerPlates)
            plate.SetActive(false);
    }

    private void ShowAllPlates()
    {
        foreach (GameObject plate in attachedAnswerPlates)
            plate.SetActive(true);
    }

    private Texture LoadTexture(string url)
    {
        int count = 0;
        WWW www = new WWW(url);
        while (!www.isDone) count++;
        return www.texture;
    }

    private void LoadDefaultMaterials()
    {
        defaultBoardMaterial = new Material(Shader.Find("Standard"));
        defaultPlateMaterial = new Material(Shader.Find("Standard"));

        WWW www;
        string path = Application.dataPath + "/Resources/";
        www = new WWW("file://" + path + "boardBackground.png");
        defaultBoardMaterial.mainTexture = www.texture;
        www = new WWW("file://" + path + "plateBackground.png");
        defaultPlateMaterial.mainTexture = www.texture;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isStandingOnThePlatform)
        {
            switch (boardMode)
            {
                // доска неактивна
                case 0:
                    DataHolder.ChangeMessageTemporary("Взаимодействие недоступно");
                    break;
                // начать выбор теста
                case 1:
                    DataHolder.ChangeMessageTemporary("Нажми F, чтобы выбрать тест");
                    if (Input.GetKeyDown(KeyCode.F))
                        ChooseTest_MODE();                    
                    break;
                // выбор теста
                case 2:
                    DataHolder.ChangeMessageTemporary("Используй таблички, чтобы листать список");
                    break;
                // прохождение теста
                case 3:
                    DataHolder.ChangeMessageTemporary("Используй таблички, чтобы выбрать правильный ответ");
                    break;
                ///сброс
                case 666:
                    DataHolder.ChangeMessageTemporary("Нажми F, чтобы перезагрузить доску");
                    if (Input.GetKeyDown(KeyCode.F))
                        Reset_MODE();
                    break;
                default:
                    break;
            }
        } 
        
        if (isWaitingAction)
        {
            switch (boardMode)
            {
                // выбор теста (игрок листает список)
                case 2:
                    if (Input.GetKeyDown(KeyCode.F))
                        ChooseTest_CHECK();
                    break;
                // последовательный выбор ответов
                case 3:
                    if (Input.GetKeyDown(KeyCode.F))
                        ShowQuestion_CHECK();
                    break;
                default:
                    break;
            }

        }
    }

    void SwitchModeTo(int newMode)
    {
        switch (newMode)
        {
            case 0:
                boardMode = 0;
                HideAllPlates();
                isWaitingAction = false;
                break;
            case 1:
                boardMode = 1;
                HideAllPlates();
                isWaitingAction = false;
                break;
            case 2:
                boardMode = 2;
                ShowAllPlates();
                isWaitingAction = true;
                break;
            case 3:
                boardMode = 3;
                chosenAnswers = new List<int>();
                ShowAllPlates();
                isWaitingAction = true;
                break;
            case 666:
                boardMode = 666;
                HideAllPlates();
                isWaitingAction = false;
                break;
            default:
                boardMode = 0;
                HideAllPlates();
                isWaitingAction = false;
                break;
        }
    }

    void ChooseTest_MODE()
    {
        // foreach (GameObject plate in attachedAnswerPlates) plate.SetActive(true);
        WriteOnPlate(0, Info.text, "<---");        
        WriteOnPlate(1, Info.text, "Начать тест");        
        WriteOnPlate(2, Info.text, "--->");               

        cursor = 0;
        ShowTestInfoOnBoard();
        SwitchModeTo(2); // выбор теста (стрелочками)
    }

    void ShowTestInfoOnBoard()
    {
        if (cursor < 0) cursor = tests.Count - 1;
        if (cursor >= tests.Count) cursor = 0;

        string addString = testsCheckIfResultExists[cursor]? "\nТест уже пройден" : "\nТест не пройден";
        WriteOnBoard(Info.text, tests[cursor].title + addString + "\nНачать?");
    }

    async Task<bool> LoadTestQuestions()
    {
        var response = await TestService.getTestWithQuestion(jwt, tests[cursor].testId);
        if (response.data != null)
        {
            testWithQuestion = response.data;            
            return true;
        }        
        return false;
    }

    void ShowQuestion()
    {
        //загрузка вопроса на доску и ответов на таблички
        Info info = testWithQuestion.questions[cursor].isText ? Info.text : Info.image;
        WriteOnBoard(info, testWithQuestion.questions[cursor].question);

        for (byte i=0; i<3; i++)
        {
            info = testWithQuestion.questions[cursor].answers[i].isText ? Info.text : Info.image;
            WriteOnPlate(i, info, testWithQuestion.questions[cursor].answers[i].answer);
        }
    }

    void ShowQuestion_CHECK()
    {
        bool check = false;
        byte i = 0;
        while (!check && i < 3)
        {
            if (attachedAnswerPlates_UpBlocks[i].IsPlateUp)
            {
                chosenAnswers.Add(i);
                check = true;
            }
            i++;   
        }
        if (check)
        {
            cursor++;
            if (cursor == testWithQuestion.questions.Count)
                FinishTest(); // даны ответы на все вопросы, завершить тест
            else ShowQuestion(); // показать следующий вопрос
        }        
    }

    async void FinishTest()
    {        
        if (chosenAnswers.Count == testWithQuestion.questions.Count)
        {
            List<AnswerResultQuestionDto> answers = new List<AnswerResultQuestionDto>();
            for (byte i = 0; i < testWithQuestion.questions.Count; i++)
                answers.Add(new AnswerResultQuestionDto(testWithQuestion.questions[i].questionId, testWithQuestion.questions[i].answers[chosenAnswers[i]].answerId));

            var response = await ResultService.createResult(jwt, testWithQuestion.testId, answers);
            if (response.isError)
            {
                switch (response.message)
                {
                    case Message.AccessDenied:
                        WriteOnBoard(Info.text, "Тест закрыт, невозможно отправить ответы");                        
                        break;
                    default:
                        Debug.LogError(response.message.ToString());
                        break;
                }                
            }
            else
            {
                var maxScores = await TestService.getMaxScoresForTestByTestId(jwt, testWithQuestion.testId);
                WriteOnBoard(Info.text, "Результат теста: " + response.data.totalScores + " баллов из " + maxScores?.data);                
            }
        }
        else
        {
            WriteOnBoard(Info.text, "Упс, что-то пошло не так");
            Debug.Log("chosenAnswers: " + chosenAnswers.Count + "/" + testWithQuestion.questions.Count);            
        }
        SwitchModeTo(666);
    }

    async void ChooseTest_CHECK()
    {
        // начать тест
        if (attachedAnswerPlates_UpBlocks[1].IsPlateUp)
        {
            SwitchModeTo(0); // на время загрузки
            if (await LoadTestQuestions())
            {
                //показать первый вопрос
                cursor = 0;
                ShowQuestion();
                SwitchModeTo(3);
            }
            else
            {
                WriteOnBoard(Info.text, "Упс, что-то пошло не так");
                SwitchModeTo(666);                
            }                
            return;
        }

        // <---
        if (attachedAnswerPlates_UpBlocks[0].IsPlateUp)
            cursor--;
        // --->
        if (attachedAnswerPlates_UpBlocks[2].IsPlateUp)
            cursor++;
        ShowTestInfoOnBoard();
    }

    void Reset_MODE()
    {
        // ждём ОК и перезагружаемся
        LoadGroupTests(groupID); //перезагрузить тесты группы
    }
}
