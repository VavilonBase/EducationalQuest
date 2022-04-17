using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MenuTeacherGroupsListInteractions : MonoBehaviour
{
    private CsGlobals gl;
    private Dropdown dd;
    private InputField codeWord;
    private GameObject buttonShowGroup;
    private GameObject buttonCopyCodeWord;
    public List<Group> listGroups;
    public int selectedGroup;

    void Awake()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
        dd = this.transform.Find("Dropdown").GetComponent<Dropdown>();
        codeWord = this.transform.Find("CodeWord").GetComponent<InputField>();
        buttonShowGroup = this.transform.Find("But_look").gameObject;
        buttonCopyCodeWord = this.transform.Find("ButtonCopyCodeWord").gameObject;

        dd.onValueChanged.AddListener(delegate {
            DropdownValueChanged();
        });
        dd.value = 0;
    }

    async void OnEnable()
    {        
        dd.ClearOptions();
        buttonShowGroup.SetActive(false);
        buttonCopyCodeWord.SetActive(false);
        codeWord.text = null;

        listGroups = await ShowAllGroupsList();
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

    async Task<List<Group>> ShowAllGroupsList()
    {
        var response = await GroupService.getAllTeacherGroups(gl.playerInfo.responseUserData.jwt);
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
                case Message.IncorrectTokenFormat:
                    Debug.LogError("Incorrect token format");
                    gl.ChangeMessageTemporary("Ошибка подключения. Попробуйте перезайти в аккаунт", 5);
                    break;               
                case Message.DBErrorExecute:
                    gl.ChangeMessageTemporary("Ошибка при выполнении запроса в базе данных", 5);
                    break;
                default:
                    gl.ChangeMessageTemporary("Default Error", 5);
                    break;
            }
            return null;
        }
        else
        {
            Debug.Log("Список групп получен");
            return response.data;
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

    public bool CheckIfTitleExists(string groupTitle)
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
}
