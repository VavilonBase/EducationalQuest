using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MenuTeacherResults : MonoBehaviour
{
    private CsGlobals gl;
    private string jwt;

    [Header("Components")]
    [SerializeField] private List_view_admin m_ListView;
    [SerializeField] private GameObject m_Prefab;

    private Dropdown ddGroups;
    private Dropdown ddTests;

    private Button buttonShowResults;

    private List<Group> listGroups;
    private List<Test> listTests;
    private int selectedGroup;
    private int selectedTest;

    private void Awake()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
        jwt = gl.playerInfo.responseUserData.jwt;

        ddGroups = this.transform.Find("DropdownGroups").GetComponent<Dropdown>();
        ddTests = this.transform.Find("DropdownTests").GetComponent<Dropdown>();
        buttonShowResults = this.transform.Find("But_look").GetComponent<Button>();

        buttonShowResults.onClick.AddListener(delegate { ShowResults(); });

        ddGroups.onValueChanged.AddListener(delegate { DropdownGroupsValueChanged(); });        
        ddTests.onValueChanged.AddListener(delegate { DropdownTestsValueChanged(); });       
    }

    private void OnEnable()
    {
        buttonShowResults.interactable = false;
        selectedGroup = -1;
        selectedTest = -1;        
        UpdateGroupsList();
        UpdateTestsList();
    }
    private void OnDisable()
    {
        listGroups = null;
        listTests = null;
    }

    async Task<List<Group>> GetGroupsList()
    {
        var response = await GroupService.getAllTeacherGroups(jwt);
        if (response.isError)
        {
            switch (response.message)
            {
                case Message.TeacherHasNotGroups:
                    gl.ChangeMessageTemporary("������ �����������", 5);
                    break;
                default:
                    gl.ChangeMessageTemporary(response.message.ToString(), 5);
                    break;
            }
            return null;
        }
        else
            return response.data;
    }

    async Task<List<Test>> GetTestsList(int groupID)
    {
        var response = await TestService.getAllGroupTests(jwt, groupID);

        if (response.isError)
        {
            switch (response.message)
            {
                case Message.GroupHasNotTests:
                    gl.ChangeMessageTemporary("����� � ������ �����������", 5);
                    break;
                default:
                    gl.ChangeMessageTemporary(response.message.ToString(), 5);
                    break;
            }
            return null;
        }
        else
            return response.data;
    }

    private async void UpdateGroupsList()
    {
        ddGroups.ClearOptions();
        listGroups = await GetGroupsList();
        if (listGroups != null)
        {
            //����� �������� ����� � Dropdown. ��� ������������, � ���������� ��������� ������ ������������ �� ������� � ������ - 1.
            List<string> m_DropOptions = new List<string>();
            m_DropOptions.Add("�������� ������...");
            foreach (Group g in listGroups)
                m_DropOptions.Add(g.title);
            ddGroups.AddOptions(m_DropOptions);
        }
        selectedGroup = -1;
    }

    private async void UpdateTestsList()
    {
        ddTests.ClearOptions();
        if (selectedGroup >= 0)
        {
            listTests = await GetTestsList(listGroups[selectedGroup].groupId);
            if (listTests != null)
            {
                //����� �������� ����� � Dropdown. ��� ������������, � ���������� ��������� ���� ������������ �� ������� � ������ - 1.
                List<string> m_DropOptions = new List<string>();
                m_DropOptions.Add("�������� ����...");
                foreach (Test t in listTests)
                    m_DropOptions.Add(t.title);
                ddTests.AddOptions(m_DropOptions);
            }
        }
        selectedTest = -1;
    }

    private void DropdownTestsValueChanged()
    {        
        selectedTest = ddTests.value - 1;        
        if (selectedTest>=0)
            buttonShowResults.interactable = true;
        else
            buttonShowResults.interactable = false;
    }

    private void DropdownGroupsValueChanged()
    {
        selectedGroup = ddGroups.value - 1;
        UpdateTestsList();
    }

    private async void ShowResults()
    {        
        m_ListView.CleanList();
        var response = await TestService.getAllResultsTestsForStudents(jwt, listTests[selectedTest].testId);
        if (response.isError)
        {
            switch (response.message)
            {
                case Message.NotFoundResult:
                    gl.ChangeMessageTemporary("���������� �����������", 5);
                    break;
                default:
                    gl.ChangeMessageTemporary(response.message.ToString(), 5);
                    break;
            }            
        }
        else
        {
            List<ResponseStudentTestResult> listResults = response.data;
            for (int i=0; i < listResults.Count; i++)
            {
                for (int j = 0; j < listResults[i].results.Count; j++)
                {
                    GameObject element = m_ListView.Add(m_Prefab);
                    List_element_admin elementMeta = element.GetComponent<List_element_admin>();
                    elementMeta.SetTitle((i + 1) + ". " + listResults[i].lastName + " " + listResults[i].firstName + " " + listResults[i].middleName);
                    elementMeta.SetDescription("�����: " + listResults[i].results[j].totalScores);
                }                    
            }
        }            
    }
}
