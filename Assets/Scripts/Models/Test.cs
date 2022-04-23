public class Test
{
    public int testId { get; set; } // Идентификатор пользователя
    public int groupId { get; set; } // Идентификатор группы
    public string title { get; set; } // Название теста
    public bool isOpened { get; set; } // Начать ли тест
    public bool canViewResult { get; set; } // Можно ли посмотреть результаты
}