using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.TaskScripts;
using System.Threading.Tasks;
using UnityEngine.UI;

public class MenuTeacherTasksEditor : MonoBehaviour
{
    private CsGlobals gl;
    string jwt;

    private MenuTeacherTestsEditor parentInfo;
    private Group group;
    private Test test;
    private List<ResponseQuestionForTest> questions;

    private GameObject menuTasksList;
    private GameObject menuAddTask;
    private GameObject menuEditTask;

    private GameObject buttonCreateTask;

    [Header("Components")]
    [SerializeField] private TaskListView m_ListViewTasksList;
    [SerializeField] private GameObject m_PrefabTasksList;


    private void Awake()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
        jwt = gl.playerInfo.responseUserData.jwt;
        parentInfo = this.transform.GetComponentInParent<MenuTeacherTestsEditor>();

        menuTasksList = this.transform.Find("UI Task List").gameObject;
        menuAddTask = this.transform.Find("UI Task Add").gameObject;
        menuEditTask = this.transform.Find("UI Task Edit").gameObject;

        buttonCreateTask = menuTasksList.transform.Find("Add New Task Btn").gameObject;
        buttonCreateTask.GetComponent<Button>().onClick.AddListener(delegate
        {
            AddTask();
        });
    }

    private void OnEnable()
    {
        group = parentInfo.GetSelectedGroup();
        test = parentInfo.GetSelectedTest();

        UpdateQuestionList();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async Task<List<ResponseQuestionForTest>> GetQuestionsList()
    {
        var response = await TestService.getTestWithQuestion(jwt, test.testId);
        if (response.isError)
        {
            switch (response.message)
            {
                case Message.TestHasNotQuestions:
                    gl.ChangeMessageTemporary("Тест пуст, добавьте первое задание", 5);
                    break;
                default:
                    gl.ChangeMessageTemporary(response.message.ToString(), 5);
                    break;
            }
            return null;
        }
        else
            return response.data.questions;
    }

    async void UpdateQuestionList()
    {
        m_ListViewTasksList.ClearList();
        questions = await GetQuestionsList();
        if (questions != null)
        {
            for (int i = 0; i < questions.Count; i++)
                CreateElement(questions[i], i);
        }
    }

    void CreateElement(ResponseQuestionForTest question, int num)
    {
        //Создаем новый элемент в списке по prefab
        GameObject element = m_ListViewTasksList.Add(m_PrefabTasksList);
        //Получаем из него объект TaskListElementView
        TaskListElementView elementMeta = element.GetComponent<TaskListElementView>();
        //Заполняем содержимое элемента
        elementMeta.SetTitle("Вопрос "+ num+1 + ":");
        if (question.isText)
        {
            elementMeta.SetTitle("Вопрос " + num + 1 + ":" + question.question);
        }
        else
        {
            elementMeta.SetTitle("Вопрос " + num + 1 + ":");
            //elementMeta.SetImage(question.);
        }                
        elementMeta.SetNumberQuestion(num);
        elementMeta.SetTaskManagerSript(this);
    }

    private async void AddTask()
    {
        //Загрушка
        var responseQuestion = await QuestionService.createQuestion(jwt, test.testId, true, "Тестовый вопрос", 10);
        if (responseQuestion.isError)
            gl.ChangeMessageTemporary(responseQuestion.message.ToString(), 5);
        else
        {
            await AnswerService.createAnswer(jwt, responseQuestion.data.questionId, "Тестовый ответ 1", true, false);
            await AnswerService.createAnswer(jwt, responseQuestion.data.questionId, "Тестовый ответ 2(right)", true, true);
            await AnswerService.createAnswer(jwt, responseQuestion.data.questionId, "Тестовый ответ 3", true, false);
            gl.ChangeMessageTemporary("Наверное, все в порядке", 5);
            UpdateQuestionList();
        }
    }

    public void EditTask(int num)
    {

    }

    public void DeleteTask(int num)
    {

    }
}
