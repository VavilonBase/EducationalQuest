using System.Collections.Generic;
using System.Threading.Tasks;

public static class ResultService
{
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
