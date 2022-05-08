using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MenuTeacherTestsEditor : MonoBehaviour
{
    private CsGlobals gl;
    string jwt;
    private List<Group> listGroups;
    private List<Test> listTests;

    private GameObject menuTestsEditor;
    private GameObject menuCreateTest;
    private GameObject menuChangeTest;
    private GameObject menuDeleteTest;

    private Dropdown ddGroups;
    private Dropdown ddTests;

    private GameObject buttonCreateTest;
    private GameObject buttonYesCreateTest;
    private GameObject buttonChangeStatus;
    private GameObject buttonEditTest;
    private GameObject buttonDeleteTest;
    private GameObject buttonYesDeleteTest;

    private InputField textStatus;
    private InputField inputCreateTestTitle;

    private int selectedGroup;
    public int SelectedGroup { get { return selectedGroup; } }

    private int selectedTest;
    public int SelectedTest { get { return selectedGroup; } }

    // Start is called before the first frame update
    void Awake()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
        jwt = gl.playerInfo.responseUserData.jwt;
        menuTestsEditor = this.transform.Find("TestsEditor").gameObject;
        menuCreateTest = this.transform.Find("CreateTest").gameObject;
        menuChangeTest = this.transform.Find("UI Tasks").gameObject;
        menuDeleteTest = this.transform.Find("DeleteTest").gameObject;

        ddGroups = menuTestsEditor.transform.Find("DropdownGroups").GetComponent<Dropdown>();
        ddTests = menuTestsEditor.transform.Find("DropdownTests").GetComponent<Dropdown>();

        buttonChangeStatus = menuTestsEditor.transform.Find("ButtonChangeStatus").gameObject;
        buttonEditTest = menuTestsEditor.transform.Find("ButtonEditTest").gameObject;
        buttonCreateTest = menuTestsEditor.transform.Find("ButtonCreateTest").gameObject;
        buttonDeleteTest = menuTestsEditor.transform.Find("ButtonDeleteTest").gameObject;

        buttonYesCreateTest = menuCreateTest.transform.Find("Create").gameObject;
        buttonYesDeleteTest = menuDeleteTest.transform.Find("Button_Yes").gameObject;

        inputCreateTestTitle = menuCreateTest.transform.Find("TestName").GetComponent<InputField>();

        textStatus = menuTestsEditor.transform.Find("TextStatus").GetComponent<InputField>();

        ddGroups.onValueChanged.AddListener(delegate {
            DropdownGroupsValueChanged();
            ChangeTextStatus();
        });        

        ddTests.onValueChanged.AddListener(delegate {
            DropdownTestsValueChanged();
            ChangeTextStatus();
        });        

        buttonYesCreateTest.GetComponent<Button>().onClick.AddListener(delegate
        {
            CreateTest();
        });

        buttonYesDeleteTest.GetComponent<Button>().onClick.AddListener(delegate
        {
            DeleteTest();
        });

        buttonChangeStatus.GetComponent<Button>().onClick.AddListener(delegate
        {
            ChangeStatus();
        });
    }

    void OnEnable()
    {
        selectedGroup = -1;
        selectedTest = -1;
        ChangeTextStatus();
        UpdateGroupsList();
        UpdateTestsList();
    }

    void OnDisable()
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
                    gl.ChangeMessageTemporary("Создайте группу для начала работы", 5);
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
                    gl.ChangeMessageTemporary("Создайте тест для начала работы", 5);
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
        buttonCreateTest.SetActive(false);

        listGroups = await GetGroupsList();
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
        buttonEditTest.SetActive(false);
        buttonDeleteTest.SetActive(false);
        buttonChangeStatus.SetActive(false);
        if (selectedGroup >= 0)
        {
            listTests = await GetTestsList(listGroups[selectedGroup].groupId);
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

    async void CreateTest()
    {
        string title = inputCreateTestTitle.text;

        if (title == "")
            gl.ChangeMessageTemporary("Введите название теста", 5);
        else
        {
            var response = await TestService.create(jwt, listGroups[selectedGroup].groupId, title, true);

            if (response.isError)
                gl.ChangeMessageTemporary(response.message.ToString(), 5);
            else
            {
                menuCreateTest.SetActive(false);
                UpdateTestsList();
                gl.ChangeMessageTemporary("Тест успешно создан, проверьте список", 5);
            }

        }                   
    }

    async void ChangeStatus()
    {
        Response<Test> response;
        if (listTests[selectedTest].isOpened)
            response = await TestService.closeTest(jwt, listTests[selectedTest].testId);
        else
            response = await TestService.openTest(jwt, listTests[selectedTest].testId);

        if (response.isError)
            gl.ChangeMessageTemporary(response.message.ToString(), 5);
        else
        {
            listTests[selectedTest] = response.data;
            ChangeTextStatus();
        }
    }

    void ChangeTextStatus()
    {
        if (selectedTest >= 0)
        {
            if (listTests[selectedTest].isOpened)
                textStatus.text = "Открыт";
            else
                textStatus.text = "Закрыт";
        }
        else
            textStatus.text = "Статус";
    }

    async void DeleteTest()
    {
        var response = await TestService.delete(jwt, listTests[selectedTest].testId);

        if (response.isError)
            gl.ChangeMessageTemporary(response.message.ToString(), 5);
        else
        {
            menuDeleteTest.SetActive(false);
            UpdateTestsList();
            ChangeTextStatus();
            gl.ChangeMessageTemporary("Тест успешно удалён", 5);
        }
    }

    private void DropdownGroupsValueChanged()
    {
        selectedGroup = ddGroups.value - 1;
        if (ddGroups.value > 0)
            buttonCreateTest.SetActive(true);
        else
            buttonCreateTest.SetActive(false);
        UpdateTestsList();
    }

    private void DropdownTestsValueChanged()
    {
        selectedTest = ddTests.value - 1;
        if (ddTests.value > 0)
        {
            buttonEditTest.SetActive(true);
            buttonDeleteTest.SetActive(true);
            buttonChangeStatus.SetActive(true);            
        }
        else
        {
            buttonEditTest.SetActive(false);
            buttonDeleteTest.SetActive(false);
            buttonChangeStatus.SetActive(false);
        }
    }

    public Group GetSelectedGroup()
    {
        return listGroups[selectedGroup];
    }

    public Test GetSelectedTest()
    {
        return listTests[selectedTest];
    }
}
