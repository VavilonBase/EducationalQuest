using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TeacherGroupsInfo : MonoBehaviour
{
    public List<Group> listGroups; // список групп учителя
    public List<Test> listTests; // список тестов в выбранной группе
    public int selectedGroup; // выбранная (в выпадающем списке) группа ; -1 = не выбрана ни одна
    public int selectedTest; // выбранный (в выпадающем списке) тест ; -1 = не выбран ни один

    public async Task<List<Group>> GetGroupsList(string jwt, CsGlobals gl)
    {
        var response = await GroupService.getAllTeacherGroups(jwt);
        if (response.isError)
        {
            switch (response.message)
            {
                case Message.TeacherHasNotGroups:
                    gl.ChangeMessageTemporary("Группы отсутствуют", 5);
                    break;
                case Message.AccessDenied:
                    gl.ChangeMessageTemporary("Доступ ограничен. Дождитесь подтверждения регистрации", 5);
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

    public async Task<List<Test>> GetTestsList(int groupID, string jwt, CsGlobals gl)
    {
        var response = await TestService.getAllGroupTests(jwt, groupID);
        if (response.isError)
        {
            switch (response.message)
            {
                case Message.GroupHasNotTests:
                    gl.ChangeMessageTemporary("Тесты в группе отсутствуют", 5);
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

    public Group GetSelectedGroup() { return listGroups[selectedGroup]; }

    public Test GetSelectedTest() { return listTests[selectedTest]; }
}


public class MenuTeacherResults : TeacherGroupsInfo
{
    private CsGlobals gl;
    private string jwt;

    [Header("Components")]
    [SerializeField] private List_view_admin m_ListView;
    [SerializeField] private GameObject m_Prefab;
    [SerializeField] private List_view_admin m_ListViewDetailedRes;
    [SerializeField] private GameObject m_PrefabDetailedRes;

    private Dropdown ddGroups;
    private Dropdown ddTests;

    private Button buttonShowResults;

    private GameObject menuShowDetailedRes;

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

        menuShowDetailedRes = this.transform.Find("Detailed_res").gameObject;
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

    

    private async void UpdateGroupsList()
    {
        ddGroups.ClearOptions();
        listGroups = await GetGroupsList(jwt, gl);
        if (listGroups != null)
        {
            //Вывод названий групп в Dropdown. Это визуализация, в дальнейшем выбранная группа определяется по индексу в списке - 1.
            List<string> m_DropOptions = new List<string>();
            m_DropOptions.Add("Выберите группу...");
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
            listTests = await GetTestsList(listGroups[selectedGroup].groupId, jwt, gl);
            if (listTests != null)
            {
                //Вывод названий групп в Dropdown. Это визуализация, в дальнейшем выбранный тест определяется по индексу в списке - 1.
                List<string> m_DropOptions = new List<string>();
                m_DropOptions.Add("Выберите тест...");
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
                    gl.ChangeMessageTemporary("Результаты отсутствуют", 5);
                    break;
                default:
                    gl.ChangeMessageTemporary(response.message.ToString(), 5);
                    break;
            }            
        }
        else
        {
            
            List<ResponseStudentTestResult> listResults = response.data;            
            var max = await TestService.getMaxScoresForTestByTestId(jwt, listTests[selectedTest].testId);
            int count = 0;
            for (int i=0; i < listResults.Count; i++)
            {                
                for (int j = 0; j < listResults[i].results.Count; j++)
                {
                    GameObject element = m_ListView.Add(m_Prefab);
                    List_element_admin elementMeta = element.GetComponent<List_element_admin>();
                    elementMeta.SetTitle((count++ + 1) + ". " + listResults[i].lastName + " " + listResults[i].firstName + " " + listResults[i].middleName);
                    elementMeta.SetDescription("Баллы: " + listResults[i].results[j].totalScores + " из "+ max?.data);                    
                    //int resID = listResults[i].results[j].resultId;
                    //int studentID = listResults[i].id;
                    //elementMeta.GetActionButton().onClick.AddListener(delegate { ShowDetailedRes(resID, studentID); });
                    elementMeta.GetActionButton().gameObject.SetActive(false);
                }                    
            }
        }            
    }

    private async void ShowDetailedRes(int resID, int studentID)
    {
        m_ListViewDetailedRes.CleanList();
        //var response = await TestService



        /*
        var response = await TestService.getStudentTestResultWithRightAnswer(jwt, 1, 1);
        response.data.resultsData[1].results[1].
        */

    }
}
