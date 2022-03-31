﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class TestController : MonoBehaviour
{
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
        var response = await UserService.login("vav", "1233");

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
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/api/users/get_all_users.php";
        // Инициализируем соединение
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hbnktc2l0ZS5vcmciLCJhdWQiOiJodHRwOlwvXC9hbnktc2l0ZS5jb20iLCJleHAiOjE2NDg3NDE5MzIsImlhdCI6MTM1Njk5OTUyNCwibmJmIjoxMzU3MDAwMDAwLCJkYXRhIjp7ImlkIjozLCJmaXJzdE5hbWUiOiJBcnRlbSIsImxhc3ROYW1lIjoiXHUwNDE1XHUwNDNiXHUwNDRjXHUwNDM0XHUwNDM1XHUwNDNkXHUwNDM1XHUwNDMyMTIzMTIzIiwibG9naW4iOiJ2YXYiLCJyb2xlIjoiQURNSU4iLCJpc0FjdGl2YXRlZCI6dHJ1ZX19.8nb6wWdUDaA6nuyOqXNMUTHH4yYFEX_JeiYxyLhBjzM" }
        };

        var result = await httpClient.Get<List<UserDto>>(url);

        Debug.Log($"{result.data[1].userId}");
    }
}