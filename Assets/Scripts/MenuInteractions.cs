using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MenuInteractions : MonoBehaviour
{   
    public GameObject menuInput; // --- меню ввода логина и парол€
    public GameObject menuRegistration; // --- меню регистрации
    public GameObject menuChangePassword; // --- меню смены парол€
    public GameObject menuAdmin; // --- главное меню администратора
    public GameObject menuTeacher; // --- главное меню учител€
    public GameObject menuStudent; // --- главное меню ученика

    private CsGlobals gl;

    void Awake()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
    }

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
        gl.menuStart.SetActive(false);            
    }

    public void LeaveSession()
    {
        gl.playerInfo.isAuthorized = false;
        gl.playerInfo.responseUserData = new ResponseUserData();
        Saving.SaveSerial.SaveAccountSettings(gl.playerInfo.responseUserData);        
        gl.menuStart.SetActive(false); gl.menuStart.SetActive(true); //перезагрузка меню
    }

    public async void login()
    {       
        string log = menuInput.transform.Find("In_log").Find("Text").GetComponent<Text>().text;
        string pass = menuInput.transform.Find("In_pas").Find("Text").GetComponent<Text>().text;
        if (log != "" && pass != "")
        {
            var response = await UserService.login(log, pass);
            if (response.isError)
            {
                switch (response.message)
                {
                    case Message.IncorrectPassword:
                        gl.ChangeMessageTemporary("Ќеправильный логин или пароль", 5);
                        break;
                    case Message.UserNotExist:
                        gl.ChangeMessageTemporary("“акого пользовател€ не существует", 5);
                        break;
                    default:
                        gl.ChangeMessageTemporary(response.message.ToString(), 5);
                        break;
                }
            }
            else
            {
                gl.playerInfo.responseUserData = response.data;
                Saving.SaveSerial.SaveAccountSettings(gl.playerInfo.responseUserData);               
                gl.ChangeMessageDurable(true, "ƒобро пожаловать, " + gl.playerInfo.responseUserData.user.firstName);
                gl.playerInfo.isAuthorized = true;
                DataHolder.PlayerInfo = gl.playerInfo; // сохранение в статический класс дл€ использовани€ на игровой сцене
                SelectWorkingMenu(gl.playerInfo.responseUserData.user.role);
            }
        }
        else gl.ChangeMessageTemporary("«аполните требуемые пол€", 5);
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
                    gl.ChangeMessageTemporary("ѕользователь уже существует", 5);                    
                    break;
                case Message.NotFoundRequiredData:
                    gl.ChangeMessageTemporary("«аполните требуемые пол€", 5);
                    break;
                default:
                    gl.ChangeMessageTemporary(response.message.ToString(), 5);
                    break;
            }           
        }
        else
        {
            gl.ChangeMessageTemporary("–егистраци€ прошла успешно", 5);                       
            //сразу заполн€ем пол€ в форме входа, чтобы пользователю не пришлось вводить их вручную
            menuInput.transform.Find("In_log").GetComponent<InputField>().text = login;
            menuInput.transform.Find("In_pas").GetComponent<InputField>().text = password;
            menuInput.SetActive(true); // возвращаем меню входа
            menuRegistration.SetActive(false); // скрываем меню регистрации
        }        
    }

    public async void ChangePassword()
    {
        if (gl.playerInfo.isAuthorized)
        {
            string oldPassword = menuChangePassword.transform.Find("oldPassword").GetComponent<InputField>().text;
            string newPassword = menuChangePassword.transform.Find("newPassword").GetComponent<InputField>().text;
            var response = await UserService.changePassword(oldPassword, newPassword, gl.playerInfo.responseUserData.jwt);

            if (response.isError)
                switch (response.message)
                {
                    case Message.NotFoundRequiredData:
                        gl.ChangeMessageTemporary("ѕроверьте правильность заполнени€ полей", 5);
                        break;
                    case Message.PasswordNotEquals:
                        gl.ChangeMessageTemporary("Ќеверный старый пароль", 5);
                        break;
                }
            else
                gl.ChangeMessageTemporary("ѕароль успешно изменен", 5);
        }
        else gl.ChangeMessageTemporary("¬ойдите в аккаунт, чтобы совершить это действие", 5);
    }    
}
