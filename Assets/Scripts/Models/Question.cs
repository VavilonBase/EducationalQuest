class Question
{
    int questionId; //Идентификатор вопроса
    int testId; // Идентификатор теста
    string question; // Сам вопрос, либо ссылка на него в виде фотографии
    bool isText; // Вопрос является текстом

    public Question(int _questionId, int _testId, string _question, bool _isText)
    {
        questionId = _questionId;
        testId = _testId;
        question = _question;
        isText = _isText;
    }
}