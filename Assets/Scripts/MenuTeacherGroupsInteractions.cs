using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MenuTeacherGroupsInteractions : MonoBehaviour
{
    private CsGlobals gl;
    private string jwt;

    private GameObject menuGroupsList;    
    private GameObject menuCreateGroup;
    private GameObject menuRenameGroup;
    private GameObject menuDeleteGroup;
    private GameObject menuGroup;

    private Dropdown dd;
    private InputField codeWord;
    private GameObject buttonShowGroup;
    private GameObject buttonCopyCodeWord;

    private Button buttonCreateGroup;
    private Button buttonRenameGroup;
    private Button buttonDeleteGroup;
    private Button buttonCreateTest;


    private List<Group> listGroups;
    private int selectedGroup;


    private Text titleGroup;
    private InputField inputNewTitle;
    private InputField inputCreateTitle;

    void Awake()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
        jwt = gl.playerInfo.responseUserData.jwt;

        menuGroupsList = this.transform.Find("Group_list_Teacher").gameObject;        
        menuCreateGroup = this.transform.Find("Create_new_group_teacher").gameObject;
        menuRenameGroup = this.transform.Find("menuRenameGroup").gameObject;
        menuDeleteGroup = this.transform.Find("Delete_group_Teacher").gameObject;
        menuGroup = this.transform.Find("Group_Teacher").gameObject;

        dd = menuGroupsList.transform.Find("Dropdown").GetComponent<Dropdown>();
        codeWord = menuGroupsList.transform.Find("CodeWord").GetComponent<InputField>();
        buttonShowGroup = menuGroupsList.transform.Find("But_look").gameObject;
        buttonCopyCodeWord = menuGroupsList.transform.Find("ButtonCopyCodeWord").gameObject;
        
        buttonCreateGroup = menuCreateGroup.transform.Find("Create").GetComponent<Button>();        
        buttonDeleteGroup = menuDeleteGroup.transform.Find("Button_Yes").GetComponent<Button>();
        buttonRenameGroup = menuRenameGroup.transform.Find("Rename").GetComponent<Button>();

        titleGroup = menuGroup.transform.Find("Text").GetComponent<Text>();
        inputNewTitle = menuRenameGroup.transform.Find("InputField").GetComponent<InputField>();
        inputCreateTitle = menuCreateGroup.transform.Find("Name_group").GetComponent<InputField>();

        dd.onValueChanged.AddListener(delegate { DropdownValueChanged(); });
        dd.value = 0;

        buttonShowGroup.transform.GetComponent<Button>().onClick.AddListener(delegate { ShowSelectedGroupInfo(); });
        buttonCopyCodeWord.transform.GetComponent<Button>().onClick.AddListener(delegate { CopyCodeWord(); });
        buttonCreateGroup.onClick.AddListener(delegate { CreateGroup(); });
        buttonDeleteGroup.onClick.AddListener(delegate { DeleteGroup(); });
        buttonRenameGroup.onClick.AddListener(delegate { RenameGroup(); });

        buttonCreateTest = menuGroup.transform.Find("CreateTest").GetComponent<Button>();
        buttonCreateTest.onClick.AddListener(delegate { CreateTest(); });
    }

    void OnEnable()
    {
        ShowGroupsList();        
    }

    async void CreateTest()
    {
        //очень тестовое создание теста
        var test = await TestService.create(jwt, listGroups[selectedGroup].groupId, "Тестовый тест", true);
        if (test.isError)
            gl.ChangeMessageTemporary(test.message.ToString(), 5);
        else
        {
            var question = await QuestionService.createQuestion(jwt, test.data.testId, true, "2x2=?", 10);
            if (question.isError)
                gl.ChangeMessageTemporary(question.message.ToString(), 5);
            else
            {
                await AnswerService.createAnswer(jwt, question.data.questionId, "3", true, false);
                await AnswerService.createAnswer(jwt, question.data.questionId, "4", true, true);
                await AnswerService.createAnswer(jwt, question.data.questionId, "8", true, false);
                gl.ChangeMessageTemporary("Наверное, всё в порядке", 5);
            }
        }
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

    async void ShowGroupsList()
    {
        dd.ClearOptions();
        buttonShowGroup.SetActive(false);
        buttonCopyCodeWord.SetActive(false);
        codeWord.text = null;

        listGroups = await GetGroupsList();
        if (listGroups != null)
        {
            //Вывод названий групп в Dropdown. Это визуализация, в дальнейшем выбранная группа определяется по индексу в списке - 1.
            List<string> m_DropOptions = new List<string>();
            m_DropOptions.Add("Выберите группу...");
            foreach (Group g in listGroups)
                m_DropOptions.Add(g.title);
            dd.AddOptions(m_DropOptions);
        }
    }

    void DropdownValueChanged()
    {
        selectedGroup = dd.value - 1;
        if (dd.value > 0)
        {
            codeWord.text = listGroups[selectedGroup].codeWord;
            buttonShowGroup.SetActive(true);
            buttonCopyCodeWord.SetActive(true);
        }
        else
        {
            codeWord.text = null;
            buttonShowGroup.SetActive(false);
            buttonCopyCodeWord.SetActive(false);
        }
    }

    public void CopyCodeWord()
    {
        GUIUtility.systemCopyBuffer = codeWord.text;
        gl.ChangeMessageTemporary("Код приглашения скопирован в буфер обмена", 5);
    }

    private void ShowSelectedGroupInfo()
    {
        titleGroup.text = "Группа " + listGroups[selectedGroup].title;
        //список учеников и редактирование тестов
    }

    async void RenameGroup()
    {
        string newGroupTitle = inputNewTitle.text;

        if (newGroupTitle == "")
            gl.ChangeMessageTemporary("Введите новое название группы", 5);
        else
        {
            if (CheckIfTitleExists(newGroupTitle))
                gl.ChangeMessageTemporary("Группа с таким названием уже существует", 5);
            else
            {
                var response = await GroupService.updateGroup(jwt, newGroupTitle, listGroups[selectedGroup].groupId);
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
                    ShowGroupsList();                    
                    menuRenameGroup.SetActive(false);
                    titleGroup.text = "Группа " + newGroupTitle;
                    gl.ChangeMessageTemporary("Название группы успешно обновлено", 5);
                }
            }
        }     
    }

    bool CheckIfTitleExists(string groupTitle)
    {
        bool compare = false;
        int i = 0;
        while (!compare && i < listGroups.Count)
        {
            if (groupTitle == listGroups[i].title) compare = true;
            else i++;
        }
        return compare;
    }
      
    public async void DeleteGroup()
    {        
        var response = await GroupService.deleteGroup(jwt, listGroups[selectedGroup].groupId);
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
            ShowGroupsList();
            //возвращаем меню           
            menuDeleteGroup.SetActive(false);
            menuGroup.SetActive(false);
            menuGroupsList.SetActive(true);
            gl.ChangeMessageTemporary("Группа успешно удалена", 5);            
        }       
    }

    public async void CreateGroup()
    {
        string groupTitle = inputCreateTitle.text;

        if (groupTitle == "")
            gl.ChangeMessageTemporary("Введите название группы", 5);
        else
        {
            if (CheckIfTitleExists(groupTitle))
                gl.ChangeMessageTemporary("Группа с таким названием уже существует", 5);
            else
            {
                var response = await GroupService.createGroup(groupTitle, jwt);
                if (response.isError)
                    switch (response.message)
                    {
                        case Message.CanNotCreateGroup:
                            gl.ChangeMessageTemporary("Не удалось создать группу", 5);
                            break;
                        case Message.NotFoundRequiredData:
                            gl.ChangeMessageTemporary("Проверьте правильность заполнения полей", 5);
                            break;
                        case Message.AccessDenied:
                            gl.ChangeMessageTemporary("Доступ ограничен. Дождитесь подтверждения регистрации", 5);
                            break;
                        default:
                            gl.ChangeMessageTemporary(response.message.ToString(), 5);
                            break;
                    }
                else
                {
                    ShowGroupsList();                    
                    menuCreateGroup.SetActive(false);
                    menuGroupsList.SetActive(true);
                    gl.ChangeMessageTemporary("Группа успешно создана, проверьте список", 5);
                }
            }
        }        
    }
}
