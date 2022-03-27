using System.Collections.Generic;
using UnityEngine;
using Npgsql;

// Класс, описывающий работу с базой данных с таблицей Users
public static class UserService
{
    //Текущей пользователь
    public static UserDto user = null; // По умолчанию равен null

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
    public static Response<string> login(string _login, string _password)
    {
        // Определяем существует ли пользователь с таким логином
        bool userIsExist = true;
        // Проверяем правильный ли пароль
        bool passwordIsCorrect = true;
        // Подключение к БД
        try
        {
            if (userIsExist)
            {
                if (passwordIsCorrect)
                {
                    user = new User(0, "Артем", "Ельденев", "Тавросович", RolesDict[RolesEnum.Admin], true, "admin", "admin");
                    return new Response<string>(false, Message.NotError, null);
                } else
                {
                    return new Response<string>(true, Message.IncorrectPassword, null);
                }
            } else
            {
                return new Response<string>(true, Message.NotFoundUser, null);
            }
        }
        catch
        {
            return new Response<string>(true, Message.InternalServer, null);
        }
        List<User> users = new List<User>()
        {
            new User(0, "Артем", "Ельденев", "Тавросович", RolesDict[RolesEnum.Admin], true, "admin", "admin"),
            new User(1, "Александр", "Крюков", "Федорович", RolesDict[RolesEnum.Teacher], true, "teacher", "teacher"),
            new User(3, "Светлана", "Борисова", "Вячеславовна", RolesDict[RolesEnum.Student], false, "student", "student"),
        };
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
    public static Response<string> registration(string _firstName, string _lastName,
        string _middleName, RolesEnum _role, bool _isActivated, string _login, string _password)
    {
        // Проверяем не существует ли пользователь с таким логином
        bool userIsExist = false;
        
        // Подключение к БД
        try
        {
            if (!userIsExist)
            {
                user = new User(0, "Артем", "Ельденев", "Тавросович", RolesDict[RolesEnum.Admin], true, "admin", "admin");
                return new Response<string>(false, Message.NotError, null);
            } else
            {
                return new Response<string>(true, Message.UserExist, null);
            }
        } catch
        {
            return new Response<string>(true, Message.InternalServer, null);
        }
    }

    /// <summary>
    /// Обновление данных о текущем пользователе
    /// </summary>
    /// <param name="_firstName">Имя</param>
    /// <param name="_lastName">Фамилия</param>
    /// <param name="_middleName">Отчество</param>
    /// <param name="_role">Роль</param>
    /// <param name="_isActivated">Активирован ли пользователь</param>
    /// <returns>Возвращает ответ либо с null, либо с ошибкой</returns>
    public static Response<string> updateUser(string _firstName, string _lastName,
        string _middleName, RolesEnum _role, bool _isActivated)
    {
        // Проверяем выполнен ли вход
        bool userIsLogin = true;
        // Подключение к БД
        try
        {
            if (userIsLogin)
            {
                user = new User(1, "Александр", "Крюков", "Федорович", RolesDict[RolesEnum.Teacher], true, "teacher", "teacher");
                return new Response<string>(false, Message.NotError, null);
            } else
            {
                return new Response<string>(true, Message.NotAuthorization, null);
            }
        } catch
        {
            return new Response<string>(true, Message.InternalServer, null);
        }
    }

    /// <summary>
    /// Выход из системы
    /// </summary>
    public static void logout()
    {
        user = null;
    }

    /// <summary>
    /// Получение всех пользователей
    /// </summary>
    /// <returns>Возвращает ответ со списком пользователей, либо с ошибкой</returns>
    public static Response<List<UserDto>> getAllUsers()
    {
        List<User> users = new List<User>()
        {
            new User(0, "Артем", "Ельденев", "Тавросович", RolesDict[RolesEnum.Admin], true, "admin", "admin"),
            new User(1, "Александр", "Крюков", "Федорович", RolesDict[RolesEnum.Teacher], true, "teacher", "teacher"),
            new User(3, "Светлана", "Борисова", "Вячеславовна", RolesDict[RolesEnum.Student], false, "student", "student"),
        };
        // Подключение к БД
        try
        {
            //Получаем данные без логина и пароля
            List<UserDto> usersDtos = new List<UserDto>();

            foreach (var u in users)
            {
                UserDto userDto = u;
                usersDtos.Add(userDto);
            }

            return new Response<List<UserDto>>(false, Message.NotError, usersDtos);

        }
        catch
        {
            return new Response<List<UserDto>>(true, Message.InternalServer, null);
        }
    }

    /// <summary>
    /// Получение одного пользователя по ID
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns>Возвращает ответ с пользователем, либо с ошибкой</returns>
    public static Response<UserDto> getUserByUserId(int userId)
    {
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
                    return new Response<UserDto>(false, Message.NotError, u);
                }
            }
            return new Response<UserDto>(false, Message.NotFoundUser, null);
        }
        catch
        {
            return new Response<UserDto>(true, Message.InternalServer, null);
        }


    }
    
    /// <summary>
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
                        return new Response<bool>(false, Message.NotError, true);
                    }
                    else
                    {
                        return new Response<bool>(true, Message.IsNotTeacher, false);
                    }
                }
                else
                {
                    return new Response<bool>(true, Message.AccessDenied, false);
                }
            } else
            {
                return new Response<bool>(true, Message.UserNotExist, false);
            }
        } catch
        {
            return new Response<bool>(true, Message.InternalServer, false);
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
    }
}

