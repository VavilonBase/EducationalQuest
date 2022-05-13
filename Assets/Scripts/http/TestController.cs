using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TestController : MonoBehaviour
{
    [SerializeField] private Image texture;
    [ContextMenu("Test Get")]
    public async void TestGet()
    {
        string url = "https://jsonplaceholder.typicode.com/todos/1";

        var httpClient = new HttpClient(new JsonSerializationOption());

        var result = await httpClient.Get<User>(url);
    }

    [ContextMenu("login")]
    public async void TestLogin()
    {
        string url = "https://educationalquest.herokuapp.com/api/users/login.php";
        var response = await UserService.login("vav", "123");

        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Debug.Log(response.data.user);
        }

    }

    [ContextMenu("registration")]
    public async void TestRegistration()
    {
        string url = "https://educationalquest.herokuapp.com/api/users/registration.php";
        var httpClient = new HttpClient(new JsonSerializationOption());
        RequestRegisrationData requestRegisrationData = new RequestRegisrationData() {
            lastName = "Ельденев",
            firstName = "Artem",
            middleName = "Tavroso",
            role = UserService.RolesDict[UserService.RolesEnum.Student],
            login = "vav11",
            password = "123"
        };

        var result = await httpClient.Post<ResponseUserData, RequestRegisrationData>(requestRegisrationData, url);

        Debug.Log($"{result.data.user.firstName}");
    }

    [ContextMenu("refresh")]
    public async void TestRefresh()
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/api/users/refresh_token.php";
        // Инициализируем соединение
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Подготавливаем данные (устанавливаем заголовки)
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hbnktc2l0ZS5vcmciLCJhdWQiOiJodHRwOlwvXC9hbnktc2l0ZS5jb20iLCJleHAiOjE2NDg3MzM1MDYsImlhdCI6MTM1Njk5OTUyNCwibmJmIjoxMzU3MDAwMDAwLCJkYXRhIjp7ImlkIjozLCJmaXJzdE5hbWUiOiJcdTA0MTBcdTA0NDBcdTA0NDJcdTA0MzVcdTA0M2MiLCJsYXN0TmFtZSI6Ilx1MDQxNVx1MDQzYlx1MDQ0Y1x1MDQzNFx1MDQzNVx1MDQzZFx1MDQzNVx1MDQzMiIsImxvZ2luIjoidmF2Iiwicm9sZSI6IkFETUlOIiwiaXNBY3RpdmF0ZWQiOnRydWV9fQ.neSwgG-b849A8ytysgJL82RBLV0qDi_zINX9rHLpW4M" }
        };
        var result = await httpClient.Get<ResponseUserData>(url);


        Debug.Log($"{result.data.user.firstName}");
    }

    [ContextMenu("update user")]
    public async void TestUpdateUser()
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/api/users/update_user.php";
        // Инициализируем соединение
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hbnktc2l0ZS5vcmciLCJhdWQiOiJodHRwOlwvXC9hbnktc2l0ZS5jb20iLCJleHAiOjE2NDg3MzM1MDYsImlhdCI6MTM1Njk5OTUyNCwibmJmIjoxMzU3MDAwMDAwLCJkYXRhIjp7ImlkIjozLCJmaXJzdE5hbWUiOiJcdTA0MTBcdTA0NDBcdTA0NDJcdTA0MzVcdTA0M2MiLCJsYXN0TmFtZSI6Ilx1MDQxNVx1MDQzYlx1MDQ0Y1x1MDQzNFx1MDQzNVx1MDQzZFx1MDQzNVx1MDQzMiIsImxvZ2luIjoidmF2Iiwicm9sZSI6IkFETUlOIiwiaXNBY3RpdmF0ZWQiOnRydWV9fQ.neSwgG-b849A8ytysgJL82RBLV0qDi_zINX9rHLpW4M" }
        };
        // Подготавливаем данные
        RequestUpdateUserData requestUpdateUserData = new RequestUpdateUserData()
        {
            lastName = "Ельденев123123",
            firstName = "Artem",
            middleName = "Tavroso",
            role = UserService.RolesDict[UserService.RolesEnum.Admin]
        };

        var result = await httpClient.Post<ResponseUserData, RequestUpdateUserData>(requestUpdateUserData, url);

        Debug.Log($"{result.data.user.lastName}");
    }

    [ContextMenu("get all users")]
    public async void TestGetAllUsers()
    {
        var response = await UserService.getAllUsers("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hbnktc2l0ZS5vcmciLCJhdWQiOiJodHRwOlwvXC9hbnktc2l0ZS5jb20iLCJleHAiOjE2NDg4NjIwNzgsImlhdCI6MTM1Njk5OTUyNCwibmJmIjoxMzU3MDAwMDAwLCJkYXRhIjp7ImlkIjozLCJmaXJzdE5hbWUiOiJBcnRlbSIsImxhc3ROYW1lIjoiXHUwNDE1XHUwNDNiXHUwNDRjXHUwNDM0XHUwNDM1XHUwNDNkXHUwNDM1XHUwNDMyMTIzMTIzIiwibG9naW4iOiJ2YXYiLCJyb2xlIjoiQURNSU4iLCJpc0FjdGl2YXRlZCI6dHJ1ZX19.43jR7ml_TGR4-K3-giIjQte43IgwVvUq5dM9XYFB-IA");

        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Debug.Log(response.data[0].lastName);
        }
    }

    [ContextMenu("activate teacher")]
    public async void TestActvateTeacher()
    {
        var response = await UserService.activateTeacher(38, "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hbnktc2l0ZS5vcmciLCJhdWQiOiJodHRwOlwvXC9hbnktc2l0ZS5jb20iLCJleHAiOjE2NDkwODU0NDYsImlhdCI6MTM1Njk5OTUyNCwibmJmIjoxMzU3MDAwMDAwLCJkYXRhIjp7ImlkIjo2MSwiZmlyc3ROYW1lIjoiXHUwNDEwXHUwNDQwXHUwNDQyXHUwNDM1XHUwNDNjIiwibGFzdE5hbWUiOiJcdTA0MTVcdTA0M2JcdTA0NGNcdTA0MzRcdTA0MzVcdTA0M2RcdTA0MzVcdTA0MzIiLCJsb2dpbiI6InZhdiIsInJvbGUiOiJBRE1JTiIsImlzQWN0aXZhdGVkIjp0cnVlfX0.sPFQFO0svFpA0Ddet6AMBewOarRnqJHOH7vTVhG-Ya4");
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Debug.Log(response.data.isActivated);
            Debug.Log(response.data.lastName);
        }
    }

    [ContextMenu("change password")]
    public async void TestChangePassword()
    {
        var response = await UserService.changePassword("123", "1234", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hbnktc2l0ZS5vcmciLCJhdWQiOiJodHRwOlwvXC9hbnktc2l0ZS5jb20iLCJleHAiOjE2NDkwODU0NDYsImlhdCI6MTM1Njk5OTUyNCwibmJmIjoxMzU3MDAwMDAwLCJkYXRhIjp7ImlkIjo2MSwiZmlyc3ROYW1lIjoiXHUwNDEwXHUwNDQwXHUwNDQyXHUwNDM1XHUwNDNjIiwibGFzdE5hbWUiOiJcdTA0MTVcdTA0M2JcdTA0NGNcdTA0MzRcdTA0MzVcdTA0M2RcdTA0MzVcdTA0MzIiLCJsb2dpbiI6InZhdiIsInJvbGUiOiJBRE1JTiIsImlzQWN0aXZhdGVkIjp0cnVlfX0.sPFQFO0svFpA0Ddet6AMBewOarRnqJHOH7vTVhG-Ya4");
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Debug.Log(response.data.isActivated);
            Debug.Log(response.data.lastName);
        }
    }

    [ContextMenu("create group")]
    public async void TestCreateGroup()
    {
        var response = await GroupService.createGroup("НГ2022", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hbnktc2l0ZS5vcmciLCJhdWQiOiJodHRwOlwvXC9hbnktc2l0ZS5jb20iLCJleHAiOjE2NDkxOTU1NTMsImlhdCI6MTM1Njk5OTUyNCwibmJmIjoxMzU3MDAwMDAwLCJkYXRhIjp7ImlkIjo0NSwiZmlyc3ROYW1lIjoiXHUwNDEwXHUwNDNiXHUwNDM1XHUwNDNhXHUwNDQxXHUwNDM1XHUwNDM5IiwibGFzdE5hbWUiOiJcdTA0MWNcdTA0MzBcdTA0NDBcdTA0M2FcdTA0MzhcdTA0M2QiLCJsb2dpbiI6ImFsZXgxMjMiLCJyb2xlIjoiVEVBQ0hFUiIsImlzQWN0aXZhdGVkIjp0cnVlfX0.xOxuM8p3YAYxf44mLfxuWq8gHU7MagGENfWlH49qcwk");
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Debug.Log(response.data.title);
            Debug.Log(response.data.codeWord);
        }
    }

    [ContextMenu("get all groups teacher")]
    public async void TestGetAllTeacherGroups()
    {
        var response = await GroupService.getAllTeacherGroups("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hbnktc2l0ZS5vcmciLCJhdWQiOiJodHRwOlwvXC9hbnktc2l0ZS5jb20iLCJleHAiOjE2NDkxOTkwMDYsImlhdCI6MTM1Njk5OTUyNCwibmJmIjoxMzU3MDAwMDAwLCJkYXRhIjp7ImlkIjo0NSwiZmlyc3ROYW1lIjoiXHUwNDEwXHUwNDNiXHUwNDM1XHUwNDNhXHUwNDQxXHUwNDM1XHUwNDM5IiwibGFzdE5hbWUiOiJcdTA0MWNcdTA0MzBcdTA0NDBcdTA0M2FcdTA0MzhcdTA0M2QiLCJsb2dpbiI6ImFsZXgxMjMiLCJyb2xlIjoiVEVBQ0hFUiIsImlzQWN0aXZhdGVkIjp0cnVlfX0.vTPKdHCf6j8kI3iO2fZDhNWO88wGHrp-H8Z1al8aGZA");
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            foreach (var group in response.data)
            {
                Debug.Log(group.title);
                Debug.Log(group.codeWord);
            }
        }
    }

    [ContextMenu("Join student to the group")]
    public async void TestJoinStudentToTheGroup()
    {
        var response = await GroupService.joinStudentToTheGroup("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hbnktc2l0ZS5vcmciLCJhdWQiOiJodHRwOlwvXC9hbnktc2l0ZS5jb20iLCJleHAiOjE2NDkxOTk1MzYsImlhdCI6MTM1Njk5OTUyNCwibmJmIjoxMzU3MDAwMDAwLCJkYXRhIjp7ImlkIjo2NywiZmlyc3ROYW1lIjoiXHUwNDEwXHUwNDQwXHUwNDQyXHUwNDM1XHUwNDNjIiwibGFzdE5hbWUiOiJcdTA0MTVcdTA0M2JcdTA0NGNcdTA0MzRcdTA0MzVcdTA0M2RcdTA0MzVcdTA0MzIiLCJsb2dpbiI6InMyIiwicm9sZSI6IlNUVURFTlQiLCJpc0FjdGl2YXRlZCI6dHJ1ZX19.Pybh2x2v6cEyX7mKt8k0JM2ktJ7ZKecSzufgKei7m3o",
            "80f19ac9-ee5f-40b6-b3fb-ebc315818e0b");
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
                Debug.Log("Вступил");
        }
    }

    [ContextMenu("Get student groups")]
    public async void TestGetStudentGroups()
    {
        var response = await GroupService.getStudentGroups("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hbnktc2l0ZS5vcmciLCJhdWQiOiJodHRwOlwvXC9hbnktc2l0ZS5jb20iLCJleHAiOjE2NDkxOTk1MzYsImlhdCI6MTM1Njk5OTUyNCwibmJmIjoxMzU3MDAwMDAwLCJkYXRhIjp7ImlkIjo2NywiZmlyc3ROYW1lIjoiXHUwNDEwXHUwNDQwXHUwNDQyXHUwNDM1XHUwNDNjIiwibGFzdE5hbWUiOiJcdTA0MTVcdTA0M2JcdTA0NGNcdTA0MzRcdTA0MzVcdTA0M2RcdTA0MzVcdTA0MzIiLCJsb2dpbiI6InMyIiwicm9sZSI6IlNUVURFTlQiLCJpc0FjdGl2YXRlZCI6dHJ1ZX19.Pybh2x2v6cEyX7mKt8k0JM2ktJ7ZKecSzufgKei7m3o");
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            foreach (var group in response.data)
            {
                Debug.Log(group.title);
            }
        }
    }

    [ContextMenu("Update group")]
    public async void TestUpdateGroup()
    {
        var response = await GroupService.updateGroup("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hbnktc2l0ZS5vcmciLCJhdWQiOiJodHRwOlwvXC9hbnktc2l0ZS5jb20iLCJleHAiOjE2NDkxOTk4MTAsImlhdCI6MTM1Njk5OTUyNCwibmJmIjoxMzU3MDAwMDAwLCJkYXRhIjp7ImlkIjo0NSwiZmlyc3ROYW1lIjoiXHUwNDEwXHUwNDNiXHUwNDM1XHUwNDNhXHUwNDQxXHUwNDM1XHUwNDM5IiwibGFzdE5hbWUiOiJcdTA0MWNcdTA0MzBcdTA0NDBcdTA0M2FcdTA0MzhcdTA0M2QiLCJsb2dpbiI6ImFsZXgxMjMiLCJyb2xlIjoiVEVBQ0hFUiIsImlzQWN0aXZhdGVkIjp0cnVlfX0._VCID1vHRWL2JlzOz1s_ZALBvndzAXzWg0Bm2LcnSTI",
            "sdfsdfsd", 1);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Debug.Log(response.data.title);
            Debug.Log(response.data.codeWord);
        }
    }

    [ContextMenu("Delete group")]
    public async void TestDeleteGroup()
    {
        var response = await GroupService.deleteGroup("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOi8vYW55LXNpdGUub3JnIiwiYXVkIjoiaHR0cDovL2FueS1zaXRlLmNvbSIsImV4cCI6MTY1MDYzODIxOCwiaWF0IjoxMzU2OTk5NTI0LCJuYmYiOjEzNTcwMDAwMDAsImRhdGEiOnsiaWQiOjg1LCJmaXJzdE5hbWUiOiJcdTA0MTBcdTA0NDBcdTA0NDJcdTA0MzVcdTA0M2MiLCJsYXN0TmFtZSI6Ilx1MDQxNVx1MDQzYlx1MDQ0Y1x1MDQzNFx1MDQzNVx1MDQzZFx1MDQzNVx1MDQzMiIsImxvZ2luIjoic2RzZCIsInJvbGUiOiJURUFDSEVSIiwiaXNBY3RpdmF0ZWQiOnRydWV9fQ.mTpkXSZppki0A_0GjvTOffS7qRT1EC7bx-MU_mLXwk8",
            1);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Debug.Log("Группа удалена");
        }
    }

    [ContextMenu("Create Test")]
    public async void TestCreateTest()
    {
        var response = await TestService.create("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOi8vYW55LXNpdGUub3JnIiwiYXVkIjoiaHR0cDovL2FueS1zaXRlLmNvbSIsImV4cCI6MTY0OTYzMTI1MSwiaWF0IjoxMzU2OTk5NTI0LCJuYmYiOjEzNTcwMDAwMDAsImRhdGEiOnsiaWQiOjQ1LCJmaXJzdE5hbWUiOiJcdTA0MTBcdTA0M2JcdTA0MzVcdTA0M2FcdTA0NDFcdTA0MzVcdTA0MzkiLCJsYXN0TmFtZSI6Ilx1MDQxY1x1MDQzMFx1MDQ0MFx1MDQzYVx1MDQzOFx1MDQzZCIsImxvZ2luIjoiYWxleDEyMyIsInJvbGUiOiJURUFDSEVSIiwiaXNBY3RpdmF0ZWQiOnRydWV9fQ.TcGP50p_m_OAmZxizl26RJy7fdMgxP2mdJMRE_C4ZkY",
            7, "Тест на создание группы", true);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Debug.Log($"Для группы с ID {response.data.groupId}\n" +
                $"был создан тест с ID {response.data.testId}\n" +
                $"и названием {response.data.title}");
        }
    }

    [ContextMenu("Update Test")]
    public async void TestUpdateTest()
    {
        var response = await TestService.update("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOi8vYW55LXNpdGUub3JnIiwiYXVkIjoiaHR0cDovL2FueS1zaXRlLmNvbSIsImV4cCI6MTY0OTYzMTI1MSwiaWF0IjoxMzU2OTk5NTI0LCJuYmYiOjEzNTcwMDAwMDAsImRhdGEiOnsiaWQiOjQ1LCJmaXJzdE5hbWUiOiJcdTA0MTBcdTA0M2JcdTA0MzVcdTA0M2FcdTA0NDFcdTA0MzVcdTA0MzkiLCJsYXN0TmFtZSI6Ilx1MDQxY1x1MDQzMFx1MDQ0MFx1MDQzYVx1MDQzOFx1MDQzZCIsImxvZ2luIjoiYWxleDEyMyIsInJvbGUiOiJURUFDSEVSIiwiaXNBY3RpdmF0ZWQiOnRydWV9fQ.TcGP50p_m_OAmZxizl26RJy7fdMgxP2mdJMRE_C4ZkY",
            6, "Тест на создание группы измененный", false);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Debug.Log($"Для группы с ID {response.data.groupId}\n" +
                $"был обновлен тест с ID {response.data.testId}\n" +
                $"с названием {response.data.title}");
        }
    }

    [ContextMenu("Delete Test")]
    public async void TestDeleteTest()
    {
        var response = await TestService.delete("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOi8vYW55LXNpdGUub3JnIiwiYXVkIjoiaHR0cDovL2FueS1zaXRlLmNvbSIsImV4cCI6MTY0OTYzMTI1MSwiaWF0IjoxMzU2OTk5NTI0LCJuYmYiOjEzNTcwMDAwMDAsImRhdGEiOnsiaWQiOjQ1LCJmaXJzdE5hbWUiOiJcdTA0MTBcdTA0M2JcdTA0MzVcdTA0M2FcdTA0NDFcdTA0MzVcdTA0MzkiLCJsYXN0TmFtZSI6Ilx1MDQxY1x1MDQzMFx1MDQ0MFx1MDQzYVx1MDQzOFx1MDQzZCIsImxvZ2luIjoiYWxleDEyMyIsInJvbGUiOiJURUFDSEVSIiwiaXNBY3RpdmF0ZWQiOnRydWV9fQ.TcGP50p_m_OAmZxizl26RJy7fdMgxP2mdJMRE_C4ZkY",
            6);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Debug.Log("Тест удален");
        }
    }

    [ContextMenu("Get All Group Tests")]
    public async void TestGetAllGroupTest()
    {
        var response = await TestService.getAllGroupTests("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOi8vYW55LXNpdGUub3JnIiwiYXVkIjoiaHR0cDovL2FueS1zaXRlLmNvbSIsImV4cCI6MTY0OTYzMTI1MSwiaWF0IjoxMzU2OTk5NTI0LCJuYmYiOjEzNTcwMDAwMDAsImRhdGEiOnsiaWQiOjQ1LCJmaXJzdE5hbWUiOiJcdTA0MTBcdTA0M2JcdTA0MzVcdTA0M2FcdTA0NDFcdTA0MzVcdTA0MzkiLCJsYXN0TmFtZSI6Ilx1MDQxY1x1MDQzMFx1MDQ0MFx1MDQzYVx1MDQzOFx1MDQzZCIsImxvZ2luIjoiYWxleDEyMyIsInJvbGUiOiJURUFDSEVSIiwiaXNBY3RpdmF0ZWQiOnRydWV9fQ.TcGP50p_m_OAmZxizl26RJy7fdMgxP2mdJMRE_C4ZkY",
            7);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            foreach(var test in response.data)
            {
                Debug.Log($"Для группы с ID {test.groupId}\n" +
                $"был получен тест с ID {test.testId}\n" +
                $"с названием {test.title}");
            }
        }
    }

    [ContextMenu("Open Test")]
    public async void TestOpenTest()
    {
        var response = await TestService.openTest("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOi8vYW55LXNpdGUub3JnIiwiYXVkIjoiaHR0cDovL2FueS1zaXRlLmNvbSIsImV4cCI6MTY0OTYzMTI1MSwiaWF0IjoxMzU2OTk5NTI0LCJuYmYiOjEzNTcwMDAwMDAsImRhdGEiOnsiaWQiOjQ1LCJmaXJzdE5hbWUiOiJcdTA0MTBcdTA0M2JcdTA0MzVcdTA0M2FcdTA0NDFcdTA0MzVcdTA0MzkiLCJsYXN0TmFtZSI6Ilx1MDQxY1x1MDQzMFx1MDQ0MFx1MDQzYVx1MDQzOFx1MDQzZCIsImxvZ2luIjoiYWxleDEyMyIsInJvbGUiOiJURUFDSEVSIiwiaXNBY3RpdmF0ZWQiOnRydWV9fQ.TcGP50p_m_OAmZxizl26RJy7fdMgxP2mdJMRE_C4ZkY",
            3);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {

            Debug.Log($"Для группы с ID {response.data.groupId}\n" +
            $"был открыт тест с ID {response.data.testId}\n" +
            $"с названием {response.data.title}");

        }
    }

    [ContextMenu("Close Test")]
    public async void TestCloseTest()
    {
        var response = await TestService.closeTest("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOi8vYW55LXNpdGUub3JnIiwiYXVkIjoiaHR0cDovL2FueS1zaXRlLmNvbSIsImV4cCI6MTY0OTYzMTI1MSwiaWF0IjoxMzU2OTk5NTI0LCJuYmYiOjEzNTcwMDAwMDAsImRhdGEiOnsiaWQiOjQ1LCJmaXJzdE5hbWUiOiJcdTA0MTBcdTA0M2JcdTA0MzVcdTA0M2FcdTA0NDFcdTA0MzVcdTA0MzkiLCJsYXN0TmFtZSI6Ilx1MDQxY1x1MDQzMFx1MDQ0MFx1MDQzYVx1MDQzOFx1MDQzZCIsImxvZ2luIjoiYWxleDEyMyIsInJvbGUiOiJURUFDSEVSIiwiaXNBY3RpdmF0ZWQiOnRydWV9fQ.TcGP50p_m_OAmZxizl26RJy7fdMgxP2mdJMRE_C4ZkY",
            3);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Debug.Log($"Для группы с ID {response.data.groupId}\n" +
                $"был закрыт тест с ID {response.data.testId}\n" +
                $"с названием {response.data.title}");
        }
    }

    [ContextMenu("Add quesiton")]
    public async void TestAddQuestion()
    {
        OpenFileName openFileName = new OpenFileName();
        string path = "";
        if (LocalDialog.GetOpenFileName(openFileName))
        {
            path = openFileName.file;
        };
        WWW www1 = new WWW("file://" + path);
        Texture2D texture = www1.texture;
        byte[] bytes = texture.EncodeToPNG();
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("testId", "29"),
            new MultipartFormDataSection("isText", "0"),
            new MultipartFormDataSection("scores", "5"),
            new MultipartFormFileSection("question", bytes, "Question.jpg", "image/jpg")
        };
        // Инициализируем соединение
        var httpClient = new HttpClient(new JsonSerializationOption());
        httpClient.headers = new List<Header>()
        {
            new Header(){name = "Authorization", value="Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOi8vYW55LXNpdGUub3JnIiwiYXVkIjoiaHR0cDovL2FueS1zaXRlLmNvbSIsImV4cCI6MTY1MDUwMDkyMiwiaWF0IjoxMzU2OTk5NTI0LCJuYmYiOjEzNTcwMDAwMDAsImRhdGEiOnsiaWQiOjg1LCJmaXJzdE5hbWUiOiJcdTA0MTBcdTA0NDBcdTA0NDJcdTA0MzVcdTA0M2MiLCJsYXN0TmFtZSI6Ilx1MDQxNVx1MDQzYlx1MDQ0Y1x1MDQzNFx1MDQzNVx1MDQzZFx1MDQzNVx1MDQzMiIsImxvZ2luIjoic2RzZCIsInJvbGUiOiJURUFDSEVSIiwiaXNBY3RpdmF0ZWQiOnRydWV9fQ.es0uv_xe396yc5aywqhARg8L8gYCpV0nhytbBlnJJTQ" }
        };
        var response = await httpClient.PostMultipart<Question>(formData, "http://localhost/educationalquest/question/create");
        if (response.isError)
        {
            Debug.Log(response.message);
        } else
        {
            Debug.Log(response.data.question);
        }
    }

    [ContextMenu("Получение максимального количества очков за тест")]
    public async void getMaxScoresTest()
    {
        var response = await TestService.getMaxScoresForTestByTestId("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOi8vYW55LXNpdGUub3JnIiwiYXVkIjoiaHR0cDovL2FueS1zaXRlLmNvbSIsImV4cCI6MTY1MjQ1MzU3MiwiaWF0IjoxMzU2OTk5NTI0LCJuYmYiOjEzNTcwMDAwMDAsImRhdGEiOnsiaWQiOjg1LCJmaXJzdE5hbWUiOiJcdTA0MTBcdTA0NDBcdTA0NDJcdTA0MzVcdTA0M2MiLCJsYXN0TmFtZSI6Ilx1MDQxNVx1MDQzYlx1MDQ0Y1x1MDQzNFx1MDQzNVx1MDQzZFx1MDQzNVx1MDQzMiIsImxvZ2luIjoic2RzZCIsInJvbGUiOiJURUFDSEVSIiwiaXNBY3RpdmF0ZWQiOnRydWV9fQ.HVeY7W3ZZUih8FRj50R9R1HeseKVI1dQgxYBzxxxbVk",
            46);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Debug.Log($"{response.data}");
        }
    }
}