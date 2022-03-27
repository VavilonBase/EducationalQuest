using System.Collections.Generic;

public class GroupDto
{
    public int groupId; // Идентификатор группы
    public int userId; // Идентификатор пользователя
    public string title; // Название группы

    public GroupDto(int _groupId, int _userId, string _title)
    {
        groupId = _groupId;
        userId = _userId;
        title = _title;
    }

    public string printGroupDto()
    {
        return "ID: " + groupId +
            "\nTitle: " + title;
    }
}