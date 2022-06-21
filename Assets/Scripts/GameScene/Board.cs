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

    private enum Info { text, image } // --- � ����� ���� ��������� ���������� �� �������

    Renderer attachedFrontPlane;
    TextMeshPro attachedBoardText;

    class AnswerPlate
    {
        public GameObject answerPlate;
        public Renderer attachedFrontPlate;
        public TextMeshPro attachedText;
        public UpBlock attachedUpBlock;

        public AnswerPlate(GameObject plate)
        {
            answerPlate = plate;
            attachedFrontPlate = plate.transform.Find("FrontPlate").GetComponent<Renderer>();
            attachedText = plate.transform.Find("frontPlateText").GetComponent<TextMeshPro>();
            attachedUpBlock = plate.transform.Find("plateTrigger").GetComponent<UpBlock>();
        }
    }

    List<AnswerPlate> attachedAnswerPlates;

    /*
    List<GameObject> attachedAnswerPlates;
    List<Renderer> attachedAnswerPlates_FrontPlates;    
    List<TextMeshPro> attachedAnswerPlates_Texts;
    List<UpBlock> attachedAnswerPlates_UpBlocks;
    */

    Material defaultBoardMaterial;
    Material defaultPlateMaterial;

    List<Test> tests;
    List<bool> testsCheckIfResultExists;
    ResponseTestWithQuestion testWithQuestion;

    private int boardMode;
    private bool isStandingOnThePlatform;
    public bool IsStandingOnThePlatform { set { isStandingOnThePlatform = value; } get { return isStandingOnThePlatform; } }
    private bool isWaitingAction; // --- ��������� �� �������� (����� ��������) �� ������� ������
    private int cursor;
    private List<int> chosenAnswers;

    private void Awake()
    {
        jwt = DataHolder.PlayerInfo.responseUserData.jwt;
        attachedFrontPlane = this.transform.Find("frontPlane").gameObject.GetComponent<Renderer>();
        attachedBoardText = this.transform.Find("BoardText").gameObject.GetComponent<TextMeshPro>();

        attachedAnswerPlates = new List<AnswerPlate>();
        for (byte i = 0; i < 3; i++)
        {
            AnswerPlate newPlate = new AnswerPlate(this.transform.Find("AnswerPlate" + (i + 1)).gameObject);           
            attachedAnswerPlates.Add(newPlate);         
        }
        LoadDefaultMaterials();
        SwitchModeTo(0); // ����� ���������                
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
                var checkResult = await TestService.getStudentTestResult(jwt, DataHolder.PlayerInfo.responseUserData.user.id, t.testId);
                testsCheckIfResultExists.Add(!checkResult.isError);
            }
                
            if (tests.Count > 0)
            {
                WriteOnBoard(Info.text, "��������� ������: " + tests.Count + "\n������ �� ���������, ����� ������");
                SwitchModeTo(1); // ����� �����
                return;
            }
        }

        SwitchModeTo(0); // ����� ���������
        WriteOnBoard(Info.text, "� ������ ��� �������� ������.");
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
                attachedAnswerPlates[num].attachedFrontPlate.material = defaultPlateMaterial;
                attachedAnswerPlates[num].attachedText.text = text;
                break;
            case Info.image:
                attachedAnswerPlates[num].attachedFrontPlate.material.mainTexture = LoadTexture(text);
                attachedAnswerPlates[num].attachedText.text = "";                
                break;
            default:
                break;
        }
    }

    private void HidePlates()
    {
        foreach (AnswerPlate plate in attachedAnswerPlates)
            plate.answerPlate.SetActive(false);
    }

    private void HidePlates(byte[] nums)
    {
        foreach (byte num in nums)
            attachedAnswerPlates[num].answerPlate.SetActive(false);
    }

    private void ShowPlates()
    {
        foreach (AnswerPlate plate in attachedAnswerPlates)
            plate.answerPlate.SetActive(true);
    }

    private void ShowPlates(byte[] nums)
    {
        foreach (byte num in nums)
            attachedAnswerPlates[num].answerPlate.SetActive(true);
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
                // ����� ���������
                case 0:
                    DataHolder.ChangeMessageTemporary("����� �� �������. ����� F, ����� �������� ������");
                    if (Input.GetKeyDown(KeyCode.F))
                        Reset_MODE(); //������������� ����� ������
                    break;
                // ������ ����� �����
                case 1:
                    DataHolder.ChangeMessageTemporary("����� F, ����� ������� ����");
                    if (Input.GetKeyDown(KeyCode.F))
                        ChooseTest_MODE();                    
                    break;
                // ����� �����
                case 2:
                    string s = tests.Count > 1 ? "��������� ��������, ����� ������� � ������� ����." : "��������� ��������, ����� ������� ����.";
                    s += " ����� F, ����� �������� ������";
                    DataHolder.ChangeMessageTemporary(s);
                    if (Input.GetKeyDown(KeyCode.F))
                        Reset_MODE(); //������������� ����� ������
                    break;
                // ����������� �����
                case 3:
                    DataHolder.ChangeMessageTemporary("��������� ��������, ����� ������� ���������� �����");
                    break;
                ///�����
                case 100:
                    DataHolder.ChangeMessageTemporary("����� F, ����� ������������� �����");
                    if (Input.GetKeyDown(KeyCode.F))
                        Reset_MODE(); //������������� ����� ������
                    break;
                default:
                    break;
            }
        }
        else if (isWaitingAction)
        {
            switch (boardMode)
            {
                // ����� ����� (����� ������� ������)
                case 2:
                    if (Input.GetKeyDown(KeyCode.F))
                        ChooseTest_CHECK();
                    break;
                // ���������������� ����� �������
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
                // ����� �� �������
                boardMode = 0;
                HidePlates();
                isWaitingAction = false;
                break;
            case 1:
                // �������� ���������� ������ � ������ �����
                boardMode = 1;
                HidePlates();
                isWaitingAction = false;
                break;
            case 2:
                // ����� ������ �����
                boardMode = 2;
                if (tests.Count > 1) ShowPlates(); else ShowPlates(new byte[1] { 1 });
                isWaitingAction = true;
                break;
            case 3:
                // ����� ������� �����
                boardMode = 3;
                chosenAnswers = new List<int>();
                ShowPlates();
                isWaitingAction = true;
                break;
            case 100:
                boardMode = 100;
                HidePlates();
                isWaitingAction = false;
                break;
            default:
                boardMode = 0;
                HidePlates();
                isWaitingAction = false;
                break;
        }
    }

    void ChooseTest_MODE()
    {
        // foreach (GameObject plate in attachedAnswerPlates) plate.SetActive(true);
        WriteOnPlate(0, Info.text, "<---");        
        WriteOnPlate(1, Info.text, "������ ����");        
        WriteOnPlate(2, Info.text, "--->");               

        cursor = 0;
        ShowTestInfoOnBoard();
        SwitchModeTo(2); // ����� ����� (�����������)
    }

    void ShowTestInfoOnBoard()
    {
        if (cursor < 0) cursor = tests.Count - 1;
        if (cursor >= tests.Count) cursor = 0;

        string addString = testsCheckIfResultExists[cursor]? "\n���� ��� �������" : "\n���� �� �������";
        WriteOnBoard(Info.text, tests[cursor].title + addString + "\n������?");
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
        //�������� ������� �� ����� � ������� �� ��������
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
            if (attachedAnswerPlates[i].attachedUpBlock.IsPlateUp)
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
                FinishTest(); // ���� ������ �� ��� �������, ��������� ����
            else ShowQuestion(); // �������� ��������� ������
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
                        WriteOnBoard(Info.text, "���� ������, ���������� ��������� ������");                        
                        break;
                    default:
                        Debug.LogError(response.message.ToString());
                        break;
                }                
            }
            else
            {
                var maxScores = await TestService.getMaxScoresForTestByTestId(jwt, testWithQuestion.testId);
                WriteOnBoard(Info.text, "��������� �����: " + response.data.totalScores + " ������ �� " + maxScores?.data);                
            }
        }
        else
        {
            WriteOnBoard(Info.text, "���, ���-�� ����� �� ���");
            Debug.Log("chosenAnswers: " + chosenAnswers.Count + "/" + testWithQuestion.questions.Count);            
        }
        SwitchModeTo(100);
    }

    async void ChooseTest_CHECK()
    {
        // ������ ����
        if (attachedAnswerPlates[1].attachedUpBlock.IsPlateUp)
        {
            SwitchModeTo(0); // �� ����� ��������
            if (await LoadTestQuestions())
            {
                //�������� ������ ������
                cursor = 0;
                ShowQuestion();
                SwitchModeTo(3);
            }
            else
            {
                WriteOnBoard(Info.text, "���, ���-�� ����� �� ���");
                SwitchModeTo(100);                
            }                
            return;
        }

        // <---
        if (attachedAnswerPlates[0].attachedUpBlock.IsPlateUp)
            cursor--;
        // --->
        if (attachedAnswerPlates[2].attachedUpBlock.IsPlateUp)
            cursor++;
        ShowTestInfoOnBoard();
    }

    void Reset_MODE()
    {
        // ��� �� � ���������������
        LoadGroupTests(groupID); //������������� ����� ������
    }
}
