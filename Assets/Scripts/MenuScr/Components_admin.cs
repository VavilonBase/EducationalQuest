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

    [Header ("Settings")] // ѕосле изменени€ надо удалить это
    [SerializeField] private string m_Tittle;
    [Space]
    [SerializeField] private int m_Count;

    private async void Awake()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
        listTeachers = await GetTeachersList();

        for (int i=0; i< listTeachers.Count; i++) // вместо this.m_Count надо вставить количество учителей
        {
            GameObject element = this.m_ListView.Add(this.m_Prefab);

            List_element_admin elementMeta = element.GetComponent<List_element_admin>();
            elementMeta.SetTitle(i+1 + ". (" + listTeachers[i].login + ") " + listTeachers[i].lastName + " " + listTeachers[i].firstName + " " + listTeachers[i].middleName); // вместо this.m_Tittle надо выводить ‘»ќ учител€

            Button actionButton = elementMeta.GetActionButton();
            actionButton.onClick.AddListener(SomeAction);

            //string id_button = ; // надо придать значение такое же как и у id учител€, чтобы по нажатию активировалс€ нужный учитель
            // elementMeta.SetSomeId(id_button);
        }
    }

    public void SomeAction()
    {
        //надо как то придумать чтобы активировалс€ тот который надо
    }

    async Task<List<User>> GetTeachersList()
    {
        //тут будет функци€ на получение учителей, пока все пользователи
        var response = await UserService.getAllUsers(gl.playerInfo.responseUserData.jwt);
        if (response.isError)
        {
            Debug.LogError(response.message.ToString());
            return null;
        }            
        else
            return response.data;
    }
}
