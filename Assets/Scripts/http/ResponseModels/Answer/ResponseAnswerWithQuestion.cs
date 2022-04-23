
public class ResponseAnswerWithQuestion
{
    public int answerId { get; set; } // Идентификатор ответа
    public string answer { get; set; } // Сам ответ, либо ссылка на него в виде фотографии
    public bool isText { get; set; } // Ответ является текстом, или фотографией
    public bool isRightAnswer { get; set; } // Ответ является правильным
}
