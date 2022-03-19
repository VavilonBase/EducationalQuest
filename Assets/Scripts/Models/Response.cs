//Класс, описывающий сообщения в системе
public class Response<T>
{
    public bool isError; // Показывает была ли ошибка
    public Message message; // Сообщение с успехом или ошибкой
    public T data; // Данные с успехом или ошибкой

    public Response(bool _isError, Message _message, T _data)
    {
        isError = _isError;
        message = _message;
        data = _data;
    }
}
