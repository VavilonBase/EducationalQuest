
using System.Collections.Generic;

public class ResponseStudentResultWithRightAnswer
{
    public int userId { get; set; } // Идентификатор ученика
    public int testId { get; set; } // Идентификатор результата
    public List<ResponseStudentResultData> resultsData { get; set; } // Результаты
}
