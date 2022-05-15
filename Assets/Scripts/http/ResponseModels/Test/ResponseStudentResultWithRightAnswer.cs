
using System.Collections.Generic;

public class ResponseStudentResultWithRightAnswer
{
    public int userId { get; set; } // Идентификатор ученика
    public int testId { get; set; } // Идентификатор теста
    public int resultId { get; set; } // Идентификатор результата
    public int totalScores { get; set; } // Количество очков за тест
    public List<ResponseFullResult> resultsData { get; set; } // Результаты
}
