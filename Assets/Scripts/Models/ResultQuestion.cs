class ResultQuestion
{
    int resultQuestionId; // Идентификатр результата ответа на вопрос
    int questionId; // Идентификатор вопроса
    int answerId; // Идентификатор ответа
    
    public ResultQuestion(int _resultQuestionId, int _questionId, int _answerId)
    {
        resultQuestionId = _resultQuestionId;
        questionId = _questionId;
        answerId = _answerId;
    }
}