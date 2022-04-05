
using System.Collections.Generic;
using System.Threading.Tasks;

public static class GroupService
{
    /// <summary>
    /// Создание группы
    /// </summary>
    /// <param name="title">Название группы</param>
    /// <param name="jwt">Токен</param>
    /// <returns>Возвращает созданную группу</returns>
    public async static Task<Response<Group>> createGroup(string _title, string jwt)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/api/groups/create_group.php";
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
    /// Возвращаем все группы учителя
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <returns>Все группы учителя</returns>
    public async static Task<Response<List<Group>>> getAllTeacherGroups(string jwt)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/api/groups/get_all_groups_teacher.php";
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
    /// Вступление ученика в группу
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_codeWord">Кодовое слово</param>
    /// <returns>Возвращаем true, если ученик вступил в группу, иначе возвращает ошибку</returns>
    public async static Task<Response<object>> joinStudentToTheGroup(string jwt, string _codeWord)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/api/groups/student_join_to_the_group.php";
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
    /// <returns>Возвращает все группы студента</returns>
    public async static Task<Response<List<ResponseStudentGroupData>>> getStudentGroups(string jwt)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/api/groups/get_student_groups.php";
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
    /// Получение всех групп студентов
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_groupId">Идентификатор группы</param>
    /// <returns></returns>
    public async static Task<Response<List<ResponseUserGroupData>>> getGroupStudents(string jwt, int _groupId)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/api/groups/get_group_students.php";
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Создаем данные
        var requestGetGroupStudentsData = new RequestWithGroupIdData() { groupId = _groupId };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Post<List<ResponseUserGroupData>, RequestWithGroupIdData>(requestGetGroupStudentsData, url);
        return result;
    }


    /// <summary>
    /// Обновление группы
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_title">Название группы</param>
    /// <param name="_groupId">Идентификатор группы</param>
    /// <returns>Возвращает обновленную группу</returns>
    public async static Task<Response<Group>> updateGroup(string jwt, string _title, int _groupId)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/api/groups/update_group.php";
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
    /// <returns>Возвращает true, если группа удалена или возвращает ошибки</returns>
    public async static Task<Response<object>> deleteGroup(string jwt, int _groupId)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/api/groups/delete_group.php";
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Создаем данные
        var requestWithGroupIdData = new RequestWithGroupIdData() { groupId = _groupId };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Post<object, RequestWithGroupIdData>(requestWithGroupIdData, url);
        return result;
    }
}