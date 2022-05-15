
public class AnswerResultQuestionDto
{
    public int answerId { get; set; } // Идентификатор ответа
    public int questionId { get; set; } // Идентификатор вопроса

    public AnswerResultQuestionDto(int answerID, int questionID)
    {
        answerId = answerID;
        questionId = questionID;
    }
}
