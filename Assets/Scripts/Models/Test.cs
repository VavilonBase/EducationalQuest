public class Test
{
    int testId; // Идентификатор пользователя
    int groupId; // Идентификатор группы
    string title; // Название теста
    bool isOpened; // Начать ли тест
    bool isClosed; // Окнчен ли тест
    bool canViewResult; // Можно ли посмотреть результаты

    public Test(int _testId, int _groupId, string _title)
    {
        testId = _testId;
        groupId = _groupId;
        title = _title;
        isOpened = false;
        isClosed = false;
        canViewResult = false;
    }
}