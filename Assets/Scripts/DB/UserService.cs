using System.Collections.Generic;
using UnityEngine;

// Класс, описывающий работу с базой данных с таблицей Users
public static class UserService
{
    //Текущей пользователь
    public static User user = null; // По умолчанию равен null

    //Перечисление с ролями пользователя
    public enum RolesEnum
    {
        Admin = 0,
        Teacher = 1,
        Student = 2
    }

    //Словарь для перевода из перечисления в строку
    private static Dictionary<RolesEnum, string> RolesDict = new Dictionary<RolesEnum, string>()
    {
        {RolesEnum.Admin, "ADMIN"},
        {RolesEnum.Teacher, "TEACHER"},
        {RolesEnum.Student, "STUDENT"}

    };

    // Временное поле список пользователей
    private static List<User> users = new List<User>();

    //Временное поле по выдачи id
    private static int id = 3;

    // Имитируем работу с БД. Инициализируем начальное состояние БД
    public static void initUsers()
    {
        //Имитируем работу с базой данных
        //Создаем каждого пользователя с разными ролями
        User admin = new User(0, "Артем", "Ельденев", "Тавросович", RolesDict[RolesEnum.Admin], true, "admin", "admin");
        User teacher = new User(1, "Александр", "Крюков", "Федорович", RolesDict[RolesEnum.Teacher], true, "teacher", "teacher");
        User student = new User(2, "Светлана", "Борисова", "Вячеславовна", RolesDict[RolesEnum.Student], false, "student", "student");
        //Добавляем их в базу
        users.Add(admin);
        users.Add(teacher);
        users.Add(student);
    }

    //Методы работы с базой данных
    //Вход в систему
    public static Response<string> login(string _login, string _password)
    {
        // Определяем существует ли пользователь с таким логином и паролем
        try
        {
            foreach (var u in users)
            {
                if (u.login == _login)
                {
                    if (u.password == _password)
                    {
                        //Назнаем текущего пользователя
                        user = u;
                        return new Response<string>(false, Message.NotError, null);
                    }
                    return new Response<string>(true, Message.IncorrectPassword, null);
                }
            }
            return new Response<string>(true, Message.NotFoundUser, null);
        } catch
        {
            return new Response<string>(true, Message.CanNotGetUsers, null);
        }

    }

    //Регистрация в системе
    public static Response<string> registration(string _firstName, string _lastName,
        string _middleName, RolesEnum _role, bool _isActivated, string _login, string _password)
    {
        //Создаем пользователя
        User newUser = new User(id, _firstName, _lastName, _middleName, RolesDict[_role], _isActivated, _login, _password);
        // Добавляем пользователя в БД
        try
        {
            users.Add(newUser);
            //Увеличиваем id
            id++;
            // Делаем нового пользователя - текущим
            user = newUser;
            return new Response<string>(false, Message.NotError, null);
        }
        catch
        {
            return new Response<string>(true, Message.CanNotRegistrationUser, null);
        }
    }

    //Обновление данных о пользователе
    public static Response<string> updateUser(string _firstName, string _lastName,
        string _middleName, RolesEnum _role, bool _isActivated)
    {
        // Проверяем выполнен ли вход
        if (user != null)
        {
            //Создаем пользователя
            User updatedUser = new User(user.id, _firstName, _lastName, _middleName, RolesDict[_role], _isActivated, user.login, user.password);
            // Обновляем пользователя в БД
            try
            {
                // Ищем пользователя с заданным id
                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].id == user.id)
                    {
                        //Обновляем пользователя в БД
                        users[i] = updatedUser;
                        //Обновляем пользователя локально
                        user = updatedUser;
                        return new Response<string>(false, Message.NotError, null);
                    }
                }
                return new Response<string>(true, Message.NotFoundUser, null);
            }
            catch
            {
                return new Response<string>(true, Message.CanNotRegistrationUser, null);
            }
        }
        // Если пользователь не вошел в систему
        return new Response<string>(true, Message.NotAuthorization, null);
    }

    //Выход из системы
    public static void logout()
    {
        user = null;
    }

    //Получение всех пользователей
    public static Response<List<User>> getAllUsers()
    {
        // Разрешенные роли
        RolesEnum[] accessUsers = new RolesEnum[] { RolesEnum.Admin };

        // Проверка на доступ
        if (checkAccess(accessUsers))
        {
            // Получение всех пользователей
            try
            {
                return new Response<List<User>>(false, Message.NotError, users);
            }
            catch
            {
                return new Response<List<User>>(true, Message.CanNotGetUsers, null);
            }
        }
        return new Response<List<User>>(true, Message.AccessDenied, null);

    }

    // Активация учителя
    public static Response<bool> activateTeacher(int _id)
    {
        //Разрешенные роли
        RolesEnum[] accessUses = new RolesEnum[] { RolesEnum.Admin };

        // Проверка на доступ
        if (checkAccess(accessUses))
        {
            //Получение пользователя с заданным id
            try
            {
                foreach (var u in users)
                {
                    //Ищем учителя с заданным id
                    if (u.id == _id && u.role == RolesDict[RolesEnum.Teacher])
                    {
                        // Обновления данные в базе данных
                        try
                        {
                            u.isActivated = true;
                            return new Response<bool>(true, Message.NotError, true);
                        } catch
                        {
                            return new Response<bool>(true, Message.CanNotActivateUser, false);
                        }
                    }
                }
                //Если пользователь не найден
                return new Response<bool>(true, Message.NotFoundUser, false);
            }
            catch
            {
                //Если возникла ошибка при поиске пользователя
                return new Response<bool>(true, Message.NotFoundUser, false);
            }
        }
        //Если нет доступа
        return new Response<bool>(true, Message.AccessDenied, false);

    }

    //Вступление ученика в группу (пока сделана просто активация)
    public static Response<bool> studentJoinGroup()
    {
        //Разрешенные роли
        RolesEnum[] accessUses = new RolesEnum[] { RolesEnum.Student };

        // Проверка на доступ
        if (checkAccess(accessUses))
        {
            //Получение пользователя с заданным id
            try
            {
                foreach (var u in users)
                {
                    //Ищем учителя с заданным id
                    if (u.id == user.id)
                    {
                        // Обновления данные в базе данных
                        try
                        {
                            u.isActivated = true;
                            return new Response<bool>(true, Message.NotError, true);
                        }
                        catch
                        {
                            return new Response<bool>(true, Message.CanNotActivateUser, false);
                        }
                    }
                }
                //Если пользователь не найден
                return new Response<bool>(true, Message.NotFoundUser, false);
            }
            catch
            {
                //Если возникла ошибка при поиске пользователя
                return new Response<bool>(true, Message.NotFoundUser, false);
            }
        }
        //Если нет доступа
        return new Response<bool>(true, Message.AccessDenied, false);
    }

    //Проверка на доступ
    private static bool checkAccess(RolesEnum[] roles)
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

