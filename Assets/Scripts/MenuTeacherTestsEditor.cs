using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MenuTeacherTestsEditor : TeacherGroupsInfo
{
    private CsGlobals gl;
    string jwt;

    private GameObject menuTestsEditor;
    private GameObject menuCreateTest;
    private GameObject menuChangeTest;
    private GameObject menuDeleteTest;

    private Dropdown ddGroups;
    private Dropdown ddTests;

    private GameObject buttonCreateTest;
    private GameObject buttonYesCreateTest;
    private GameObject buttonChangeStatus;
    private GameObject buttonChangeCanViewRes;
    private GameObject buttonEditTest;
    private GameObject buttonDeleteTest;
    private GameObject buttonYesDeleteTest;

    private GameObject textStatus;
    private GameObject textCanViewRes;
    private InputField inputCreateTestTitle;


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
        buttonChangeCanViewRes = menuTestsEditor.transform.Find("ButtonChangeCanViewRes").gameObject;
        buttonEditTest = menuTestsEditor.transform.Find("ButtonEditTest").gameObject;
        buttonCreateTest = menuTestsEditor.transform.Find("ButtonCreateTest").gameObject;
        buttonDeleteTest = menuTestsEditor.transform.Find("ButtonDeleteTest").gameObject;

        buttonYesCreateTest = menuCreateTest.transform.Find("Create").gameObject;
        buttonYesDeleteTest = menuDeleteTest.transform.Find("Button_Yes").gameObject;

        inputCreateTestTitle = menuCreateTest.transform.Find("TestName").GetComponent<InputField>();

        textStatus = menuTestsEditor.transform.Find("TextStatus").gameObject;
        textCanViewRes = menuTestsEditor.transform.Find("TextCanViewRes").gameObject;

        ddGroups.onValueChanged.AddListener(delegate {
            DropdownGroupsValueChanged();            
        });        

        ddTests.onValueChanged.AddListener(delegate {            
            DropdownTestsValueChanged();            
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

        buttonChangeCanViewRes.GetComponent<Button>().onClick.AddListener(delegate
        {
            ChangeCanViewRes();
        });
    }    

    void OnEnable()
    {
        selectedGroup = -1;
        selectedTest = -1;
        //ChangeTextStatus();
        UpdateGroupsList();
        UpdateTestsList();
    }

    void OnDisable()
    {
        listGroups = null;
        listTests = null;
    }

    private async void UpdateGroupsList()
    {
        ddGroups.ClearOptions();
        buttonCreateTest.SetActive(false);

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
        DropdownTestsValueChanged();
    }

    async void CreateTest()
    {
        string title = inputCreateTestTitle.text;

        if (title == "")
            gl.ChangeMessageTemporary("Введите название теста", 5);
        else
        {
            var response = await TestService.create(jwt, listGroups[selectedGroup].groupId, title, false);

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
        var check = await TestService.getTestWithQuestion(jwt, listTests[selectedTest].testId);
        if (check.isError)
        {
            switch (check.message)
            {
                case Message.TestHasNotQuestions:
                    gl.ChangeMessageTemporary("В тесте нет вопросов. Создайте вопросы, прежде чем открывать тест", 5);
                    break;
                default:
                    gl.ChangeMessageTemporary(check.message.ToString(), 5);
                    break;
            }
        }            
        else
        {
            if (check.data.questions != null)
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
                    DropdownTestsValueChanged();
                }
            }
        }        
    }

    async void ChangeCanViewRes()
    {
        var response = await TestService.update(jwt, listTests[selectedTest].testId, listTests[selectedTest].title, !listTests[selectedTest].canViewResults);
        if (response.isError)
            gl.ChangeMessageTemporary(response.message.ToString(), 5);
        else
        {
            listTests[selectedTest] = response.data;            
            DropdownTestsValueChanged();
        }
    }

    void ChangeTextStatus()
    {
        Debug.Log("SelectedTest"+selectedTest);
        if (selectedTest >= 0)
        {
            textStatus.GetComponent<InputField>().text = listTests[selectedTest].isOpened ? "Открыт" : "Закрыт";
            textCanViewRes.GetComponent<InputField>().text = listTests[selectedTest].canViewResults ? "Ответы ВИДНО" : "Ответы НЕ ВИДНО";
        }
        else
        {
            textStatus.GetComponent<InputField>().text = "Статус";
            textCanViewRes.GetComponent<InputField>().text = "---";
        }            
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
            gl.ChangeMessageTemporary("Тест успешно удалён", 5);
        }
    }

    private void DropdownGroupsValueChanged()
    {
        selectedGroup = ddGroups.value - 1;
        buttonCreateTest.SetActive(true);
        if (ddGroups.value > 0)
            buttonCreateTest.GetComponent<Button>().interactable = true;
        else
            buttonCreateTest.GetComponent<Button>().interactable = false;
        UpdateTestsList();
    }

    private void DropdownTestsValueChanged()
    {       
        selectedTest = ddTests.value - 1;
        if (ddTests.value > 0)
        {
            buttonEditTest.SetActive(true);
            buttonDeleteTest.SetActive(true);
            if (listTests[selectedTest].isOpened)
            {
                buttonEditTest.GetComponent<Button>().interactable = false;
                buttonDeleteTest.GetComponent<Button>().interactable = false;
            }
            else
            {
                buttonEditTest.GetComponent<Button>().interactable = true;
                buttonDeleteTest.GetComponent<Button>().interactable = true;
            }              
            
            buttonChangeStatus.SetActive(true);
            buttonChangeCanViewRes.SetActive(true);
            textStatus.SetActive(true);
            textCanViewRes.SetActive(true);
        }
        else
        {
            buttonEditTest.SetActive(false);
            buttonDeleteTest.SetActive(false);
            buttonChangeStatus.SetActive(false);
            buttonChangeCanViewRes.SetActive(false);
            textStatus.SetActive(false);
            textCanViewRes.SetActive(false);
        }
        ChangeTextStatus();
    }
}
