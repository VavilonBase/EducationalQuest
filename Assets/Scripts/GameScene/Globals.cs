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
    
    private async void Awake()
    {
        try
        {
            //��������� ���������� ������        
            DataHolder.MessageTemporary = this.transform.Find("Canvas").Find("Message_Temporary").gameObject;
            jwt = DataHolder.PlayerInfo.responseUserData.jwt;

            //���� ��� ������� � ����������� � ��� ������, ���������
            classes = new List<Class>();
            for (int i = 0; i < 8; i++)
                classes.Add(this.transform.Find("school").Find("Class" + (i + 1)).gameObject.AddComponent<Class>());                         

            //��������� ����� � ����������� �� ����� �������
            var response = await GroupService.getStudentGroups(jwt);
            if (response.isError)
            {
                switch (response.message)
                {
                    case Message.StudentHasNotGroups:
                        DataHolder.ChangeMessageTemporary("��� ������ �������. ������� � ������� ���� � ������ � ������, ����� ����������");
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
                    //���� ������� �������, ��������� ���������� ����������
                    classes[i].AssignInformation(response.data[i].title, response.data[i].groupId);
                }
            }
            DataHolder.ChangeMessageTemporary("������ ��������");

        }        
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
