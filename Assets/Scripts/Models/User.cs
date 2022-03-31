// Модель таблицы Users базы данных
using System.Collections.Generic;

public class User : UserDto
{
    public string login { get; set; } // Логин пользователя
    public string password { get; set; } // Пароль пользователя
                                      //Конструктор класса
    public User(int _userId, string _firstName, string _lastName,
        string _middleName, string _role, bool _isActivated, string _login, string _password)
        : base(_userId, _firstName, _lastName, _middleName, _role, _isActivated)
    {
        login = _login;
        password = _password;
    }

}