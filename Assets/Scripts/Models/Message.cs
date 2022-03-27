public enum Message
{
    // Общие
    NotError, // Нет ошибки
    AccessDenied, // Доступ запрещен,
    InternalServer, // Ошибка сервера

    // Пользователи
    IncorrectPassword, // Неверный пароль
    UserExist, // Такой пользователь уже существует
    NotFoundUser, // Пользователь не найден
    NotAuthorization, // Пользователь не авторизован,
    IsNotTeacher, // Пользователь не является учителем
    IsNotStudent, // Пользователь не является учеником
    UserNotExist, // Такого пользователя не существует

    // Группы
    CanNotGetGroups, // Не удается получить группы из базы данных
    CanNotGetGroup, // Не удается получить группу из базы данных
    NotFoundGroup, // Группы не найдена
    CanNotCreateGroup, // Не удалось добавить группу в БД
    CanNotUpdateGroup, // Не удалось обновиь группу
    IncorrectCodeWord, // Неверное кодовое слово,
    IsNotCreator, // Пользователь не является создателем группы
}
