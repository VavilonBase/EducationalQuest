
using System.Collections.Generic;
using UnityEngine;

class QuestionTests : MonoBehaviour
{
    [SerializeField] private int quesitonId;
    [SerializeField] private int testId;
    [SerializeField] private string question;
    [SerializeField] private bool isText;
    [SerializeField] private int scores;
    [SerializeField] private string path;
    [SerializeField] private string jwt;
    [SerializeField] public List<ResponseAnswerWithQuestion> answers;

    [ContextMenu("Create Question")]
    [System.Obsolete]
    public async void create()
    {
        var response = await QuestionService.createQuestion(jwt, testId, isText, path, scores);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Question data = response.data;
            quesitonId = data.questionId;
            question = data.question;
            testId = data.testId;
            isText = data.isText;
            scores = data.scores;
        }
    }

    [ContextMenu("Update Question")]
    [System.Obsolete]
    public async void update()
    {
        var response = await QuestionService.updateQuestion(jwt, quesitonId, isText, path, scores);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Question data = response.data;
            quesitonId = data.questionId;
            question = data.question;
            testId = data.testId;
            isText = data.isText;
            scores = data.scores;
        }
    }

    [ContextMenu("Delete Question")]
    [System.Obsolete]
    public async void delete()
    {
        var response = await QuestionService.delete(jwt, quesitonId);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Debug.Log("OK");
        }
    }

    [ContextMenu("Get Question With Answers")]
    [System.Obsolete]
    public async void getQuestionWithAnswers()
    {
        var response = await QuestionService.getQuestionWithAnswers(jwt, quesitonId);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            ResponseQuestionWithAnswers data = response.data;
            quesitonId = data.questionId;
            question = data.question;
            testId = data.testId;
            isText = data.isText;
            scores = data.scores;
            answers = data.answers;
        }
    }
}

