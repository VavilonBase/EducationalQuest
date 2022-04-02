using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuInteractions : MonoBehaviour
{
    public GameObject menuStart;
    public GameObject menuInput;
    public GameObject menuRegistration;
    public GameObject menuChangePassword;
    public GameObject menuAdmin;
    public GameObject menuTeacher;
    public GameObject menuStudent;

    private CsGlobals gl;

    void SelectWorkingMenu(string role)
    {        
        switch (role)
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

    public void StartSession()
    {
        if (gl.playerInfo.isAuthorized) 
            SelectWorkingMenu(gl.playerInfo.responseUserData.user.role);
        else 
            menuInput.SetActive(true);
        menuStart.SetActive(false);            
    }

    public void LeaveSession()
    {
        gl.playerInfo.isAuthorized = false;
        gl.playerInfo.responseUserData = new ResponseUserData();
        Saving.SaveSerial.SaveAccountSettings(gl.playerInfo.responseUserData);
        gl.ChangeMessageDurable(false);
    }

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
                    StartCoroutine(gl.ChangeMessageTemporary("Неправильный логин или пароль", 5));                    
                    break;
                case Message.UserNotExist:
                    StartCoroutine(gl.ChangeMessageTemporary("Такого пользователя не существует", 5));                    
                    break;
                default:
                    StartCoroutine(gl.ChangeMessageTemporary("Что-то пошло не так, но вы держитесь", 5));                    
                    break;
            }            
        }
        else
        {
            gl.playerInfo.responseUserData = response.data;
            Saving.SaveSerial.SaveAccountSettings(gl.playerInfo.responseUserData);

            gl.ChangeMessageDurable(true, "Здравствуй, " + gl.playerInfo.responseUserData.user.firstName);
            gl.playerInfo.isAuthorized = true;
            SelectWorkingMenu(gl.playerInfo.responseUserData.user.role);
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
                    StartCoroutine(gl.ChangeMessageTemporary("Пользователь уже существует", 5));                    
                    break;
                case Message.CanNotCreateUser:
                    StartCoroutine(gl.ChangeMessageTemporary("Проверьте правильность заполнения полей", 5));
                    break;
                default:
                    StartCoroutine(gl.ChangeMessageTemporary("Что-то пошло не так, но вы держитесь", 5));                    
                    break;
            }           
        }
        else
        {
            StartCoroutine(gl.ChangeMessageTemporary("Регистрация прошла успешно", 5));                       
            menuInput.transform.Find("In_log").GetComponent<InputField>().text = login;
            menuInput.transform.Find("In_pas").GetComponent<InputField>().text = password;
            menuInput.SetActive(true);
            menuRegistration.SetActive(false);
        }        
    }

    public async void ChangePassword()
    {
        string oldPassword = menuChangePassword.transform.Find("oldPassword").GetComponent<InputField>().text;
        var response = await UserService.login(gl.playerInfo.responseUserData.user.login, oldPassword);
        if (response.isError)
            StartCoroutine(gl.ChangeMessageTemporary("Неверный пароль", 5));
        else
        {
            //UserService.updateUser();
            StartCoroutine(gl.ChangeMessageTemporary("Всё верно, но поживи пока со старым паролем :)", 5));
        }
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
