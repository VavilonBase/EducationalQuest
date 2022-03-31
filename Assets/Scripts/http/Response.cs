//Класс, описывающий сообщения в системе
public class Response<T>
{
    public bool isError { get; set; } // Показывает была ли ошибка
    public Message message { get; set; } // Сообщение с успехом или ошибкой
    public T data { get; set; } // Данные с успехом или ошибкой

}
