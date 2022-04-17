using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTeacherGroupInteractions : MonoBehaviour
{
    public GameObject menuTeacherGroupsList;
    public GameObject menuDeleteGroup;
    
    private CsGlobals gl;
    private MenuTeacherGroupsListInteractions menuData;
    private Text titleGroup;
    private InputField inputNewTitle;

    // Start is called before the first frame update
    void Awake()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
        menuData = menuTeacherGroupsList.GetComponent<MenuTeacherGroupsListInteractions>();
        titleGroup = this.transform.Find("Text").GetComponent<Text>();
        inputNewTitle = this.transform.Find("InputField").GetComponent<InputField>();
    }

    void OnEnable()
    {
        titleGroup.text = menuData.listGroups[menuData.selectedGroup].title;
    }

    /// IncorrectTokenFormat
    /// AccessDenied
    /// CanNotUpdateGroup
    /// GroupNotFound
    /// UserIsNotCreatorGroup
    /// DBErrorExecute
    public async void ChangeGroupTitle()
    {
        string newGroupTitle = inputNewTitle.text;
        if (menuData.CheckIfTitleExists(newGroupTitle))
            gl.ChangeMessageTemporary("Группа с таким названием уже существует", 5);
        else
        {
            var response = await GroupService.updateGroup(gl.playerInfo.responseUserData.jwt, newGroupTitle, menuData.listGroups[menuData.selectedGroup].groupId);
            if (response.isError)
                switch (response.message)
                {
                    case Message.CanNotUpdateGroup:
                        gl.ChangeMessageTemporary("Не удалось обновить название. Проверьте правильность заполнения полей", 5);
                        break;
                    default:
                        gl.ChangeMessageTemporary(response.message.ToString(), 5);
                        break;
                }
            else
            {
                gl.ChangeMessageTemporary("Название группы успешно обновлено", 5);
                titleGroup.text = response.data.title;
            }
        }
    }

    public async void DeleteGroup()
    {
        var response = await GroupService.deleteGroup(gl.playerInfo.responseUserData.jwt, menuData.listGroups[menuData.selectedGroup].groupId);
        if (response.isError)
            switch (response.message)
            {
                case Message.CanNotDeleteGroup:
                    gl.ChangeMessageTemporary("Не удалось удалить группу", 5);
                    break;
                default:
                    gl.ChangeMessageTemporary(response.message.ToString(), 5);
                    break;
            }
        else
        {
            gl.ChangeMessageTemporary("Группа успешно удалена", 5);
            //возвращаем меню           
            menuDeleteGroup.SetActive(false);
            menuTeacherGroupsList.SetActive(true);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
