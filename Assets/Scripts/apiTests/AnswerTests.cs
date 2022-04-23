
using UnityEngine;

class AnswerTests : MonoBehaviour
{
    [SerializeField] private int answerId;
    [SerializeField] private int quesitonId;
    [SerializeField] private string answer;
    [SerializeField] private bool isText;
    [SerializeField] private bool isRightAnswer;
    [SerializeField] private string path;
    [SerializeField] private string jwt;

    [ContextMenu("Create Answer")]
    [System.Obsolete]
    public async void create()
    {
        var response = await AnswerService.createAnswer(jwt, quesitonId, path, isText, isRightAnswer);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Answer data = response.data;
            answerId = data.answerId;
            quesitonId = data.questionId;
            answer = data.answer;
            isText = data.isText;
            isRightAnswer = data.isRightAnswer;
        }
    }

    [ContextMenu("Update Answer")]
    [System.Obsolete]
    public async void update()
    {
        var response = await AnswerService.updateAnswer(jwt, answerId, quesitonId, path, isText, isRightAnswer);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Answer data = response.data;
            answerId = data.answerId;
            quesitonId = data.questionId;
            answer = data.answer;
            isText = data.isText;
            isRightAnswer = data.isRightAnswer;
        }
    }

    [ContextMenu("Delete Answer")]
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
}
