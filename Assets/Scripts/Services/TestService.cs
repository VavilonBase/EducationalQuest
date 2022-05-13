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
        var requestCreateTestData = new RequestCreateTestData() { groupId = _groupId, title = _title, canViewResults = _canViewResults };
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
        var requestUpdateTestData = new RequestUpdateTestData() { testId = _testId, title = _title, canViewResults = _canViewResults };
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
        var result = await httpClient.Delete<object>(url);
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
    public async static Task<Response<List<Test>>> getAllGroupTests(string jwt, int _groupId)
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
    /// Открытие теста
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
    /// Закрытие теста
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

    /// <summary>
    /// Получение теста с вопросами
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_testId">ID теста</param>
    /// <returns>Возвращает тест с вопросами, либо ошибку
    /// При вызове этого метода учеников, isRightAnswer = false всегда
    /// Ошибка:
    /// IncorrectTokenFormat
    /// AccessDenied
    /// NotFoundRequiredData
    /// TestNotExist
    /// UserIsNotCreatorTest
    /// TestHasNotQuestions
    /// </returns>
    public async static Task<Response<ResponseTestWithQuestion>> getTestWithQuestion(string jwt, int _testId)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/test/getTestWithQuestion?testId=" + _testId;
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Get<ResponseTestWithQuestion>(url);
        return result;
    }

    /// <summary>
    /// Получение результата ученика за тест без правильных ответов, только результат
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_testId">ID теста</param>
    /// <returns>Массив (так как ученик мог написать тест больше 1 раза) результатов ученика за тест
    /// Ошибка:
    /// IncorrectTokenFormat
    /// AccessDenied
    /// NotFoundRequiredData
    /// NotFoundResult
    /// </returns>
    public async static Task<Response<List<ResponseTestWithQuestion>>> getStudentTestResult(string jwt, int _studentId, int _testId)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/test/getStudentTestResult?studentId=" + _studentId + "&testId=" + _testId;
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Get<List<ResponseTestWithQuestion>>(url);
        return result;
    }

    /// <summary>
    /// Получение результата ученика за тест с правильными ответами
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_testId">ID теста</param>
    /// <param name="_studentId">ID ученика</param>
    /// <returns>Массив (так как ученик мог написать тест больше 1 раза) результатов ученика за тест с ответами
    /// Ошибка:
    /// IncorrectTokenFormat
    /// AccessDenied
    /// TestNotExist
    /// NotFoundRequiredData
    /// UserIsNotCreatorTest
    /// NotFoundResult
    /// NotFoundQuestion
    /// </returns>
    public async static Task<Response<ResponseStudentResultWithRightAnswer>> getStudentTestResultWithRightAnswer(string jwt, int _studentId, int _testId)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/test/getStudentTestResultWithRightAnswer?studentId=" + _studentId + "&testId=" + _testId;
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Get<ResponseStudentResultWithRightAnswer>(url);
        return result;
    }

    /// <summary>
    /// Получение результатов всех студентов (обучающихся) за тест
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_testId">ID теста</param>
    /// <returns>Массив (так как ученик мог написать тест больше 1 раза) результатов ученика за тест с ответами
    /// Ошибка:
    /// IncorrectTokenFormat
    /// AccessDenied
    /// TestNotExist
    /// NotFoundRequiredData
    /// UserIsNotCreatorTest
    /// NotFoundResult
    /// NotFoundQuestion
    /// </returns>
    public async static Task<Response<List<ResponseStudentTestResult>>> getAllResultsTestsForStudents(string jwt, int _testId)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/test/getAllResultsTestForStudents?testId=" + _testId;
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Get<List<ResponseStudentTestResult>>(url);
        return result;
    }

    /// <summary>
    /// Получение максимального количества очков за тест
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_testId">Идентификатор теста</param>
    /// <returns></returns>
    public async static Task<Response<int>> getMaxScoresForTestByTestId(string jwt, int _testId)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/test/getMaxScoresForTestByTestId?testId=" + _testId;
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Get<int>(url);
        return result;
    }
}