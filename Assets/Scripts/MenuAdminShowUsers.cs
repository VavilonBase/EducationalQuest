using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MenuAdminShowUsers : MonoBehaviour
{
    private CsGlobals gl;
    private List<User> listTeachers;
    private List<User> listStudents;

    [Header("Components")]    
    [SerializeField] private List_view_admin m_ListViewTeacher;
    [SerializeField] private List_view_admin m_ListViewStudent;
    [SerializeField] private GameObject m_PrefabTeacher;
    [SerializeField] private GameObject m_PrefabStudent;

    private void Awake()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
    }

    void OnEnable()
    {
        UpdateTeachersList();
        UpdateStudentsList();
    }

    async Task<List<User>> GetUsersList(UserService.RolesEnum role)
    {       
        var response = await UserService.getAllUsers(gl.playerInfo.responseUserData.jwt, role);
        if (response.isError)
        {
            Debug.LogError(response.message.ToString());
            return null;
        }
        else
            return response.data;
    }

    async void PrepareUserList(UserService.RolesEnum role, List<User> list, List_view_admin m_ListView, GameObject m_Prefab)
    {       
        list = await GetUsersList(role);
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                GameObject element = m_ListView.Add(m_Prefab);

                List_element_admin elementMeta = element.GetComponent<List_element_admin>();
                elementMeta.SetTitle(i + 1 + ". (" + list[i].login + ") " + list[i].lastName + " " + list[i].firstName + " " + list[i].middleName); // вместо this.m_Tittle надо выводить ФИО учителя

                if (role == UserService.RolesEnum.Teacher)
                {
                    Button actionButton = elementMeta.GetActionButton();
                    if (!list[i].isActivated)
                    {
                        actionButton.onClick.AddListener(elementMeta.ActivateTeacher);

                        string id_button = list[i].userId.ToString(); // надо придать значение такое же как и у id учителя, чтобы по нажатию активировался нужный учитель                
                        elementMeta.SetSomeId(id_button);
                    }
                    else
                        actionButton.gameObject.SetActive(false);
                }
            }

        }        
    }

    public void UpdateTeachersList()
    {
        m_ListViewTeacher.CleanList();
        //CleanList(m_ListViewTeacher);
        PrepareUserList(UserService.RolesEnum.Teacher, listTeachers, m_ListViewTeacher, m_PrefabTeacher);
    }
    public void UpdateStudentsList()
    {
        m_ListViewStudent.CleanList();
        //CleanList(m_ListViewStudent);
        PrepareUserList(UserService.RolesEnum.Student, listStudents, m_ListViewStudent, m_PrefabStudent);
    }

    /*
    void CleanList(List_view_admin m_ListView)
    {       
        var elements = m_ListView.Elements;
        foreach (GameObject element in elements)
            DestroyImmediate(element);
        m_ListView.Elements = new List<GameObject>();
    }
    */
}
