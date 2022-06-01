using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class DataHolder
{
    private static PlayerInfo playerInfo;
    public static PlayerInfo PlayerInfo { get { return playerInfo; } set { playerInfo = value; } }

    private static GameObject messageTemporary;
    private static Text textMessageTemporary;
    public static GameObject MessageTemporary { set { messageTemporary = value; textMessageTemporary = messageTemporary.transform.Find("Message").GetComponent<Text>(); } }

    public static void ChangeMessageTemporary(string newText = "")
    {
        if (newText == "")
            messageTemporary.SetActive(false);
        else
        {
            textMessageTemporary.text = newText;
            messageTemporary.SetActive(true);
        }        
    }
}


public class Globals : MonoBehaviour
{
    string jwt;
    List<Class> classes;

    void RememberChoice()
    {
        DataHolder.PlayerInfo.needToHideInfo = true;
        Saving.SaveAccountSettings(DataHolder.PlayerInfo);
        Debug.Log("Больше не покажем эту табличку");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Scenes.NextScene(0);
    }

    private async void Awake()
    {
        try
        {
            //настройка глобальных данных        
            DataHolder.MessageTemporary = this.transform.Find("Canvas").Find("Message_Temporary").gameObject;
            GameObject info = this.transform.Find("Canvas").Find("Info").gameObject;
            if (DataHolder.PlayerInfo.needToHideInfo)
                info.SetActive(false);
            else
            {
                info.transform.Find("But_close").GetComponent<Button>().onClick.AddListener(RememberChoice);
                info.SetActive(true);
            }

            jwt = DataHolder.PlayerInfo.responseUserData.jwt;

            //ищем все комнаты и приклепляем к ним классы, сохраняем
            classes = new List<Class>();
            for (int i = 0; i < 8; i++)
                classes.Add(this.transform.Find("school").Find("Class" + (i + 1)).gameObject.AddComponent<Class>());                         

            //настройка сцены в зависимости от групп ученика
            var response = await GroupService.getStudentGroups(jwt);
            if (response.isError)
            {
                switch (response.message)
                {
                    case Message.StudentHasNotGroups:
                        DataHolder.ChangeMessageTemporary("Все классы закрыты. Вернись в главное меню и вступи в группу, чтобы продолжить");
                        break;
                    default:
                        DataHolder.ChangeMessageTemporary(response.message.ToString());
                        break;
                }
            }
            else
            {
                for (int i = 0; i < response.data.Count && i < 8; i++)
                {
                    //если комната открыта, загружаем полученную информацию
                    classes[i].AssignInformation(response.data[i].title, response.data[i].groupId);
                }
            }          
        }        
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}
