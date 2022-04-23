using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MenuStudentGroupsInteractions : MonoBehaviour
{
    private CsGlobals gl;
    private List<ResponseStudentGroupData> listGroups;
    private List<Test> listTests;
    
    private GameObject menuTests;
    private GameObject menuResults;
    private GameObject menuExitGroup;
    private Dropdown dd;
    private GameObject buttonShowResults;
    private GameObject buttonExitGroup;
    private GameObject buttonUpdate;    
    private GameObject buttonYesExitGroup;    

    private int selectedGroup;
    public int SelectedGroup { get { return selectedGroup; } }

    [Header("Components")]
    [SerializeField] private List_view_admin m_ListViewGroupContent;
    [SerializeField] private GameObject m_PrefabGroupContent;
    [SerializeField] private List_view_admin m_ListViewGroupResults;
    [SerializeField] private GameObject m_PrefabGroupResults;

    private void Awake()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
        menuTests = this.transform.Find("menuTests").gameObject;
        menuResults = this.transform.Find("Results_group_student").gameObject;
        menuExitGroup = this.transform.Find("ExitGroup_Student").gameObject;

        dd = menuTests.transform.Find("GroupsDropdown").GetComponent<Dropdown>();
        buttonShowResults = menuTests.transform.Find("But_res").gameObject;
        buttonExitGroup = menuTests.transform.Find("But_quit").gameObject;
        buttonUpdate = menuResults.transform.Find("Update").gameObject;        
        buttonYesExitGroup = menuExitGroup.transform.Find("Button_Yes").gameObject;

        dd.onValueChanged.AddListener(delegate {
            DropdownValueChanged();
        });
        dd.value = 0;

        buttonShowResults.GetComponent<Button>().onClick.AddListener(delegate
        {
            ShowGroupResults();
        });

        buttonUpdate.GetComponent<Button>().onClick.AddListener(delegate
        {
            UpdateGroupResults();
        });

        buttonYesExitGroup.GetComponent<Button>().onClick.AddListener(delegate
        {
            ExitGroup();
        });
    }

    void OnEnable()
    {        
        UpdateGroupsList();
    }

    async Task<List<ResponseStudentGroupData>> GetGroupsList()
    {
        var response = await GroupService.getStudentGroups(gl.playerInfo.responseUserData.jwt);

        if (response.isError)
        {
            switch (response.message)
            {
                case Message.StudentHasNotGroups:
                    gl.ChangeMessageTemporary("������ ����� ����. ����� ��������������� ��� � ������� ����", 5);
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

    async void UpdateGroupsList()
    {
        dd.ClearOptions();
        buttonShowResults.SetActive(false);
        buttonExitGroup.SetActive(false);

        listGroups = await GetGroupsList();
        if (listGroups != null)
        {
            //����� �������� ����� � Dropdown. ��� ������������, � ���������� ��������� ������ ������������ �� ������� � ������ - 1.
            List<string> m_DropOptions = new List<string>();
            m_DropOptions.Add("������ ������...");
            foreach (ResponseStudentGroupData g in listGroups)
                m_DropOptions.Add(g.title);
            dd.AddOptions(m_DropOptions);
        }
    }

    void DropdownValueChanged()
    {
        selectedGroup = dd.value - 1;
        if (dd.value > 0)
        {            
            buttonShowResults.SetActive(true);
            buttonExitGroup.SetActive(true);
            ShowGroupTests();
        }
        else
        {
            buttonShowResults.SetActive(false);
            buttonExitGroup.SetActive(false);
        }
    }

    async void ShowGroupTests()
    {
        m_ListViewGroupContent.CleanList();

        var response = await TestService.getAllGroupTest(gl.playerInfo.responseUserData.jwt, listGroups[selectedGroup].groupId);

        if (response.isError)
        {
            switch (response.message)
            {
                case Message.GroupHasNotTests:
                    gl.ChangeMessageTemporary("� ������ ��� ������", 5);
                    break;
                default:
                    gl.ChangeMessageTemporary(response.message.ToString(), 5);
                    break;
            }
        }
        else
        {
            listTests = response.data;
            int count = 0;

            for (int i = 0; i < listTests.Count; i++)
            {
                GameObject element = m_ListViewGroupContent.Add(m_PrefabGroupContent);
                List_element_admin elementMeta = element.GetComponent<List_element_admin>();

                string status;
                if (listTests[i].isClosed)
                    status = " | ������: ������";
                else
                {
                    status = " | ������: ������� (!)";
                    count++;
                }                   

                elementMeta.SetTitle(i + 1 + ". " + listTests[i].title + status);
            }

            if (count > 0)
                gl.ChangeMessageTemporary("�������� ������ � ������:" + count + " . ������ �� � ������ � ������� �����", 15);
        }     
    }

    async void ShowGroupResults()
    {
        Debug.Log("It's working");
    }

    async void UpdateGroupResults()
    {

    }

    async void ExitGroup()
    {
        gl.ChangeMessageTemporary("��������, ����� �� ������", 5);
        menuExitGroup.SetActive(false);
        menuResults.SetActive(false);
        menuTests.SetActive(true);
    }
}
