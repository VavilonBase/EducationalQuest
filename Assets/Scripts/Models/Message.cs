public enum Message
{
    NotError, // Нет ошибки
    IncorrectPassword, // Неверный пароль
    NotFoundUser, // Пользователь не найден
    CanNotGetUsers, // Не удается получить пользователей из базы данных
    CanNotRegistrationUser, // Не удалось добавить пользователя в базу данных
    AccessDenied, // Доступ запрещен,
    CanNotActivateUser, // Не получилось активировать пользователя,
    NotAuthorization, // Пользователь не авторизован
}
