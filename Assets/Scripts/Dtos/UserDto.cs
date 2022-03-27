using System.Collections.Generic;

public class UserDto
{
	public int userId; // Идентификатор пользователя
	public string firstName; // Имя пользователя
	public string lastName; // Фамилия пользователя
	public string middleName; // Отчество пользователя
	public string role; // Роль пользователя
	public bool isActivated; // Активирован ли пользователь

	//Конструктор класса
	public UserDto(int _userId, string _firstName, string _lastName,
		string _middleName, string _role, bool _isActivated)
	{
		userId = _userId;
		firstName = _firstName;
		lastName = _lastName;
		middleName = _middleName;
		role = _role;
		isActivated = _isActivated;
	}

	// Вывод объекта класса в строке
	public string printUserDto()
    {
		string isActivateString = isActivated ? "Да" : "Нет";
		return "ID: " + userId +
			"\nИмя: " + firstName +
			"\nФамилия: " + lastName +
			"\nОтчество: " + middleName +
			"\nРоль: " + role +
			"\nПользователь активирован: " + isActivateString;

	}
}