// ������ ������� Users ���� ������
public class User
{
	public int id; // ������������� ������������
	public string firstName; // ��� ������������
	public string lastName; // ������� ������������
	public string middleName; // �������� ������������
	public string role; // ���� ������������
	public bool isActivated; // ����������� �� ������������
	public string login; // ����� ������������
	public string password; // ������ ������������

	//����������� ������
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