
using System.Collections.Generic;
using System.Threading.Tasks;

public static class GroupService
{
    /// <summary>
    /// Создание группы
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
    public async static Task<Response<Group>> createGroup(string _title, string jwt)
    {
        // Задаем URL
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
        return result;
    }

    /// <summary>
    /// Обновление группы
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_title">Название группы</param>
    /// <param name="_groupId">Идентификатор группы</param>
    /// <returns>Возвращает обновленную группу
    /// Ошибки:
    /// IncorrectTokenFormat
    /// AccessDenied
    /// CanNotUpdateGroup
    /// GroupNotFound
    /// UserIsNotCreatorGroup
    /// DBErrorExecute
    /// </returns>
    public async static Task<Response<Group>> updateGroup(string jwt, string _title, int _groupId)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/group/update";
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Создаем данные
        var requestUpdateGroupData = new RequestUpdateGroupData() { groupId = _groupId, title = _title };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Post<Group, RequestUpdateGroupData>(requestUpdateGroupData, url);
        return result;
    }

    /// <summary>
    /// Удаление группы
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_groupId">Идентификатор группы</param>
    /// <returns>Возвращает true, если группа удалена или возвращает ошибки
    /// Ошибки:
    /// IncorrectTokenFormat
    /// AccessDenied
    /// CanNotDeleteGroup
    /// GroupNotFound
    /// UserIsNotCreatorGroup
    /// DBErrorExecute
    /// </returns>
    public async static Task<Response<object>> deleteGroup(string jwt, int _groupId)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/group/delete?groupId=" + _groupId;
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
    /// Удаление ученика из группы учителем
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_studentId">Идентификатор ученика</param>
    /// <param name="_groupId">Идентификатор группы</param>
    /// <returns>true - если удаление прошло успешно, иначе ошибка
    /// Ошибки:
    /// IncorrectTokenFormat
    /// AccessDenied
    /// NotFoundRequiredData
    /// GroupNotFound
    /// UserIsNotCreatorGroup
    /// DBErrorExecute
    /// </returns>
    public async static Task<Response<bool>> removeStudentFromGroup(string jwt, int _studentId, int _groupId)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/group/removeStudent";
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Создаем данные
        var requestData = new RequestRemoveStudentFromGroup() { groupId = _groupId, studentId = _studentId };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Post<bool, RequestRemoveStudentFromGroup>(requestData, url);
        return result;
    }

    /// <summary>
    /// Выход ученика из группы
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_groupId">Идентификатор группы</param>
    /// <returns>true - если выход прошел успешно, иначе ошибка
    /// Ошибки:
    /// IncorrectTokenFormat
    /// AccessDenied
    /// NotFoundRequiredData
    /// StudentIsNotInAGroup
    /// DBErrorExecute
    /// </returns>
    public async static Task<Response<bool>> leavingStudentFromGroup(string jwt, int _groupId)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/group/leave";
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Создаем данные
        var requestData = new RequestLeavingStudentFromGroup() { groupId = _groupId };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Post<bool, RequestLeavingStudentFromGroup>(requestData, url);
        return result;
    }

    /// <summary>
    /// Получение всех групп учителя
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <returns>Все группы учителя
    /// Ошибки:
    /// IncorrectTokenFormat
    /// AccessDenied
    /// TeacherHasNotGroups
    /// DBErrorExecute
    /// </returns>
    public async static Task<Response<List<Group>>> getAllTeacherGroups(string jwt)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/group/getAllTeacherGroups";
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Get<List<Group>>(url);
        return result;
    }

    /// <summary>
    /// Присоединение ученика к группе
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_codeWord">Кодовое слово</param>
    /// <returns>Возвращаем true, если ученик вступил в группу, иначе возвращает ошибку
    /// Ошибки:
    /// IncorrectTokenFormat
    /// AccessDenied
    /// CanNotJoinStudentInTheGroup
    /// StudentIsInAGroup
    /// DBErrorExecute
    /// </returns>
    public async static Task<Response<object>> joinStudentToTheGroup(string jwt, string _codeWord)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/group/join";
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Создаем данные
        var requestJoinStudentToTheGroupData = new RequestJoinStudentToTheGroupData() { codeWord = _codeWord };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Post<object, RequestJoinStudentToTheGroupData>(requestJoinStudentToTheGroupData, url);
        return result;
    }

    /// <summary>
    /// Получение всех групп студента
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <returns>Возвращает все группы студента
    /// Ошибки:
    /// IncorrectTokenFormat
    /// AccessDenied
    /// StudentHasNotGroups
    /// DBErrorExecute
    /// </returns>
    public async static Task<Response<List<ResponseStudentGroupData>>> getStudentGroups(string jwt)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/group/getAllStudentGroups";
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Get<List<ResponseStudentGroupData>>(url);
        return result;
    }

    /// <summary>
    /// Получение всех учеников группы
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_groupId">Идентификатор группы</param>
    /// <param name="_isStudy">Если true - то будут возвращены только обучающиеся ученики (по умолчанию - true)</param>
    /// <returns>Возвращает список учеников группы
    /// Ошибки:
    /// IncorrectTokenFormat
    /// AccessDenied
    /// GroupHasNotStudents
    /// GroupNotFound
    /// DBErrorExecute
    /// </returns>
    public async static Task<Response<List<ResponseUserGroupData>>> getGroupStudents(string jwt, int _groupId, bool _isStudy = true)
    {
        string url = "";

        // Задаем URL
        if (_isStudy)
        {
            url = "https://educationalquest.herokuapp.com/group/getAllGroupStudyingStudents?groupId=" + _groupId;
        }
        else
        {
            url = "https://educationalquest.herokuapp.com/group/getAllGroupStudents?groupId=" + _groupId;
        }
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Get<List<ResponseUserGroupData>>(url);
        return result;
    }
}