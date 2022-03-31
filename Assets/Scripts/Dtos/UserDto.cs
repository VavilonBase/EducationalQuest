public class UserDto
{
	public int userId { get; set; } // Идентификатор пользователя
	public string firstName { get; set; } // Имя пользователя
	public string lastName { get; set; } // Фамилия пользователя
	public string middleName { get; set; } // Отчество пользователя
	public string role { get; set; } // Роль пользователя
	public bool isActivated { get; set; } // Активирован ли пользователь

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
}