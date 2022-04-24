
using System.Collections.Generic;

public class CreateResultData
{
    public int testId { get; set; } // Идентификатор вопроса
    public List<AnswerResultQuestionDto> answers { get; set; } // Ответы
}
