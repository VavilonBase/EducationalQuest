﻿public enum Message
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
}
