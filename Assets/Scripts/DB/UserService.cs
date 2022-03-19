using System.Collections.Generic;
using UnityEngine;

// �����, ����������� ������ � ����� ������ � �������� Users
public static class UserService
{
    //������� ������������
    public static User user = null; // �� ��������� ����� null

    //������������ � ������ ������������
    public enum RolesEnum
    {
        Admin = 0,
        Teacher = 1,
        Student = 2
    }

    //������� ��� �������� �� ������������ � ������
    private static Dictionary<RolesEnum, string> RolesDict = new Dictionary<RolesEnum, string>()
    {
        {RolesEnum.Admin, "ADMIN"},
        {RolesEnum.Teacher, "TEACHER"},
        {RolesEnum.Student, "STUDENT"}

    };

    // ��������� ���� ������ �������������
    private static List<User> users = new List<User>();

    //��������� ���� �� ������ id
    private static int id = 3;

    // ��������� ������ � ��. �������������� ��������� ��������� ��
    public static void initUsers()
    {
        //��������� ������ � ����� ������
        //������� ������� ������������ � ������� ������
        User admin = new User(0, "�����", "��������", "����������", RolesDict[RolesEnum.Admin], true, "admin", "admin");
        User teacher = new User(1, "���������", "������", "���������", RolesDict[RolesEnum.Teacher], true, "teacher", "teacher");
        User student = new User(2, "��������", "��������", "������������", RolesDict[RolesEnum.Student], false, "student", "student");
        //��������� �� � ����
        users.Add(admin);
        users.Add(teacher);
        users.Add(student);
    }

    //������ ������ � ����� ������
    //���� � �������
    public static Response<string> login(string _login, string _password)
    {
        // ���������� ���������� �� ������������ � ����� ������� � �������
        try
        {
            foreach (var u in users)
            {
                if (u.login == _login)
                {
                    if (u.password == _password)
                    {
                        //������� �������� ������������
                        user = u;
                        return new Response<string>(false, Message.NotError, null);
                    }
                    return new Response<string>(true, Message.IncorrectPassword, null);
                }
            }
            return new Response<string>(true, Message.NotFoundUser, null);
        } catch
        {
            return new Response<string>(true, Message.CanNotGetUsers, null);
        }

    }

    //����������� � �������
    public static Response<string> registration(string _firstName, string _lastName,
        string _middleName, RolesEnum _role, bool _isActivated, string _login, string _password)
    {
        //������� ������������
        User newUser = new User(id, _firstName, _lastName, _middleName, RolesDict[_role], _isActivated, _login, _password);
        // ��������� ������������ � ��
        try
        {
            users.Add(newUser);
            //����������� id
            id++;
            // ������ ������ ������������ - �������
            user = newUser;
            return new Response<string>(false, Message.NotError, null);
        }
        catch
        {
            return new Response<string>(true, Message.CanNotRegistrationUser, null);
        }
    }

    //���������� ������ � ������������
    public static Response<string> updateUser(string _firstName, string _lastName,
        string _middleName, RolesEnum _role, bool _isActivated)
    {
        // ��������� �������� �� ����
        if (user != null)
        {
            //������� ������������
            User updatedUser = new User(user.id, _firstName, _lastName, _middleName, RolesDict[_role], _isActivated, user.login, user.password);
            // ��������� ������������ � ��
            try
            {
                // ���� ������������ � �������� id
                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].id == user.id)
                    {
                        //��������� ������������ � ��
                        users[i] = updatedUser;
                        //��������� ������������ ��������
                        user = updatedUser;
                        return new Response<string>(false, Message.NotError, null);
                    }
                }
                return new Response<string>(true, Message.NotFoundUser, null);
            }
            catch
            {
                return new Response<string>(true, Message.CanNotRegistrationUser, null);
            }
        }
        // ���� ������������ �� ����� � �������
        return new Response<string>(true, Message.NotAuthorization, null);
    }

    //����� �� �������
    public static void logout()
    {
        user = null;
    }

    //��������� ���� �������������
    public static Response<List<User>> getAllUsers()
    {
        // ����������� ����
        RolesEnum[] accessUsers = new RolesEnum[] { RolesEnum.Admin };

        // �������� �� ������
        if (checkAccess(accessUsers))
        {
            // ��������� ���� �������������
            try
            {
                return new Response<List<User>>(false, Message.NotError, users);
            }
            catch
            {
                return new Response<List<User>>(true, Message.CanNotGetUsers, null);
            }
        }
        return new Response<List<User>>(true, Message.AccessDenied, null);

    }

    // ��������� �������
    public static Response<bool> activateTeacher(int _id)
    {
        //����������� ����
        RolesEnum[] accessUses = new RolesEnum[] { RolesEnum.Admin };

        // �������� �� ������
        if (checkAccess(accessUses))
        {
            //��������� ������������ � �������� id
            try
            {
                foreach (var u in users)
                {
                    //���� ������� � �������� id
                    if (u.id == _id && u.role == RolesDict[RolesEnum.Teacher])
                    {
                        // ���������� ������ � ���� ������
                        try
                        {
                            u.isActivated = true;
                            return new Response<bool>(true, Message.NotError, true);
                        } catch
                        {
                            return new Response<bool>(true, Message.CanNotActivateUser, false);
                        }
                    }
                }
                //���� ������������ �� ������
                return new Response<bool>(true, Message.NotFoundUser, false);
            }
            catch
            {
                //���� �������� ������ ��� ������ ������������
                return new Response<bool>(true, Message.NotFoundUser, false);
            }
        }
        //���� ��� �������
        return new Response<bool>(true, Message.AccessDenied, false);

    }

    //���������� ������� � ������ (���� ������� ������ ���������)
    public static Response<bool> studentJoinGroup()
    {
        //����������� ����
        RolesEnum[] accessUses = new RolesEnum[] { RolesEnum.Student };

        // �������� �� ������
        if (checkAccess(accessUses))
        {
            //��������� ������������ � �������� id
            try
            {
                foreach (var u in users)
                {
                    //���� ������� � �������� id
                    if (u.id == user.id)
                    {
                        // ���������� ������ � ���� ������
                        try
                        {
                            u.isActivated = true;
                            return new Response<bool>(true, Message.NotError, true);
                        }
                        catch
                        {
                            return new Response<bool>(true, Message.CanNotActivateUser, false);
                        }
                    }
                }
                //���� ������������ �� ������
                return new Response<bool>(true, Message.NotFoundUser, false);
            }
            catch
            {
                //���� �������� ������ ��� ������ ������������
                return new Response<bool>(true, Message.NotFoundUser, false);
            }
        }
        //���� ��� �������
        return new Response<bool>(true, Message.AccessDenied, false);
    }

    //�������� �� ������
    private static bool checkAccess(RolesEnum[] roles)
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

