using System.Collections.Generic;
using System.Threading.Tasks;

// �����, ����������� ������ � ����� ������ � �������� Users
public static class UserService
{

    //������������ � ������ ������������
    public enum RolesEnum
    {
        Admin = 0,
        Teacher = 1,
        Student = 2
    }

    //������� ��� �������� �� ������������ � ������
    public static Dictionary<RolesEnum, string> RolesDict = new Dictionary<RolesEnum, string>()
    {
        {RolesEnum.Admin, "ADMIN"},
        {RolesEnum.Teacher, "TEACHER"},
        {RolesEnum.Student, "STUDENT"}

    };

    //������ ������ � ����� ������
    /// <summary>
    /// ���� � �������
    /// </summary>
    /// <param name="_login">�����</param>
    /// <param name="_password">������</param>
    /// <returns>���������� ����� ���� � null, ���� � �������</returns>
    public async static Task<Response<ResponseUserData>> login(string _login, string _password)
    {
        // ������ URL
        string url = "https://educationalquest.herokuapp.com/api/users/login.php";
        // �������������� http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // ������� ������
        RequestLoginData requestLoginData = new RequestLoginData() { login = _login, password = _password };
        // ���������� ������ � ���� ������
        var result = await httpClient.Post<ResponseUserData, RequestLoginData>(requestLoginData, url);
        return result;
    }

    /// <summary>
    /// ����������� � �������
    /// </summary>
    /// <param name="_firstName">���</param>
    /// <param name="_lastName">�������</param>
    /// <param name="_middleName">��������</param>
    /// <param name="_role">����</param>
    /// <param name="_isActivated">����������� �� ������������</param>
    /// <param name="_login">�����</param>
    /// <param name="_password">������</param>
    /// <returns>���������� ����� ���� � null, ���� � �������</returns>
    public async static Task<Response<ResponseUserData>> registration(string _firstName, string _lastName,
        RolesEnum _role, bool _isActivated, string _login, string _password, string _middleName = "")
    {
        // ������ URL
        string url = "https://educationalquest.herokuapp.com/api/users/registration.php";
        // �������������� ����������
        var httpClient = new HttpClient(new JsonSerializationOption());
        // �������������� ������
        RequestRegisrationData requestRegisrationData = new RequestRegisrationData()
        {
            lastName = _lastName,
            firstName = _firstName,
            middleName = _middleName,
            role = RolesDict[_role],
            login = _login,
            password = _password
        };
        // ���������� ������ � ���� �����
        var result = await httpClient.Post<ResponseUserData, RequestRegisrationData>(requestRegisrationData, url);
        return result;
    }
    /// <summary>
    /// ���������� ������ ������������
    /// </summary>
    /// <param name="jwt">�����</param>
    /// <returns>��� ������ � ������������ � ����� �����</returns>
    public async static Task<Response<ResponseUserData>> refresh(string jwt)
    {
        // ������ URL
        string url = "https://educationalquest.herokuapp.com/api/users/refresh_token.php";
        // �������������� ����������
        var httpClient = new HttpClient(new JsonSerializationOption());
        // �������������� ������ (������������� ���������)
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        var result = await httpClient.Get<ResponseUserData>(url);
        return result;
    }
/// <summary>
/// ���������� ������ � ������� ������������
/// </summary>
/// <param name="_firstName">���</param>
/// <param name="_lastName">�������</param>
/// <param name="_middleName">��������</param>
/// <param name="_role">����</param>
/// <param name="_jwt">�����</param>
/// <returns>���������� ����� ���� � null, ���� � �������</returns>
public async static Task<Response<ResponseUserData>> updateUser(string _firstName, string _lastName,
    string _middleName, RolesEnum _role, string jwt)
{
        // ������ URL
        string url = "https://educationalquest.herokuapp.com/api/users/update_user.php";
        // �������������� ����������
        var httpClient = new HttpClient(new JsonSerializationOption());
        // ������������� ���������
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}" }
        };
        // �������������� ������
        RequestUpdateUserData requestUpdateUserData = new RequestUpdateUserData()
        {
            lastName = _firstName,
            firstName = _lastName,
            middleName = _middleName,
            role = RolesDict[_role],
        };

        var result = await httpClient.Post<ResponseUserData, RequestUpdateUserData>(requestUpdateUserData, url);
        return result;
    }



    /// <summary>
    /// ��������� ���� �������������
    /// </summary>
    /// <returns>���������� ����� �� ������� �������������, ���� � �������</returns>
    public async static Task<Response<List<User>>> getAllUsers(string jwt)
    {
        // ������ URL
        string url = "https://educationalquest.herokuapp.com/api/users/get_all_users.php";
        // �������������� ����������
        var httpClient = new HttpClient(new JsonSerializationOption());
        // ������������� ���������
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}" }
        };

        var result = await httpClient.Get<List<User>>(url);
        return result;
    }

    /// <summary>
    /// ��������� �������
    /// </summary>
    /// <param name="teacherId">������������� �������</param>
    /// <param name="jwt">�����</param>
    /// <returns>���������� �������, ���� �� ��� �����������, ����� ������</returns>
    public async static Task<Response<ResponseUser>> activateTeacher(int teacherId, string jwt)
    {
        // ������ URL
        string url = "https://educationalquest.herokuapp.com/api/users/activate_teacher.php?teacherId=" + teacherId.ToString();

        // �������������� ����������
        var httpClient = new HttpClient(new JsonSerializationOption());
        // ������������� ���������
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}" }
        };

        var result = await httpClient.Get<ResponseUser>(url);
        return result;
    }

    /// <summary>
    /// ��������� ������
    /// </summary>
    /// <param name="_lastPassword">������ ������</param>
    /// <param name="_newPassword">����� ������</param>
    /// <param name="jwt">�����</param>
    /// <returns>���������� ������������, ���� ������ ��� �������, ����� ������</returns>
    public async static Task<Response<ResponseUser>> changePassword(string _lastPassword, string _newPassword, string jwt)
    {
        // ������ URL
        string url = "https://educationalquest.herokuapp.com/api/users/change_password.php";

        // �������������� ����������
        var httpClient = new HttpClient(new JsonSerializationOption());
        // ������������� ���������
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}" }
        };

        // �������������� ������
        RequestChangePasswordData requestChangePasswordData = new RequestChangePasswordData()
        {
            lastPassword = _lastPassword,
            password = _newPassword
        };
        var result = await httpClient.Post<ResponseUser, RequestChangePasswordData>(requestChangePasswordData, url);
        return result;
    }

    /// <summary>
    /// ��������� ������ ������������ �� ID
    /// </summary>
    /// <param name="userId">������������� ������������</param>
    /// <returns>���������� ����� � �������������, ���� � �������</returns>
    /* public static Response<UserDto> getUserByUserId(int userId)*/
    /*{
        List<User> users = new List<User>()
        {
            new User(0, "�����", "��������", "����������", RolesDict[RolesEnum.Admin], true, "admin", "admin"),
            new User(1, "���������", "������", "���������", RolesDict[RolesEnum.Teacher], true, "teacher", "teacher"),
            new User(3, "��������", "��������", "������������", RolesDict[RolesEnum.Student], false, "student", "student"),
        };
        // ����� ������������

        try
        {
            foreach (var u in users)
            {
                if (u.userId == userId)
                {
                    return new Response<UserDto>(false, message.NotError, u);
                }
            }
            return new Response<UserDto>(false, message.NotFoundUser, null);
        }
        catch
        {
            return new Response<UserDto>(true, message.InternalServer, null);
        }


    }*/

    /*  /// <summary>
      /// ��������� �������
      /// </summary>
      /// <param name="_id">������������� �������</param>
      /// <returns>���������� ����� � true, ���� ��������� ������ �������, ����� false</returns>
      public static Response<bool> activateTeacher(int _id)
      {
          // ��������� ���������� �� ������������
          bool userExist = true;
          // ��������� ���� �� ������
          bool isAccess = true;
          // �������� �������� �� ������������ ��������
          bool isTeacher = true;
          User teacher = new User(1, "���������", "������", "���������", RolesDict[RolesEnum.Teacher], false, "teacher", "teacher");

          // ����������� � ��
          try
          {
              if (userExist)
              {
                  if (isAccess)
                  {
                      if (isTeacher)
                      {
                          teacher.isActivated = true;
                          return new Response<bool>(false, message.NotError, true);
                      }
                      else
                      {
                          return new Response<bool>(true, message.IsNotTeacher, false);
                      }
                  }
                  else
                  {
                      return new Response<bool>(true, message.AccessDenied, false);
                  }
              }
              else
              {
                  return new Response<bool>(true, message.UserNotExist, false);
              }
          }
          catch
          {
              return new Response<bool>(true, message.InternalServer, false);
          }
      }

      /// <summary>
      /// �������� ������������ �� ������
      /// </summary>
      /// <param name="roles">��������� ���� ������������</param>
      /// <returns>true - ���� ������ ����, ����� false</returns>
      public static bool checkAccess(RolesEnum[] roles)
      {
          //���������� �� ���� ����������� �����
          foreach (var r in roles)
          {
              if (RolesDict[r] == user.role)
              {
                  return true;
              }
          }
          return false;
      }*/
}