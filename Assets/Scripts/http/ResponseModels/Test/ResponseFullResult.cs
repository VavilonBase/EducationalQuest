
public class ResponseFullResult
{
    public int questionId { get; set; } // Идентификатор вопроса
    public string question { get; set; } // Сам вопрос
    public bool isQuestionText { get; set; } // Вопрос текстовый?
    public int answerId { get; set; } // Идентификатор ответа
    public string answer { get; set; } // Сам ответ
    public bool isAnswerText { get; set; } // Ответ текстовый?
    public int rightAnswerId { get; set; } // Идентификатор правильного ответа
    public string rightAnswer { get; set; } // Сам правильный ответ
    public bool isRightAnswer { get; set; } // Правильный ответ текстовый?
}
