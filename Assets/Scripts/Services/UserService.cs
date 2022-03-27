using System.Collections.Generic;
using UnityEngine;
using Npgsql;

// �����, ����������� ������ � ����� ������ � �������� Users
public static class UserService
{
    //������� ������������
    public static UserDto user = null; // �� ��������� ����� null

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
    public static Response<string> login(string _login, string _password)
    {
        // ���������� ���������� �� ������������ � ����� �������
        bool userIsExist = true;
        // ��������� ���������� �� ������
        bool passwordIsCorrect = true;
        // ����������� � ��
        try
        {
            if (userIsExist)
            {
                if (passwordIsCorrect)
                {
                    user = new User(0, "�����", "��������", "����������", RolesDict[RolesEnum.Admin], true, "admin", "admin");
                    return new Response<string>(false, Message.NotError, null);
                } else
                {
                    return new Response<string>(true, Message.IncorrectPassword, null);
                }
            } else
            {
                return new Response<string>(true, Message.NotFoundUser, null);
            }
        }
        catch
        {
            return new Response<string>(true, Message.InternalServer, null);
        }
        List<User> users = new List<User>()
        {
            new User(0, "�����", "��������", "����������", RolesDict[RolesEnum.Admin], true, "admin", "admin"),
            new User(1, "���������", "������", "���������", RolesDict[RolesEnum.Teacher], true, "teacher", "teacher"),
            new User(3, "��������", "��������", "������������", RolesDict[RolesEnum.Student], false, "student", "student"),
        };
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
    public static Response<string> registration(string _firstName, string _lastName,
        string _middleName, RolesEnum _role, bool _isActivated, string _login, string _password)
    {
        // ��������� �� ���������� �� ������������ � ����� �������
        bool userIsExist = false;
        
        // ����������� � ��
        try
        {
            if (!userIsExist)
            {
                user = new User(0, "�����", "��������", "����������", RolesDict[RolesEnum.Admin], true, "admin", "admin");
                return new Response<string>(false, Message.NotError, null);
            } else
            {
                return new Response<string>(true, Message.UserExist, null);
            }
        } catch
        {
            return new Response<string>(true, Message.InternalServer, null);
        }
    }

    /// <summary>
    /// ���������� ������ � ������� ������������
    /// </summary>
    /// <param name="_firstName">���</param>
    /// <param name="_lastName">�������</param>
    /// <param name="_middleName">��������</param>
    /// <param name="_role">����</param>
    /// <param name="_isActivated">����������� �� ������������</param>
    /// <returns>���������� ����� ���� � null, ���� � �������</returns>
    public static Response<string> updateUser(string _firstName, string _lastName,
        string _middleName, RolesEnum _role, bool _isActivated)
    {
        // ��������� �������� �� ����
        bool userIsLogin = true;
        // ����������� � ��
        try
        {
            if (userIsLogin)
            {
                user = new User(1, "���������", "������", "���������", RolesDict[RolesEnum.Teacher], true, "teacher", "teacher");
                return new Response<string>(false, Message.NotError, null);
            } else
            {
                return new Response<string>(true, Message.NotAuthorization, null);
            }
        } catch
        {
            return new Response<string>(true, Message.InternalServer, null);
        }
    }

    /// <summary>
    /// ����� �� �������
    /// </summary>
    public static void logout()
    {
        user = null;
    }

    /// <summary>
    /// ��������� ���� �������������
    /// </summary>
    /// <returns>���������� ����� �� ������� �������������, ���� � �������</returns>
    public static Response<List<UserDto>> getAllUsers()
    {
        List<User> users = new List<User>()
        {
            new User(0, "�����", "��������", "����������", RolesDict[RolesEnum.Admin], true, "admin", "admin"),
            new User(1, "���������", "������", "���������", RolesDict[RolesEnum.Teacher], true, "teacher", "teacher"),
            new User(3, "��������", "��������", "������������", RolesDict[RolesEnum.Student], false, "student", "student"),
        };
        // ����������� � ��
        try
        {
            //�������� ������ ��� ������ � ������
            List<UserDto> usersDtos = new List<UserDto>();

            foreach (var u in users)
            {
                UserDto userDto = u;
                usersDtos.Add(userDto);
            }

            return new Response<List<UserDto>>(false, Message.NotError, usersDtos);

        }
        catch
        {
            return new Response<List<UserDto>>(true, Message.InternalServer, null);
        }
    }

    /// <summary>
    /// ��������� ������ ������������ �� ID
    /// </summary>
    /// <param name="userId">������������� ������������</param>
    /// <returns>���������� ����� � �������������, ���� � �������</returns>
    public static Response<UserDto> getUserByUserId(int userId)
    {
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
                    return new Response<UserDto>(false, Message.NotError, u);
                }
            }
            return new Response<UserDto>(false, Message.NotFoundUser, null);
        }
        catch
        {
            return new Response<UserDto>(true, Message.InternalServer, null);
        }


    }
    
    /// <summary>
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
                        return new Response<bool>(false, Message.NotError, true);
                    }
                    else
                    {
                        return new Response<bool>(true, Message.IsNotTeacher, false);
                    }
                }
                else
                {
                    return new Response<bool>(true, Message.AccessDenied, false);
                }
            } else
            {
                return new Response<bool>(true, Message.UserNotExist, false);
            }
        } catch
        {
            return new Response<bool>(true, Message.InternalServer, false);
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
    }
}

