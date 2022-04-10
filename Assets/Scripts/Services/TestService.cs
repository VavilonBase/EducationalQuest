using System.Collections.Generic;
using System.Threading.Tasks;

public static class TestService
{
    /// <summary>
    /// Создание теста для группы
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_groupId">ID группы</param>
    /// <param name="_title">Название теста</param>
    /// <param name="_canViewResults">Можно ли посмотрить результаты теста</param>
    /// <returns>Созданный тест, или ошибку
    /// Ошибки:
    /// IncorrectTokenFormat
    /// AccessDenied
    /// NotFoundRequiredData
    /// UserIsNotCreatorGroup
    /// CanNotCreateTest
    /// </returns>
    public async static Task<Response<Test>> create(string jwt, int _groupId, string _title, bool _canViewResults)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/test/create";
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Создаем данные
        var requestCreateTestData = new RequestCreateTestData() { groupId = _groupId, title = _title, canViewResult = _canViewResults };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Post<Test, RequestCreateTestData>(requestCreateTestData, url);
        return result;
    }

    /// <summary>
    /// Обновление теста
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_testId">ID теста</param>
    /// <param name="_title">Название теста</param>
    /// <param name="_canViewResults">Можно ли посмотреть результаты</param>
    /// <returns>Обновленный тест или ошибку
    /// Ошибка:
    /// IncorrectTokenFormat
    /// AccessDenied
    /// NotFoundRequiredData
    /// TestNotExist
    /// UserIsNotCreatorGroup
    /// CanNotUpdateTest 
    /// </returns>
    public async static Task<Response<Test>> update(string jwt, int _testId, string _title, bool _canViewResults)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/test/update";
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Создаем данные
        var requestUpdateTestData = new RequestUpdateTestData() { testId = _testId, title = _title, canViewResult = _canViewResults };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Post<Test, RequestUpdateTestData>(requestUpdateTestData, url);
        return result;
    }

    /// <summary>
    /// Удаление теста
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_testId">ID токена</param>
    /// <returns>Либо ничего не возвращает, либо возвращает ошибку
    /// Ошибка:
    /// IncorrectTokenFormat
    /// AccessDenied
    /// NotFoundRequiredData
    /// TestNotExist
    /// UserIsNotCreatorGroup
    /// </returns>
    public async static Task<Response<object>> delete(string jwt, int _testId)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/test/delete?testId=" + _testId;
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Get<object>(url);
        return result;
    }

    /// <summary>
    /// Получение всех тестов группы
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_groupId">ID группы</param>
    /// <returns>Возвращает список тестов группы, либо ошибку
    /// Ошибка:
    /// IncorrectTokenFormat
    /// AccessDenied
    /// NotFoundRequiredData
    /// GroupHasNotTests
    /// </returns>
    public async static Task<Response<List<Test>>> getAllGroupTest(string jwt, int _groupId)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/test/getAllGroupTests?groupId=" + _groupId;
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Get<List<Test>>(url);
        return result;
    }

    /// <summary>
    /// Открывает тест
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_testId">ID теста</param>
    /// <returns>Возвращает тест, либо ошибки
    /// Ошибка:
    /// IncorrectTokenFormat
    /// AccessDenied
    /// NotFoundRequiredData
    /// TestNotExist
    /// UserIsNotCreatorGroup
    /// TestIsClosed
    /// </returns>
    public async static Task<Response<Test>> openTest(string jwt, int _testId)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/test/open?testId=" + _testId;
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Get<Test>(url);
        return result;
    }

    /// <summary>
    /// Закрывает тест
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_testId">ID теста</param>
    /// Ошибка:
    /// IncorrectTokenFormat
    /// AccessDenied
    /// NotFoundRequiredData
    /// TestNotExist
    /// UserIsNotCreatorGroup
    /// TestIsNotOpened
    /// </returns>
    public async static Task<Response<Test>> closeTest(string jwt, int _testId)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/test/close?testId=" + _testId;
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Get<Test>(url);
        return result;
    }
}