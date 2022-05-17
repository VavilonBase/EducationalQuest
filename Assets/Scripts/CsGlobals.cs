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
    public ResponseUserData responseUserData; // --- ���������� � ������������ �� ��
    public bool isAuthorized; // --- ����� �� ������������ � �������
}

public class CsGlobals : MonoBehaviour
{   
    public PlayerInfo playerInfo;
    public GameObject messageDurable; // ��������� ������� �������� (������)
    public GameObject messageTemporary; // �������� ������������ �������� (�����)
    public GameObject menuStart; // ������ - ������� ����
    
    private IEnumerator ChangeMessageTemporaryIE(string newMessage, float time)
    {
        //���������� �������� � ����������
        messageTemporary.transform.Find("Text").GetComponent<Text>().text = newMessage;
        messageTemporary.SetActive(true);

        //��� �������� �����
        yield return new WaitForSeconds(time);

        //������� ��������
        messageTemporary.SetActive(false);
    }

    ///<summary>
    /// ������ �����������: ��������� ����� ������������ �������� ����� ������ 
    ///</summary>
    public void ChangeMessageTemporary(string newMessage, float time)
    {
        StartCoroutine(ChangeMessageTemporaryIE(newMessage, time));
    }

    ///<summary>
    /// ���� active = false, ������� ���������, ����� ������ ��������� �� ��������� ������
    ///</summary>
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
        // ------- �� ������, ���� �� ���������, ����������������� � ���������
        //Saving.SaveSerial.DeleteAccountSettings();

        playerInfo = new PlayerInfo(); //��������� ������������� ������
        Saving.AccountSettingsData accountData = Saving.SaveSerial.LoadAccountSettings(); //������� ��������� ������ ������������
        if (accountData != null)
        {
            try
            {
                //���� ������� ��������� ������, ��������� ����� ������������
                var response = await UserService.refresh(accountData.jwt);
                if (response != null && !response.isError)
                {
                    Saving.SaveSerial.SaveAccountSettings(playerInfo.responseUserData = response.data); //��������� ����������� ����� � ������ � ������������
                    Debug.Log("��� ������: " + playerInfo.responseUserData.user.firstName);
                    Debug.Log("id ������: " + playerInfo.responseUserData.user.userId);
                    playerInfo.isAuthorized = true; //�������, ��� ������������ ����� � �������
                    DataHolder.PlayerInfo = playerInfo; //��������� ������ � ������������ � ����������� ����� (������������ �� ������� �����)
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        else Debug.Log("Account data doesn't exist");   
        menuStart.SetActive(true); // ���������� ������� ���� 
    }
}
