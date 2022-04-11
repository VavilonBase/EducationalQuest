using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.UI;

[System.Serializable]
public class PlayerInfo
{
    public ResponseUserData responseUserData;
    public bool isAuthorized = false;

    // "инвентарь" - наличие или отсутствие у игрока ключа
    private bool activeKey = false;    
    public bool ActiveKey { get { return activeKey; } }

    private byte keysCount = 0;
    public byte KeysCount { get { return keysCount; } }

    // прогресс игры    
    private byte[] numRightAnswersGiven; // дано верных ответов
    public byte[] NumRightAnswersGiven { get { return numRightAnswersGiven; } }
    public void SetNumOfRightAnswers(byte numRoom, byte numRightAnsw)
    {
        numRightAnswersGiven[numRoom] = numRightAnsw;
    }

    public byte NumRightAnswersTotal 
    { 
        get 
        {
            byte sum = 0;
            for (int i = 0; i < numRightAnswersGiven.Length; i++) sum += numRightAnswersGiven[i];
            return sum;
        } 
    }

    
    public bool[] roomsOpen; // флаги открытых комнат    



    public int points = 0; // набранные очки
    public byte cur_rank = 0; // текущее звание (титул)
    

    public PlayerInfo()
    {
        numRightAnswersGiven = new byte[3];        
        roomsOpen = new bool[3] {true, false, false};
        //rightAnswersGiven = new bool[3, 10];
    }

    public PlayerInfo(byte[] numRightAnswers, bool[] roomsOp, bool key, byte keysC)
    {
        numRightAnswersGiven = numRightAnswers;
        roomsOpen = roomsOp;
        activeKey = key;
        keysCount = keysC;
    }
    public void GetKey()
    {
        keysCount++;
        activeKey = true;
    }
    public void PutAwayKey(byte numRoom)
    {
        activeKey = false;
        roomsOpen[numRoom] = true;
    }
}




public class CsGlobals : MonoBehaviour
{    
    public GameObject player; // игровой объект - игрок
    public PlayerInfo playerInfo;
    public GameObject messageDurable; //постоянно висящая табличка (вверху)
    public GameObject messageTemporary; //временно появляющаяся табличка (внизу)
    public GameObject menuStart;

    public TaskBoardInformation[] boardsInfo;
    public bool RELOAD = false;
    public byte RELOADcount = 0;

    private int questionsCount = 30;
    public int QuestionsCount { get { return questionsCount; } }
    public void RefreshQuestionsCount()
    {
        questionsCount = 0;
        for (byte i = 0; i < boardsInfo.Length; i++) questionsCount += boardsInfo[i].NumberOfQuestions;
    }

    //public int rooms = 3; // количество доступных в игре комнат с вопросами
    //public int max_answers = 30; // максимально возможное число ответов
    public string[] ranks; // перечень всех титулов - массив строк


    

    public Vector3 null_position = new Vector3(-100, -100, -100); // нулевая позиция - координаты за пределами видимой сцены
                                                                  // туда отправляются все объекты, которым нужно "исчезнуть"
    public bool startMessageIsShowing = true;
    public bool endMessageIsShowing = false;
    // ссылки на визуальные объекты - текстовые сообщения, иконки и проч.
    public GameObject textUI_startMessage;
    public GameObject textUI_endMessage;
    public GameObject textUI_question;
    public GameObject textUI_pressF;
    public GameObject textUI_lockedDoor;
    public GameObject key;
    public GameObject keyIcon;
    public GameObject crownIcon;
    
    public void PrintLabel(string text)
    {
        textUI_pressF.GetComponent<UnityEngine.UI.Text>().text = text;
        textUI_pressF.SetActive(true);
    }

    public void HideLabel()
    {
        textUI_pressF.SetActive(false);
    }



    /* Доступ к переменным:
    CsGlobals gl;

    void Start()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
    }
    */

    private IEnumerator ChangeMessageTemporaryIE(string newMessage, float time)
    {
        //показываем табличку с сообщением
        messageTemporary.transform.Find("Text").GetComponent<Text>().text = newMessage;
        messageTemporary.SetActive(true);

        //ждём
        yield return new WaitForSeconds(time);

        //убираем табличку
        messageTemporary.SetActive(false);
    }

    public void ChangeMessageTemporary(string newMessage, float time)
    {
        StartCoroutine(ChangeMessageTemporaryIE(newMessage, time));
    }

    public void ChangeMessageDurable(bool active, string newMessage = null)
    {
        if (active)
        {
            messageDurable.transform.Find("Text").GetComponent<Text>().text = newMessage;
            messageDurable.SetActive(true);
        }
        else messageDurable.SetActive(false);
    }

    async void Awake()
    {
        // ------- на случай, если всё сломалось, раскомментировать и запустить
        //Saving.SaveSerial.DeleteAccountSettings();

        playerInfo = new PlayerInfo(); //начальная инициализация игрока
        Saving.AccountSettingsData accountData = Saving.SaveSerial.LoadAccountSettings();
        if (accountData != null)
        {
            var response = await UserService.refresh(accountData.jwt);
            if (!response.isError)
            {                
                Saving.SaveSerial.SaveAccountSettings(playerInfo.responseUserData = response.data);
                playerInfo.isAuthorized = true;                
            }               
        }
        else Debug.Log("Account data doesn't exist");     

        menuStart.SetActive(true); 

        boardsInfo = new TaskBoardInformation[3];
        
        byte[] answ = new byte[10] { 1, 0, 0, 2, 1, 1, 0, 1, 2, 0 };
        boardsInfo[0] = new TaskBoardInformation(0,"Mechanics", answ);
        answ = new byte[10] { 2, 1, 0, 0, 2, 0, 1, 0, 1, 0 };
        boardsInfo[1] = new TaskBoardInformation(1, "Electricity", answ);

        //answ = new byte[10] { 0, 0, 1, 2, 0, 0, 0, 0, 1, 2 };
        answ = new byte[1] { 0 };
        boardsInfo[2] = new TaskBoardInformation(2, "Molecular", answ);
        RefreshQuestionsCount();

        //answ = new byte[10] { 1, 0, 0, 2, 1, 1, 0, 1, 2, 0 };
        //boardInfo1 = new TaskBoardInformation(0, "Mechanics", answ);

        /*
        textUI_startMessage.SetActive(true);
        textUI_question.SetActive(false);
        textUI_endMessage.SetActive(false);
        textUI_pressF.SetActive(false);
        textUI_lockedDoor.SetActive(false);
        key.transform.position = null_position;
        keyIcon.SetActive(false);
        crownIcon.SetActive(false);   
        */
    }

    void Update()
    {
        /*
        if (startMessageIsShowing && Input.GetKeyDown(KeyCode.F))
        {
            startMessageIsShowing = false;
            textUI_startMessage.SetActive(false);
            textUI_question.SetActive(true);
        }
        
        if (playerInfo.ActiveKey && playerInfo.KeysCount == 3)
        {
            
            if (playerInfo.points >= rooms*70)
            {
                crownIcon.transform.localPosition = new Vector3(0, 245, 0);
                crownIcon.SetActive(true);
            }
            
            
            textUI_question.SetActive(false);
            endMessageIsShowing = true;
            playerInfo.PutAwayKey(0);
            //keyIcon.SetActive(false);
            textUI_endMessage.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = 
                "Молодец! Ты ответил на много вопросов, в результате набрал " + playerInfo.NumRightAnswersTotal*10 + 
                " очков и приобрёл звание " + ranks[playerInfo.cur_rank] +
                ".\nТы можешь походить по классам, ответить на оставшиеся вопросы, почитать про выдающихся учёных физиков или выйти из игры.\nДо скорых встреч!" +
                "\n\nНажми F, чтобы закрыть окно.";
            textUI_endMessage.SetActive(true);            
        }       

        if (endMessageIsShowing && Input.GetKeyDown(KeyCode.F))
        {
            crownIcon.transform.localPosition = new Vector3(0, 60, 0);
            endMessageIsShowing = false;
            textUI_endMessage.SetActive(false);
            textUI_question.SetActive(true);            
        } */       
    }
}
