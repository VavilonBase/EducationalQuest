using System.Collections.Generic;
using System.Threading.Tasks;

public static class ResultService
{
    /// <summary>
    /// Создание результата
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_testId">Идентификатор теста</param>
    /// <param name="_answers">Ответы на вопросы</param>
    /// <returns>Возвращает созданный результат или ошибку:
    /// IncorrectTokenFormat
    /// AccessDenied
    /// NotFoundRequiredData
    /// StudentIsNotInAGroup
    /// TestHasNotQuestions
    /// DBErrorExecute
    /// </returns>
    public async static Task<Response<Result>> createResult(string jwt, int _testId, List<AnswerResultQuestionDto> _answers)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/result/create";
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Создаем данные
        var requestData = new CreateResultData() { testId = _testId, answers = _answers };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Post<Result, CreateResultData>(requestData, url);
        return result;
    }
}
