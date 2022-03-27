using System.Collections.Generic;

public class Group : GroupDto
{
    public string codeWord; // Кодовое слово группы

    public Group(int _groupId, int _userId, string _title, string _codeWord)
        :base(_groupId, _userId, _title)
    {
        codeWord = _codeWord;
    }

    public string printGroup()
    {
        return printGroupDto() + "\nCodeWord: " + codeWord;
    }
}
