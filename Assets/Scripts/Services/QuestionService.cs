using System.Threading.Tasks;
using UnityEngine;

public static class QuestionService
{
    /// <summary>
    /// Создание вопроса
    /// </summary>
    /// <param name="title">Название группы</param>
    /// <param name="jwt">Токен</param>
    /// <returns>Возвращает созданную группу
    /// Ошибки:
    /// IncorrectTokenFormat
    /// AccessDenied
    /// CanNotCreateGroup
    /// DBErrorExecute
    /// </returns>
    public async static void createQuestion()
    {

        /*// Задаем URL
        string url = "https://educationalquest.herokuapp.com/group/create";
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Создаем данные
        RequestCreateGroupData requestCreateGroupData = new RequestCreateGroupData() { title = _title };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Post<Group, RequestCreateGroupData>(requestCreateGroupData, url);
        return result;*/
    }
}
