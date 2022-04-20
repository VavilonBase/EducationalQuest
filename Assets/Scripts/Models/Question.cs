public class Question
{
    public int questionId { get; set; } //Идентификатор вопроса
    public int testId { get; set; } // Идентификатор теста
    public string question { get; set; } // Сам вопрос, либо ссылка на него в виде фотографии
    public bool isText { get; set; } // Вопрос является текстом
    public int scores { get; set; } // Очки за вопрос
}