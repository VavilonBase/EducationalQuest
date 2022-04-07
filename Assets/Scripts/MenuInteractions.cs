using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MenuInteractions : MonoBehaviour
{   
    public GameObject menuInput;
    public GameObject menuRegistration;
    public GameObject menuChangePassword;
    public GameObject menuAdmin;
    public GameObject menuTeacher;
    public GameObject menuStudent;
    public GameObject menuTeacherGroupsList;
    public GameObject menuJoinStudent;

    private CsGlobals gl;

    void SelectWorkingMenu(string role)
    {        
        switch (role)
        {
            case "ADMIN":
                menuAdmin.SetActive(true);                
                //UserService.activateTeacher(65, gl.playerInfo.responseUserData.jwt); -- у этого учителя явно связи, в обход всех протоколов активировался
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
        string newPassword = menuChangePassword.transform.Find("newPassword").GetComponent<InputField>().text;
        var response = await UserService.changePassword(oldPassword, newPassword, gl.playerInfo.responseUserData.jwt);

        if (response.isError)
            switch (response.message)
            {
                case Message.NotFountRequiredData:
                    StartCoroutine(gl.ChangeMessageTemporary("Проверьте правильность заполнения полей", 5));
                    break;
                case Message.PassowordNotEquals:
                    StartCoroutine(gl.ChangeMessageTemporary("Неверный старый пароль", 5));
                    break;
            }            
        else
        {
            StartCoroutine(gl.ChangeMessageTemporary("Пароль успешно изменен", 5));
        }
    }

    public async static Task<List<Group>> ShowAllGroupsList(string jwt)
    {
        var response = await GroupService.getAllTeacherGroups(jwt);
        if (response.isError)
        {
            Debug.Log("Ошибка при получении списка групп");
            return null;
        }
        else
        {
            Debug.Log("Список групп получен");
            return response.data;
        }
    }

    public async void CreateGroup()
    {  
        string groupName = menuTeacherGroupsList.transform.Find("Name_group").Find("Text").GetComponent<Text>().text;
        var response = await GroupService.createGroup(groupName, gl.playerInfo.responseUserData.jwt);
        if (response.isError)
        {
            //Выводить предупреждение, если учитель не активирован
            Debug.Log("Ошибка при создании группы");
        }
        else
        {
            Debug.Log("Группа успешно создана");
            menuTeacherGroupsList.SetActive(false);
            menuTeacherGroupsList.SetActive(true);
        }
    }

    public async void ShowGroupContent()
    {
        MenuTeacherGroupsListInteractions menu = menuTeacherGroupsList.GetComponent<MenuTeacherGroupsListInteractions>();
        var list = menu.listGroups;     
        Text text = menuTeacherGroupsList.transform.Find("Panel").GetComponent<Text>();

        Dropdown dd = menuTeacherGroupsList.transform.Find("Dropdown").GetComponent<Dropdown>();
        int groupId = list[dd.value].groupId;

        //string sRole = dd.options[dd.value].text;

        string t = "Кодовое слово = " + list[dd.value].codeWord + "\n";

        var response = await GroupService.getGroupStudents(gl.playerInfo.responseUserData.jwt, groupId);
        if (response.isError)
        {
            Debug.Log("Ошибка при получении списка группы");
        }
        else
        {
            var listStudents = response.data;            
            foreach (ResponseUserGroupData student in listStudents)
            {
                t += student.lastName + " " + student.firstName + " (" + student.userId + ")\n";
            }
        }
        text.text = t;
    }

    public async void JoinGroup()
    {
        string codeWord = menuJoinStudent.transform.Find("InputField").GetComponent<InputField>().text;
        var response = await GroupService.joinStudentToTheGroup(gl.playerInfo.responseUserData.jwt, codeWord);
        if (response.isError)
        {
            Debug.Log("Ошибка при вступлении в группу " + codeWord);
        }
        else
        {
            Debug.Log("Успешное вступление в группу");
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
