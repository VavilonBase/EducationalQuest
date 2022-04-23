
using System.Collections.Generic;

public class ResponseStudentTestResult
{
    public int userId { get; set; } // Идентификатор пользователя
    public string lastName { get; set; } // Фамилия
    public string firstName { get; set; } // Имя
    public string middleName { get; set; } // Отчество
    public List<ResponseResult> results { get; set; } // Результаты
}
