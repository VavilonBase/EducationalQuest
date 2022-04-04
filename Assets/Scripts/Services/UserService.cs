using System.Collections.Generic;
using System.Threading.Tasks;

// Класс, описывающий работу с базой данных с таблицей Users
public static class UserService
{

    //Перечисление с ролями пользователя
    public enum RolesEnum
    {
        Admin = 0,
        Teacher = 1,
        Student = 2
    }

    //Словарь для перевода из перечисления в строку
    public static Dictionary<RolesEnum, string> RolesDict = new Dictionary<RolesEnum, string>()
    {
        {RolesEnum.Admin, "ADMIN"},
        {RolesEnum.Teacher, "TEACHER"},
        {RolesEnum.Student, "STUDENT"}

    };

    //Методы работы с базой данных
    /// <summary>
    /// Вход в систему
    /// </summary>
    /// <param name="_login">Логин</param>
    /// <param name="_password">Пароль</param>
    /// <returns>Возвращает ответ либо с null, либо с ошибкой</returns>
    public async static Task<Response<ResponseUserData>> login(string _login, string _password)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/api/users/login.php";
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Создаем данные
        RequestLoginData requestLoginData = new RequestLoginData() { login = _login, password = _password };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Post<ResponseUserData, RequestLoginData>(requestLoginData, url);
        return result;
    }

    /// <summary>
    /// Регистрация в системе
    /// </summary>
    /// <param name="_firstName">Имя</param>
    /// <param name="_lastName">Фамилия</param>
    /// <param name="_middleName">Отчество</param>
    /// <param name="_role">Роль</param>
    /// <param name="_isActivated">Активирован ли пользователь</param>
    /// <param name="_login">Логин</param>
    /// <param name="_password">Пароль</param>
    /// <returns>Возвращает ответ либо с null, либо с ошибкой</returns>
    public async static Task<Response<ResponseUserData>> registration(string _firstName, string _lastName,
        RolesEnum _role, bool _isActivated, string _login, string _password, string _middleName = "")
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/api/users/registration.php";
        // Инициализируем соединение
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Подготавливаем данные
        RequestRegisrationData requestRegisrationData = new RequestRegisrationData()
        {
            lastName = _lastName,
            firstName = _firstName,
            middleName = _middleName,
            role = RolesDict[_role],
            login = _login,
            password = _password
        };
        // Отправляем запрос и ждем ответ
        var result = await httpClient.Post<ResponseUserData, RequestRegisrationData>(requestRegisrationData, url);
        return result;
    }
    /// <summary>
    /// Обновление токена пользователя
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <returns>Все данные о пользователе и новый токен</returns>
    public async static Task<Response<ResponseUserData>> refresh(string jwt)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/api/users/refresh_token.php";
        // Инициализируем соединение
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Подготавливаем данные (устанавливаем заголовки)
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        var result = await httpClient.Get<ResponseUserData>(url);
        return result;
    }
/// <summary>
/// Обновление данных о текущем пользователе
/// </summary>
/// <param name="_firstName">Имя</param>
/// <param name="_lastName">Фамилия</param>
/// <param name="_middleName">Отчество</param>
/// <param name="_role">Роль</param>
/// <param name="_jwt">Токен</param>
/// <returns>Возвращает ответ либо с null, либо с ошибкой</returns>
public async static Task<Response<ResponseUserData>> updateUser(string _firstName, string _lastName,
    string _middleName, RolesEnum _role, string jwt)
{
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/api/users/update_user.php";
        // Инициализируем соединение
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}" }
        };
        // Подготавливаем данные
        RequestUpdateUserData requestUpdateUserData = new RequestUpdateUserData()
        {
            lastName = _firstName,
            firstName = _lastName,
            middleName = _middleName,
            role = RolesDict[_role],
        };

        var result = await httpClient.Post<ResponseUserData, RequestUpdateUserData>(requestUpdateUserData, url);
        return result;
    }



    /// <summary>
    /// Получение всех пользователей
    /// </summary>
    /// <returns>Возвращает ответ со списком пользователей, либо с ошибкой</returns>
    public async static Task<Response<List<User>>> getAllUsers(string jwt)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/api/users/get_all_users.php";
        // Инициализируем соединение
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}" }
        };

        var result = await httpClient.Get<List<User>>(url);
        return result;
    }

    /// <summary>
    /// Активация учителя
    /// </summary>
    /// <param name="teacherId">Идентификатор учителя</param>
    /// <param name="jwt">Токен</param>
    /// <returns>Возвращает учителя, если он был активирован, иначе ошибку</returns>
    public async static Task<Response<ResponseUser>> activateTeacher(int teacherId, string jwt)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/api/users/activate_teacher.php?teacherId=" + teacherId.ToString();

        // Инициализируем соединение
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}" }
        };

        var result = await httpClient.Get<ResponseUser>(url);
        return result;
    }

    /// <summary>
    /// Изменение пароля
    /// </summary>
    /// <param name="_lastPassword">Старый пароль</param>
    /// <param name="_newPassword">Новый пароль</param>
    /// <param name="jwt">Токен</param>
    /// <returns>Возвращает пользователя, если пароль был изменен, иначе ошибку</returns>
    public async static Task<Response<ResponseUser>> changePassword(string _lastPassword, string _newPassword, string jwt)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/api/users/change_password.php";

        // Инициализируем соединение
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}" }
        };

        // Подготавливаем данные
        RequestChangePasswordData requestChangePasswordData = new RequestChangePasswordData()
        {
            lastPassword = _lastPassword,
            password = _newPassword
        };
        var result = await httpClient.Post<ResponseUser, RequestChangePasswordData>(requestChangePasswordData, url);
        return result;
    }

    /// <summary>
    /// Получение одного пользователя по ID
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns>Возвращает ответ с пользователем, либо с ошибкой</returns>
    /* public static Response<UserDto> getUserByUserId(int userId)*/
    /*{
        List<User> users = new List<User>()
        {
            new User(0, "Артем", "Ельденев", "Тавросович", RolesDict[RolesEnum.Admin], true, "admin", "admin"),
            new User(1, "Александр", "Крюков", "Федорович", RolesDict[RolesEnum.Teacher], true, "teacher", "teacher"),
            new User(3, "Светлана", "Борисова", "Вячеславовна", RolesDict[RolesEnum.Student], false, "student", "student"),
        };
        // Поиск пользователя

        try
        {
            foreach (var u in users)
            {
                if (u.userId == userId)
                {
                    return new Response<UserDto>(false, message.NotError, u);
                }
            }
            return new Response<UserDto>(false, message.NotFoundUser, null);
        }
        catch
        {
            return new Response<UserDto>(true, message.InternalServer, null);
        }


    }*/

    /*  /// <summary>
      /// Активация учителя
      /// </summary>
      /// <param name="_id">Идентификатор учителя</param>
      /// <returns>Возвращает ответ с true, если активация прошла успешно, иначе false</returns>
      public static Response<bool> activateTeacher(int _id)
      {
          // Проверяем существует ли пользователь
          bool userExist = true;
          // Проверяем есть ли доступ
          bool isAccess = true;
          // Проверка является ли пользователь учителем
          bool isTeacher = true;
          User teacher = new User(1, "Александр", "Крюков", "Федорович", RolesDict[RolesEnum.Teacher], false, "teacher", "teacher");

          // Подключение к БД
          try
          {
              if (userExist)
              {
                  if (isAccess)
                  {
                      if (isTeacher)
                      {
                          teacher.isActivated = true;
                          return new Response<bool>(false, message.NotError, true);
                      }
                      else
                      {
                          return new Response<bool>(true, message.IsNotTeacher, false);
                      }
                  }
                  else
                  {
                      return new Response<bool>(true, message.AccessDenied, false);
                  }
              }
              else
              {
                  return new Response<bool>(true, message.UserNotExist, false);
              }
          }
          catch
          {
              return new Response<bool>(true, message.InternalServer, false);
          }
      }

      /// <summary>
      /// Проверка пользователя на доступ
      /// </summary>
      /// <param name="roles">Доступные роли пользователя</param>
      /// <returns>true - если доступ есть, иначе false</returns>
      public static bool checkAccess(RolesEnum[] roles)
      {
          //Проходимся по всем разрешенным ролям
          foreach (var r in roles)
          {
              if (RolesDict[r] == user.role)
              {
                  return true;
              }
          }
          return false;
      }*/
}