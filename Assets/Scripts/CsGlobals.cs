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
    //------------------------------------------------- НУЖНО ДЛЯ МЕНЮ
    public ResponseUserData responseUserData;
    public bool isAuthorized;

    //------------------------------------------------- НУЖНО ДЛЯ ИГРОВОЙ СЦЕНЫ
    public bool[] roomsOpen; // флаги открытых комнат 
    public string[] roomsTitle;
    public int[] roomsGroupID; //id группы, закрепленной за комнатой

    public PlayerInfo()
    {
        isAuthorized = false;
        roomsOpen = new bool[8];
        roomsTitle = new string[8];
        roomsGroupID = new int[8];
    }
}

public class CsGlobals : MonoBehaviour
{    
    public GameObject player; // игровой объект - игрок
    public PlayerInfo playerInfo;
    public GameObject messageDurable; //постоянно висящая табличка (вверху)
    public GameObject messageTemporary; //временно появляющаяся табличка (внизу)
    public GameObject menuStart;

    
    public bool RELOAD = false;
    public byte RELOADcount = 0;
 

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

        //DataHolder.PlayerInfo = new PlayerInfo();

        playerInfo = new PlayerInfo(); //начальная инициализация игрока
        Saving.AccountSettingsData accountData = Saving.SaveSerial.LoadAccountSettings();
        if (accountData != null)
        {
            try
            {
                var response = await UserService.refresh(accountData.jwt);
                if (!response.isError)
                {
                    Saving.SaveSerial.SaveAccountSettings(playerInfo.responseUserData = response.data);                    
                    playerInfo.isAuthorized = true;
                    DataHolder.PlayerInfo = playerInfo;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        else Debug.Log("Account data doesn't exist");     

        menuStart.SetActive(true); 
    }
}
