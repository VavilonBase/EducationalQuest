using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTeacherCreateGroupInteractions : MonoBehaviour
{
    public GameObject menuTeacherGroups;
    private MenuTeacherGroupsListInteractions menuData;

    private void Awake()
    {
        menuData = menuTeacherGroups.GetComponent<MenuTeacherGroupsListInteractions>();
    }
    public async void CreateGroup()
    {
        CsGlobals gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
        string groupName = this.transform.Find("Name_group").GetComponent<InputField>().text;

        if (menuData.CheckIfTitleExists(groupName))
            gl.ChangeMessageTemporary("Группа с таким названием уже существует", 5);
        else
        {
            var response = await GroupService.createGroup(groupName, gl.playerInfo.responseUserData.jwt);
            if (response.isError)
                switch (response.message)
                {
                    case Message.CanNotCreateGroup:
                        gl.ChangeMessageTemporary("Не удалось создать группу. Проверьте правильность заполнения полей", 5);
                        break;
                    case Message.NotFoundRequiredData:
                        gl.ChangeMessageTemporary("Проверьте правильность заполнения полей", 5);
                        break;
                    case Message.AccessDenied:
                        gl.ChangeMessageTemporary("Доступ ограничен. Дождитесь подтверждения регистрации", 5);
                        break;
                    case Message.IncorrectTokenFormat:
                        Debug.LogError("Incorrect token format");
                        gl.ChangeMessageTemporary("Ошибка подключения. Попробуйте перезайти в аккаунт", 5);
                        break;
                    case Message.DBErrorExecute:
                        gl.ChangeMessageTemporary("Ошибка при выполнении запроса в базе данных", 5);
                        break;
                    default:
                        gl.ChangeMessageTemporary("Default Error", 5);
                        break;
                }
            else
            {
                gl.ChangeMessageTemporary("Группа успешна создана, проверьте список", 5);
                this.transform.gameObject.SetActive(false);
                menuTeacherGroups.SetActive(true);
            }
        }        
    }
}
