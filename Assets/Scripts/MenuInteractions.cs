using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuInteractions : MonoBehaviour
{
    public GameObject message;
    public GameObject menuInput;
    public GameObject menuRegistration;
    public GameObject menuAdmin;
    public GameObject menuTeacher;
    public GameObject menuStudent;

    private CsGlobals gl;

    public async void login()
    {
        string log = menuInput.transform.Find("In_log").Find("Text").GetComponent<Text>().text;
        string pass = menuInput.transform.Find("In_pas").Find("Text").GetComponent<Text>().text;     
        
        //var response = await UserService.login("vav", "1233");
        var response = await UserService.login(log, pass);
        if (response.isError)
        {
            switch (response.message)
            {
                case Message.IncorrectPassword:
                    message.GetComponent<Text>().text = "Неправильный логин или пароль";
                    break;
                case Message.UserNotExist:
                    message.GetComponent<Text>().text = "Такого пользователя не существует";
                    break;
                default:
                    message.GetComponent<Text>().text = "Что-то пошло не так, но вы держитесь";
                    break;
            }
            message.SetActive(true);
        }
        else
        {
            gl.playerInfo.user = response.data.user;
            gl.playerInfo.jwt = response.data.jwt;
            message.GetComponent<Text>().text = "Здравствуй, " + gl.playerInfo.user.firstName;
            message.SetActive(true);
            switch (gl.playerInfo.user.role)
            {
                case "ADMIN":
                    menuAdmin.SetActive(true);
                    break;
                case "TEACHER":
                    menuTeacher.SetActive(true);
                    break;
                case "STUDENT":
                    menuStudent.SetActive(true);
                    break;
            }
            menuInput.SetActive(false);               
        }
    }

    public async void registration()
    {
        string lastName, firstName, middleName, login, password;
        UserService.RolesEnum role;
        firstName = menuRegistration.transform.Find("In_name").Find("Text").GetComponent<Text>().text;
        lastName = menuRegistration.transform.Find("In_surname").Find("Text").GetComponent<Text>().text;        
        middleName = menuRegistration.transform.Find("In_patronymic").Find("Text").GetComponent<Text>().text;
        login = menuRegistration.transform.Find("In_log").Find("Text").GetComponent<Text>().text;
        password = menuRegistration.transform.Find("In_pas").Find("Text").GetComponent<Text>().text;

        Dropdown dd = menuRegistration.transform.Find("Dropdown").GetComponent<Dropdown>();
        string sRole = dd.options[dd.value].text;       

        switch (sRole) 
        {
            case "учитель":
                role = UserService.RolesEnum.Teacher;
                break;
            case "ученик":
                role = UserService.RolesEnum.Student;
                break;
            default:
                role = UserService.RolesEnum.Student;
                break;
        }

        var response = await UserService.registration(firstName, lastName, role, false, login, password, middleName);
        if (response.isError)
        {
            switch (response.message)
            {
                case Message.UserExist:
                    message.GetComponent<Text>().text = "Пользователь уже существует";
                    break;
                case Message.CanNotCreateUser:
                    message.GetComponent<Text>().text = "Проверьте правильность заполнения полей";
                    break;
                default:
                    message.GetComponent<Text>().text = "Что-то пошло не так, но вы держитесь";
                    break;
            }
            message.SetActive(true);
        }
        else
        {
            message.GetComponent<Text>().text = "Регистрация прошла успешно";
            message.SetActive(true);
            menuInput.SetActive(true);
            menuInput.transform.Find("In_log").Find("Text").GetComponent<Text>().text = login;            
            menuInput.transform.Find("In_pas").Find("Text").GetComponent<Text>().text = password;          
            menuRegistration.SetActive(false);
        }

        menuInput.SetActive(true);
        menuInput.transform.Find("In_log").GetComponent<InputField>().text = login;
        menuInput.transform.Find("In_pas").GetComponent<InputField>().text = password;
        menuRegistration.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
