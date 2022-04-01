// Модель таблицы Users базы данных
using System.Collections.Generic;

public class User : UserDto
{
    public string login { get; set; } // Логин пользователя
    public string password { get; set; } // Пароль пользователя
                                      //Конструктор класса
}