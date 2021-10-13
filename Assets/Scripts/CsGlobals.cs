using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class TaskBoardInformation
{
    private bool welcomeMessageMode;
    public bool WelcomeMessageMode { get { return welcomeMessageMode; } set { welcomeMessageMode = value; } }

    byte numberOfQuestions;
    string materialWelcome;
    string[] materialBoardPaths;
    string[,] materialQuestionsPaths;
    byte[] rightAnswers;
    bool[] questionsRightAnswered;
    private byte countQuestionsRightAnswered;
    public byte CountQuestionsRightAnswered { get { return currentQuestion; } }
    private byte currentQuestion;
    public byte CurrentQuestion { get { return currentQuestion; } }
    bool keyWasGiven;

    public TaskBoardInformation(string room)
    {
        welcomeMessageMode = true;
        numberOfQuestions = 10;
        materialWelcome = "W.png";
        materialBoardPaths = new string[10];
        for (int i = 0; i < 10; i++)
        {
            materialBoardPaths[i] = room + "/Q" + (i + 1) + ".png";            
        }
        materialQuestionsPaths = new string[10, 3];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                materialQuestionsPaths[i, j] = room + "/Q" + (i + 1) + "/" + (j + 1) + ".png";                
            }
        }
        rightAnswers = new byte[10] { 1, 0, 0, 2, 1, 1, 0, 1, 2, 0 };
        questionsRightAnswered = new bool[10]; // �� ��������� false
        countQuestionsRightAnswered = 0;
        currentQuestion = 0;
        keyWasGiven = false;
    }

    public bool NextQuestion(bool isFirstQuestion, ref ObjectMaterials board, ref ObjectMaterials[] plates)
    {
        try
        {
            if (currentQuestion == numberOfQuestions-1) return false;
            if (!isFirstQuestion) currentQuestion++;            

            board.SetTexture(materialBoardPaths[currentQuestion]);
            for (int i = 0; i < plates.Length; i++)
            {                
                plates[i].SetTexture(materialQuestionsPaths[currentQuestion, i]);
            }            
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
    }

    public bool CheckAnswer(byte answerNum, byte questionNum)
    {
        if (rightAnswers[questionNum] == answerNum)
        {
            questionsRightAnswered[questionNum] = true;
            countQuestionsRightAnswered++;
            return true;
        }
        else
        {
            questionsRightAnswered[questionNum] = false;
            return false;
        }
    }

    public byte NumberOfQuestions { get { return numberOfQuestions; } }
    public string[] MaterialBoardPaths { get { return materialBoardPaths; } }
    public string MaterialWelcome { get { return materialWelcome; } }
    
}

public class ObjectMaterials
{
    Material frontMaterial;
    public Material FrontMaterial { get { return frontMaterial; } }
    Texture frontTexture;

    public ObjectMaterials()
    {
        frontMaterial = new Material(Shader.Find("Standard"));
    }

    public void SetTexture(string pathFromResources)
    {        
        string fullPath = Application.dataPath + "/Resources/" + pathFromResources;
        WWW www = new WWW("file://" + fullPath);
        Texture _texture = www.texture;
        frontMaterial.mainTexture = _texture;
    }
}


[System.Serializable]
public class PlayerInfo
{
    // "���������" - ������� ��� ���������� � ������ �����
    public bool active_key = false;

    // �������� ����    
    public int rightAnswersGivenCount; // ���� ������ �������
    public bool[] roomsOpen; // ����� �������� ������
    public bool[,] rightAnswersGiven; //����� ������ ������� � ������ �������



    public int points = 0; // ��������� ����
    public byte cur_rank = 0; // ������� ������ (�����)
    

    public PlayerInfo()
    {
        rightAnswersGivenCount = 0;
        roomsOpen = new bool[3] {true, false, false};
        rightAnswersGiven = new bool[3, 10];
    }
}




public class CsGlobals : MonoBehaviour
{
    public TaskBoardInformation b1;
    public GameObject player; // ������� ������ - �����
    public int rooms = 3; // ���������� ��������� � ���� ������ � ���������
    public int max_answers = 30; // ����������� ��������� ����� �������
    public string[] ranks; // �������� ���� ������� - ������ �����


    public PlayerInfo playerInfo;

    public Vector3 null_position = new Vector3(-100, -100, -100); // ������� ������� - ���������� �� ��������� ������� �����
                                                                  // ���� ������������ ��� �������, ������� ����� "���������"
    public bool startMessageIsShowing = true;
    public bool endMessageIsShowing = false;
    // ������ �� ���������� ������� - ��������� ���������, ������ � ����.
    public GameObject textUI_startMessage;
    public GameObject textUI_endMessage;
    public GameObject textUI_question;
    public GameObject textUI_pressF;
    public GameObject textUI_lockedDoor;
    public GameObject key;
    public GameObject keyIcon;
    public GameObject crownIcon;
    
    

    

    /* ������ � ����������:
    CsGlobals gl;

    void Start()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
    }
    */

    void Start()
    {
        playerInfo = new PlayerInfo(); //��������� ������������� ������
        // b1 = new TaskBoard("Mechanics");

        textUI_startMessage.SetActive(true);
        textUI_question.SetActive(false);
        textUI_endMessage.SetActive(false);
        textUI_pressF.SetActive(false);
        textUI_lockedDoor.SetActive(false);
        key.transform.position = null_position;
        keyIcon.SetActive(false);
        crownIcon.SetActive(false);        
    }

    void Update()
    {
        if (startMessageIsShowing && Input.GetKeyDown(KeyCode.F))
        {
            startMessageIsShowing = false;
            textUI_startMessage.SetActive(false);
            textUI_question.SetActive(true);
        }

        if (playerInfo.active_key && playerInfo.rightAnswersGivenCount >= rooms*7)
        {
            if (playerInfo.points >= rooms*70)
            {
                crownIcon.transform.localPosition = new Vector3(0, 245, 0);
                crownIcon.SetActive(true);
            }
            textUI_question.SetActive(false);
            endMessageIsShowing = true;
            playerInfo.active_key = false;
            keyIcon.SetActive(false);
            textUI_endMessage.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = 
                "�������! �� ������� �� ����� ��������, � ���������� ������ " + playerInfo.points + 
                " ����� � ������� ������ " + ranks[playerInfo.cur_rank] +
                ".\n�� ������ �������� �� �������, �������� �� ���������� �������, �������� ��� ���������� ������ ������� ��� ����� �� ����.\n�� ������ ������!" +
                "\n\n����� F, ����� ������� ����.";
            textUI_endMessage.SetActive(true);            
        }

        if (endMessageIsShowing && Input.GetKeyDown(KeyCode.F))
        {
            crownIcon.transform.localPosition = new Vector3(0, 60, 0);
            endMessageIsShowing = false;
            textUI_endMessage.SetActive(false);
            textUI_question.SetActive(true);            
        }       
    }
}
