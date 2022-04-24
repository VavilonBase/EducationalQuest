
using System.Collections.Generic;
using UnityEngine;

class GroupTests : MonoBehaviour
{
    [SerializeField] int groupId; // Идентификатор группы
    [SerializeField] int userId; // Идентификатор пользователя
    [SerializeField] int studentId; // Идентификатор студента
    [SerializeField] string title; // Название группы
    [SerializeField] string codeWord; // Кодовое слово группы
    [SerializeField] string creatorJwt; // Токен
    [SerializeField] string studentJwt; // Токен
    [SerializeField] List<string> teacherGroups = new List<string>(); // Группы учителя
    [SerializeField] List<string> studentGroups = new List<string>(); // Группы ученика
    [SerializeField] List<string> groupStudents = new List<string>(); // Ученики группы
    [ContextMenu("Create Group")]
    [System.Obsolete]
    public async void create()
    {
        var response = await GroupService.createGroup(title, creatorJwt);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Group data = response.data;
            groupId = data.groupId;
            userId = data.userId;
            title = data.title;
            codeWord = data.codeWord;
        }
    }

    [ContextMenu("Update Group")]
    [System.Obsolete]
    public async void update()
    {
        var response = await GroupService.updateGroup(creatorJwt, title, groupId);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Group data = response.data;
            groupId = data.groupId;
            userId = data.userId;
            title = data.title;
            codeWord = data.codeWord;
        }
    }

    [ContextMenu("Delete Group")]
    [System.Obsolete]
    public async void delete()
    {
        var response = await GroupService.deleteGroup(creatorJwt, groupId);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Debug.Log("OK");
        }
    }

    [ContextMenu("Remove Student From Group")]
    [System.Obsolete]
    public async void removeStudentFromGroup()
    {
        var response = await GroupService.removeStudentFromGroup(creatorJwt, studentId, groupId);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Debug.Log(response.data);
        }
    }

    [ContextMenu("Leaving Student From Group")]
    [System.Obsolete]
    public async void leavingStudentFromGroup()
    {
        var response = await GroupService.leavingStudentFromGroup(studentJwt, groupId);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Debug.Log(response.data);
        }
    }

    [ContextMenu("Get All Teacher Groups")]
    [System.Obsolete]
    public async void getAllTeacherGroups()
    {
        var response = await GroupService.getAllTeacherGroups(creatorJwt);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            teacherGroups = new List<string>();
            foreach (var group in response.data)
            {
                teacherGroups.Add($"{groupId} {group.title} {group.codeWord}");
            }
        }
    }

    [ContextMenu("Join Student To The Group")]
    [System.Obsolete]
    public async void joinStudentToTheGroup()
    {
        var response = await GroupService.joinStudentToTheGroup(studentJwt, codeWord);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            Debug.Log(response.data);
        }
    }

    [ContextMenu("Get Student Groups")]
    [System.Obsolete]
    public async void getStudentGroups()
    {
        var response = await GroupService.getStudentGroups(studentJwt);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            studentGroups = new List<string>();
            foreach (var group in response.data)
            {
                studentGroups.Add($"{groupId} {group.title}");
            }
        }
    }

    [ContextMenu("Get Group Students")]
    [System.Obsolete]
    public async void getGroupStudents()
    {
        var response = await GroupService.getGroupStudents(creatorJwt, groupId);
        if (response.isError)
        {
            Debug.Log(response.message);
        }
        else
        {
            groupStudents = new List<string>();
            foreach (var student in response.data)
            {
                groupStudents.Add($"{student.userId} {student.firstName} {student.lastName} {student.middleName}");
            }
        }
    }
}
