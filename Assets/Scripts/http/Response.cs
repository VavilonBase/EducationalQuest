//Класс, описывающий сообщения в системе
public class Response<T>
{
    // Показывает была ли ошибка
    public bool isError { get; set; }
    // Сообщение с успехом или ошибкой
    public Message message { get; set; }
    // Данные с успехом или ошибкой
    public T data { get; set; } 

}
