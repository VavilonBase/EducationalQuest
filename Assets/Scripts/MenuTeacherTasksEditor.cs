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
    public int GetTestId() { return test.testId; }
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

        UpdateQuestionsList();
    }

    async Task<List<ResponseQuestionForTest>> GetQuestionsList()
    {
        var response = await TestService.getTestWithQuestion(jwt, test.testId);
        if (response.isError)
        {
            switch (response.message)
            {
                case Message.TestHasNotQuestions:
                    gl.ChangeMessageTemporary("���� ����, �������� ������ �������", 5);
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

    async public void UpdateQuestionsList()
    {
        m_ListViewTasksList.ClearList();
        questions = await GetQuestionsList();
        if (questions != null)
        {
            Debug.Log("���������� ��������: " + questions.Count);
            for (int i = 0; i < questions.Count; i++)
                CreateElement(questions[i], i);
        }
    }

    void CreateElement(ResponseQuestionForTest question, int num)
    {
        Debug.Log(question.questionId + " / " + question.question + " / " + num);
        //������� ����� ������� � ������ �� prefab
        GameObject element = m_ListViewTasksList.Add(m_PrefabTasksList);
        //�������� �� ���� ������ TaskListElementView
        TaskListElementView elementMeta = element.GetComponent<TaskListElementView>();
        //��������� ���������� ��������        
        if (question.isText)
        {
            Debug.Log(question.question);
            elementMeta.SetTitle("������ " + (num+1) + ":" + question.question);
        }
        else
        {
            elementMeta.SetTitle("������ " + (num+1) + ":");
            //elementMeta.SetImage(question.);
        }                
        elementMeta.SetNumberQuestion(question.questionId);
        elementMeta.SetTaskManagerSript(this);
    }

    private async void AddTask()
    {
        menuTasksList.SetActive(false);
        var component = menuAddTask.GetComponent<MenuTeacherTaskChange>();
        component.questionWithAnswersInfo = new MenuTeacherTaskChange.QuestionWithAnswersInfo();
        menuAddTask.SetActive(true);

        /*
        //��������
        var responseQuestion = await QuestionService.createQuestion(jwt, test.testId, true, "�������� ������", 10);
        if (responseQuestion.isError)
            gl.ChangeMessageTemporary(responseQuestion.message.ToString(), 5);
        else
        {
            await AnswerService.createAnswer(jwt, responseQuestion.data.questionId, "�������� ����� 1", true, false);
            await AnswerService.createAnswer(jwt, responseQuestion.data.questionId, "�������� ����� 2(right)", true, true);
            await AnswerService.createAnswer(jwt, responseQuestion.data.questionId, "�������� ����� 3", true, false);
            gl.ChangeMessageTemporary("��������, ��� � �������", 5);
            UpdateQuestionList();
        */
    }

    public void EditTask(int id)
    {

    }

    public async void DeleteTask(int id)
    {
        Debug.Log("id ���������� �������: " + id);

        var response = await QuestionService.delete(jwt, id);
        if (response.isError)
            gl.ChangeMessageTemporary(response.message.ToString(), 5);
        else
        {
            UpdateQuestionsList();
            gl.ChangeMessageTemporary("������ ������� ������", 5);
        }
    }
}
