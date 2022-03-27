using System.Collections.Generic;
using System.IO;

public static class GroupService
{
    /// <summary>
    /// Получение всех групп
    /// </summary>
    /// <returns>Все группы</returns>
    public static Response<List<GroupDto>> getAllGroups()
    {
        Group group1 = new Group(0, 1, "Информатика", "dsqfew141");
        Group group2 = new Group(1, 3, "Базы данных", "gdwewegwd");
        Group group3 = new Group(2, 1, "Unity", "fsdfewed");
        List<Group> groups = new List<Group>()
        {
            new Group(0, 1, "Информатика", "dsqfew141"),
            new Group(1, 3, "Базы данных", "gdwewegwd"),
            new Group(2, 1, "Unity", "fsdfewed")
        };
        // Проверяем доступ
        bool isAccess = true;
        // Подключение к БД
        try
        {
            if (isAccess)
            {
                List<GroupDto> groupsDto = new List<GroupDto>();
                for (int i = 0; i < groups.Count; i++)
                {
                    groupsDto.Add(groups[i]);
                }
                return new Response<List<GroupDto>>(false, Message.NotError, groupsDto);
            } else
            {
                return new Response<List<GroupDto>>(true, Message.AccessDenied, null);
            }
        }
        catch
        {
            return new Response<List<GroupDto>>(true, Message.InternalServer, null);
        }
    }
    
    /// <summary>
    /// Получение группы по ID
    /// </summary>
    /// <param name="groupId">Идентификатор группы</param>
    /// <returns>Ответ с группой или ошибкой</returns>
    public static Response<GroupDto> getGroupByGroupId(int groupId)
    {
        // Существует ли группы
        bool groupExist = true;
        // Пробуем получить группу из базы данных
        try
        {
            if (groupExist)
            {
                return new Response<GroupDto>(false, Message.NotError, new Group(2, 1, "Unity", "fsdfewed"));
            } else
            {
                return new Response<GroupDto>(true, Message.InternalServer, null);
            }
        }
        catch
        {
            return new Response<GroupDto>(true, Message.CanNotGetGroup, null);
        }
    }

    /// <summary>
    /// Получение всех групп учителя по его ID
    /// </summary>
    /// <param name="userId">Идентификатор учителя</param>
    /// <returns>Ответ с группами или с ошибкой</returns>
    public static Response<List<GroupDto>> getGroupsByTeacherId(int userId)
    {
        // Существует ли пользователь
        bool userExist = true;
        // Является ли найденный пользователь учителем
        bool isTeacher = true;
        // Пробуем найти все группы учителя
        try
        {
            if (userExist)
            {
                if (isTeacher)
                {
                    return new Response<List<GroupDto>>(false, Message.NotError, new List<GroupDto>() {
                        new Group(0, 1, "Информатика", "dsqfew141"),
                        new Group(1, 3, "Базы данных", "gdwewegwd"),
                        new Group(2, 1, "Unity", "fsdfewed")
                });
                } else
                {
                    return new Response<List<GroupDto>>(true, Message.IsNotTeacher, null);
                }
            } else
            {
                return new Response<List<GroupDto>>(true, Message.NotFoundUser, null);
            }
        }
        catch
        {
            return new Response<List<GroupDto>>(true, Message.CanNotGetGroups, null);
        }
    }

    /// <summary>
    /// Получение ключевого слова для группы
    /// </summary>
    /// <param name="groupId">Идентификатор группы</param>
    /// <returns></returns>
    public static Response<string> getGroupCodeWord(int groupId)
    {
        // Проверяем существует ли группы
        bool groupExist = true;
        // Проверяем является ли пользователь создателем группы
        bool isCreator = true;
        // Подключение к БД
        try
        {
            if (groupExist)
            {
                if (isCreator)
                {
                    return new Response<string>(false, Message.NotError, "dsqfew141");
                } else
                {
                    return new Response<string>(true, Message.IsNotCreator, null);
                }
            } else
            {
                return new Response<string>(true, Message.NotFoundGroup, null);
            }
        }
        catch
        {
            return new Response<string>(true, Message.CanNotGetGroup, null);
        }
    }

    /// <summary>
    /// Создание группы
    /// </summary>
    /// <param name="_title">Название группы</param>
    /// <returns></returns>
    public static Response<Group> createGroup(string _title)
    {
        // Проверяем на доступность
        bool isAccess = true;
        // Подключаемся к БД
        try
        {
            // Генерируем кодовое слово
            string _codeWord = Path.GetRandomFileName();
            return new Response<Group>(false, Message.NotError, new Group(0, 1, _title, _codeWord));
        } catch
        {
            return new Response<Group>(true, Message.InternalServer, null);
        }
    }

    /// <summary>
    /// Обновление группы
    /// </summary>
    /// <param name="_groupId">Идентификатор группы</param>
    /// <param name="_title">Название группы</param>
    /// <returns></returns>
    public static Response<Group> updateGroup(int _groupId, string _title)
    {
        //Проверяем доступность
        bool isAccess = true;
        // Проверяем существует ли группы
        bool groupExist = true;
        try
        {
            if (isAccess)
            {
                if (groupExist)
                {
                    return new Response<Group>(false, Message.NotError, new Group(0, 1, _title, "dsfsdfs"));
                } else
                {
                    return new Response<Group>(true, Message.NotFoundGroup, null);
                }
            } else
            {
                return new Response<Group>(true, Message.AccessDenied, null);
            }
        } catch
        {
            return new Response<Group>(true, Message.InternalServer, null);
        }
    }

    /// <summary>
    /// Получение все групп ученика по его ID
    /// </summary>
    /// <param name="_userId">Идентификатор ученика</param>
    /// <returns>Возвращает группы </returns>
    public static Response<List<GroupDto>> getGroupsStudentsByStudentId(int _userId)
    {
        // Проверяем существует ли пользователь
        bool userExist = true;
        // Проверка явялется ли пользователь учеников
        bool isStudent = true;
        try
        {
            if (userExist)
            {
                if (isStudent)
                {
                    return new Response<List<GroupDto>>(false, Message.NotError, new List<GroupDto>()
                    {
                        new Group(0, 1, "Информатика", "dsqfew141"),
                        new Group(1, 3, "Базы данных", "gdwewegwd"),
                        new Group(2, 1, "Unity", "fsdfewed")
                    });
                } else
                {
                    return new Response<List<GroupDto>>(true, Message.IsNotStudent, null);
                }
            } else
            {
                return new Response<List<GroupDto>>(true, Message.NotFoundUser, null);
            }
        } catch
        {
            return new Response<List<GroupDto>>(true, Message.NotError, null);
        }
    }

    /// <summary>
    /// Получение всех учеников группы по ее ID
    /// </summary>
    /// <param name="_groupId">Идентификатор группы</param>
    /// <returns>Список учеников группы</returns>
    public static Response<List<UserDto>> getStudentsFromGroupByGroupId(int _groupId)
    {
        // Проверяем существует ли группы
        bool groupExist = true;
        // Поиск группы
        try
        {
            if (groupExist)
            {
                return new Response<List<UserDto>>(false, Message.NotError, new List<UserDto>()
                {
                    new UserDto(0, "Артем", "Ельденев", "Тавросович", UserService.RolesDict[UserService.RolesEnum.Admin], true),
                    new UserDto(1, "Александр", "Крюков", "Федорович", UserService.RolesDict[UserService.RolesEnum.Teacher], true),
                    new UserDto(3, "Светлана", "Борисова", "Вячеславовна", UserService.RolesDict[UserService.RolesEnum.Student], true),
                });
            } else
            {
                return new Response<List<UserDto>>(true, Message.NotFoundGroup, null);
            }
        } catch
        {
            return new Response<List<UserDto>>(true, Message.InternalServer, null);
        }
    }

    /// <summary>
    /// Вступление текущего пользователя в группу
    /// </summary>
    /// <param name="_groupId">Идентификатор группы</param>
    /// <param name="_codeWord">Кодовое слово группы</param>
    /// <returns>Ответ, с true - если удалось добавить ученика, иначе - false</returns>
    public static Response<bool> joinStudentToGroupByGroupId(int _groupId, string _codeWord)
    {
        // Проверяем существует ли группы
        bool groupExist = true;
        // Проверяем правильность кодового слова
        bool codeWordRigth = true;
        // Поиск группы
        try
        {
            if (groupExist)
            {
                if (codeWordRigth)
                {
                    return new Response<bool>(false, Message.NotError, true);
                } else
                {
                    return new Response<bool>(true, Message.IncorrectCodeWord, false);
                }
            }
            else
            {
                return new Response<bool>(true, Message.NotFoundGroup, false);
            }
        }
        catch
        {
            return new Response<bool>(true, Message.InternalServer, false);
        }
    }
}