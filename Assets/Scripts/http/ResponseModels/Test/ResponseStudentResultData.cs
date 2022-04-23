
using System.Collections.Generic;

public class ResponseStudentResultData
{
    public int resultId { get; set; } // Идентификатор результата
    public int totalScores { get; set; } // Очки за тест по текущему результату
    public List<ResponseFullResult> results { get; set; } // Результаты
}
