
public interface ISerializationOption
{
    // Тип передаваемых данных, в нашем случае application/json или multipart, при передачи данных с медиа-файлами
    string ContentType { get; }
    // Метод преобразование строки формата JSON в объект типа T
    T Deserialize<T>(string text);
    // Метод преобразования объекта, типа T в строку формата JSON
    string Serialize<T>(T data);
}