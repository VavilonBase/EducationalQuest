
using System.Collections.Generic;

public class ResponseQuestionForTest
{
    public int questionId { get; set; } // Идентификатор вопроса
    public bool isText { get; set; } // Вопрос в виде теста?
    public string question { get; set; } // Сам вопрос
    public int scores { get; set; } // Очки за вопрос
    public List<ResponseAnswerForTest> answers { get; set; } // Ответы
}

