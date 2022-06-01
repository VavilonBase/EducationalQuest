using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerInfo
{   
    public ResponseUserData responseUserData; // --- ���������� � ������������ �� ��
    public bool isAuthorized; // --- ����� �� ������������ � �������
    public bool needToHideInfo; // --- ����� �� �������� ���� �� �������� (���� ������������ ������ �� ������ ���)
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
        Saving.AccountSettingsData accountData = Saving.LoadAccountSettings(); //������� ��������� ������ ������������
        if (accountData != null)
        {
            try
            {
                //���������� ����������� � ���������� �����
                playerInfo.needToHideInfo = accountData.hideInfo;
                Debug.Log("Need to hide info? " + playerInfo.needToHideInfo);

                //���� ������� ��������� ������, ��������� ����� ������������
                var response = await UserService.refresh(accountData.jwt);
                if (response != null && !response.isError)
                {
                    playerInfo.responseUserData = response.data;
                    Saving.SaveAccountSettings(playerInfo); //��������� ����������� ����� � ������ � ������������                    
                    playerInfo.isAuthorized = true; //�������, ��� ������������ ����� � �������                    
                }

                DataHolder.PlayerInfo = playerInfo; //��������� ������ � ������������ � ����������� ����� (������������ �� ������� �����)
            }
            catch (Exception e) { Debug.LogError(e); }
        }
        else Debug.Log("Account data doesn't exist");   
        menuStart.SetActive(true); // ���������� ������� ���� 
    }
}
