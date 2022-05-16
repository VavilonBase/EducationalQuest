using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Components_admin : MonoBehaviour
{
    private CsGlobals gl;
    private List<User> listTeachers;

    [Header("Components")]
    [SerializeField] private List_view_admin m_ListView;
    [SerializeField] private GameObject m_Prefab;

    private void Awake()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
    }

    async void OnEnable()
    {
        listTeachers = await GetTeachersList();

        for (int i = 0; i < listTeachers.Count; i++)
        {
            GameObject element = this.m_ListView.Add(this.m_Prefab);

            List_element_admin elementMeta = element.GetComponent<List_element_admin>();
            elementMeta.SetTitle(i + 1 + ". (" + listTeachers[i].login + ") " + listTeachers[i].lastName + " " + listTeachers[i].firstName + " " + listTeachers[i].middleName); // вместо this.m_Tittle надо выводить ФИО учителя

            Button actionButton = elementMeta.GetActionButton();
            if (!listTeachers[i].isActivated)
            {
                actionButton.onClick.AddListener(elementMeta.ActivateTeacher);

                string id_button = listTeachers[i].id.ToString(); // надо придать значение такое же как и у id учителя, чтобы по нажатию активировался нужный учитель                
                elementMeta.SetSomeId(id_button);
            }
            else
                actionButton.gameObject.SetActive(false);
        }
    }

    async Task<List<User>> GetTeachersList()
    {        
        var response = await UserService.getAllUsers(gl.playerInfo.responseUserData.jwt, UserService.RolesEnum.Teacher);
        if (response.isError)
        {
            Debug.LogError(response.message.ToString());
            return null;
        }            
        else
            return response.data;
    }
}
