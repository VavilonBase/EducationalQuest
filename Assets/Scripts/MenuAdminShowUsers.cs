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
            Debug.Log(response.message.ToString());
            return null;
        }
        else
            return response.data;
    }

    async void PrepareUserList(UserService.RolesEnum role, List<User> list, List_view_admin m_ListView, GameObject m_Prefab)
    {
        m_ListView.CleanList();
        list = await GetUsersList(role);
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                GameObject element = m_ListView.Add(m_Prefab);

                List_element_admin elementMeta = element.GetComponent<List_element_admin>();
                elementMeta.SetTitle(i + 1 + ". (" + list[i].login + ") " + list[i].lastName + " " + list[i].firstName + " " + list[i].middleName); // ������ this.m_Tittle ���� �������� ��� �������

                if (role == UserService.RolesEnum.Teacher)
                {
                    Button actionButton = elementMeta.GetActionButton();
                    if (!list[i].isActivated)
                    {
                        actionButton.onClick.AddListener(elementMeta.ActivateTeacher);

                        string id_button = list[i].id.ToString(); // ���� ������� �������� ����� �� ��� � � id �������, ����� �� ������� ������������� ������ �������                
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
        PrepareUserList(UserService.RolesEnum.Teacher, listTeachers, m_ListViewTeacher, m_PrefabTeacher);
    }
    public void UpdateStudentsList()
    {               
        PrepareUserList(UserService.RolesEnum.Student, listStudents, m_ListViewStudent, m_PrefabStudent);
    }
}
