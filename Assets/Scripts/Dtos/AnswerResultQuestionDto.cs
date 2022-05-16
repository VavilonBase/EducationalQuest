
public class AnswerResultQuestionDto
{
    public int answerId { get; set; } // Идентификатор ответа
    public int questionId { get; set; } // Идентификатор вопроса

    public AnswerResultQuestionDto(int questionID, int answerID)
    {
        answerId = answerID;
        questionId = questionID;
    }
}
