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
    public ResponseUserData responseUserData; // --- информация о пользователе из БД
    public bool isAuthorized; // --- вошёл ли пользователь в аккаунт
}

public class CsGlobals : MonoBehaviour
{   
    public PlayerInfo playerInfo;
    public GameObject messageDurable; // постоянно висящая табличка (вверху)
    public GameObject messageTemporary; // временно появляющаяся табличка (внизу)
    public GameObject menuStart; // объект - главное меню
    
    private IEnumerator ChangeMessageTemporaryIE(string newMessage, float time)
    {
        //показываем табличку с сообщением
        messageTemporary.transform.Find("Text").GetComponent<Text>().text = newMessage;
        messageTemporary.SetActive(true);

        //ждём заданное время
        yield return new WaitForSeconds(time);

        //убираем табличку
        messageTemporary.SetActive(false);
    }

    ///<summary>
    /// Запуск сопрограммы: сообщение будет отображаться заданное число секунд 
    ///</summary>
    public void ChangeMessageTemporary(string newMessage, float time)
    {
        StartCoroutine(ChangeMessageTemporaryIE(newMessage, time));
    }

    ///<summary>
    /// Если active = false, убирает сообщение, иначе меняет сообщение на указанную строку
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
        // ------- на случай, если всё сломалось, раскомментировать и запустить
        //Saving.SaveSerial.DeleteAccountSettings();

        playerInfo = new PlayerInfo(); //начальная инициализация игрока
        Saving.AccountSettingsData accountData = Saving.SaveSerial.LoadAccountSettings(); //пробуем загрузить данные пользователя
        if (accountData != null)
        {
            try
            {
                //если удалось загрузить данные, обновляем токен пользователя
                var response = await UserService.refresh(accountData.jwt);
                if (response != null && !response.isError)
                {
                    Saving.SaveSerial.SaveAccountSettings(playerInfo.responseUserData = response.data); //сохраняем обновленный токен и данные о пользователе
                    Debug.Log("Имя игрока: " + playerInfo.responseUserData.user.firstName);
                    Debug.Log("id игрока: " + playerInfo.responseUserData.user.userId);
                    playerInfo.isAuthorized = true; //считаем, что пользователь зашёл в аккаунт
                    DataHolder.PlayerInfo = playerInfo; //сохраняем данные о пользователе в статический класс (используется на игровой сцене)
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        else Debug.Log("Account data doesn't exist");   
        menuStart.SetActive(true); // активируем главное меню 
    }
}
