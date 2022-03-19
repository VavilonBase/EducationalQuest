// Модель таблицы Users базы данных
public class User
{
	public int id; // Идентификатор пользователя
	public string firstName; // Имя пользователя
	public string lastName; // Фамилия пользователя
	public string middleName; // Отчество пользователя
	public string role; // Роль пользователя
	public bool isActivated; // Активирован ли пользователь
	public string login; // Логин пользователя
	public string password; // Пароль пользователя

	//Конструктор класса
	public User(int _id, string _firstName, string _lastName,
		string _middleName, string _role, bool _isActivated, string _login, string _password)
	{
		id = _id;
		firstName = _firstName;
		lastName = _lastName;
		middleName = _middleName;
		role = _role;
		isActivated = _isActivated;
		login = _login;
		password = _password;
	}
}