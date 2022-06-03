using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerInfo
{   
    public ResponseUserData responseUserData; // --- информаци€ о пользователе из Ѕƒ
    public bool isAuthorized; // --- вошЄл ли пользователь в аккаунт
}

public class CsGlobals : MonoBehaviour
{   
    public PlayerInfo playerInfo;
    public GameObject messageDurable; // посто€нно вис€ща€ табличка (вверху)
    public GameObject messageTemporary; // временно по€вл€юща€с€ табличка (внизу)
    public GameObject menuStart; // объект - главное меню
    
    private IEnumerator ChangeMessageTemporaryIE(string newMessage, float time)
    {
        //показываем табличку с сообщением
        messageTemporary.transform.Find("Text").GetComponent<Text>().text = newMessage;
        messageTemporary.SetActive(true);
        //ждЄм заданное врем€
        yield return new WaitForSeconds(time);
        //убираем табличку
        messageTemporary.SetActive(false);
    }

    ///<summary>
    /// «апуск сопрограммы: сообщение будет отображатьс€ заданное число секунд 
    ///</summary>
    public void ChangeMessageTemporary(string newMessage, float time)
    {
        StartCoroutine(ChangeMessageTemporaryIE(newMessage, time));
    }

    ///<summary>
    /// ≈сли active = false, убирает сообщение, иначе мен€ет сообщение на указанную строку
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
        // ------- на случай, если всЄ сломалось, раскомментировать и запустить
        //Saving.SaveSerial.DeleteAccountSettings();

        playerInfo = new PlayerInfo(); //начальна€ инициализаци€ игрока
        Saving.AccountSettingsData accountData = Saving.SaveSerial.LoadAccountSettings(); //пробуем загрузить данные пользовател€
        if (accountData != null)
        {
            try
            {
                //если удалось загрузить данные, обновл€ем токен пользовател€
                var response = await UserService.refresh(accountData.jwt);
                if (response != null && !response.isError)
                {
                    Saving.SaveSerial.SaveAccountSettings(playerInfo.responseUserData = response.data); //сохран€ем обновленный токен и данные о пользователе                    
                    playerInfo.isAuthorized = true; //считаем, что пользователь зашЄл в аккаунт
                    DataHolder.PlayerInfo = playerInfo; //сохран€ем данные о пользователе в статический класс (используетс€ на игровой сцене)
                }
            }
            catch (Exception e) { Debug.LogError(e); }
        }
        else Debug.Log("Account data doesn't exist");   
        menuStart.SetActive(true); // активируем главное меню 
    }
}
