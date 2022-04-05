public enum Message
{
    // Общие
    NotError = 0, // Нет ошибки

    // Токен
    JWTError = 100, // Ошибка с jwt
    IncorrectTokenFormat = 101, // Токена не было вообще или был, но неправильного формата
    UnauthorizedUser = 102, // Пользователь не авторизован
    AccessDenied = 103, // Нет доступа
    NotFoundQueryParam = 104, // Не передан query параметр в url
    NotFountRequiredData = 105, // Нет обязательных данных

    // База данных
    DBErrorExecute = 200, // Ошибка при выполнении запроса в базе данных

    // Пользователи
    CanNotCreateUser = 300, // Невозможно создать пользователя, возможно какое-то значение не передано, либо нет доступа к базе данных
    UserExist = 301, // Ошибка при регистрации пользователя, когда такой пользователь уже существует
    UserNotExist = 302, // Ошибка при обращении к несуществующему пользователю
    IncorrectPassword = 303, // Неверный пароль
    CanNotUpdateUser = 304, // Ошибка при обновлении пользователя
    CanNotGetUsers = 305, // Ошибка при получении пользователей
    IsNotTeacher = 306, // Пользователь не является учителем
    PassowordNotEquals = 307, // При смене пароля, пароли не совпали

    // Группы
    CanNotCreateGroup = 400, // Не удалось создать группу
    TeacherHasNotGroups = 401, // У учителя нет групп
    GroupNotFound = 402, // Группа не найдена
    StudentIsInAGroup = 403, // Ученик уже находится в группе
    CanNotJoinStudentInTheGroup = 404, // Не удалось добавить ученика в группу
    StudentHasNotGroups = 405, // У ученика нет групп
    GroupHasNotStudents = 406, // У группы нет учеников
    UserIsNotCreatorGroup = 407, // Пользователь не является создателем группы
    CanNotUpdateGroup = 408, // Не удалось обновить группы
    CanNotDeleteGroup = 409, // Не удалось удалить группу
}