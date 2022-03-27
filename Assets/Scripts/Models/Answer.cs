class Answer
{
    int answerId; // Идентификатор ответа
    int questionId; // Идентификатор вопроса
    string answer; // Сам ответ, либо ссылка на него в виде фотографии
    bool isText; // Ответ является текстом, или фотографией
    bool isRightAnswer; // Ответ является правильным

    public Answer(int _answerId, int _questionId, string _answer, bool _isText, bool _isRightAnswer)
    {
        answerId = _answerId;
        questionId = _questionId;
        answer = _answer;
        isText = _isText;
        isRightAnswer = _isRightAnswer;
    }
}