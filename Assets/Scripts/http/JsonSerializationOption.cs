/*
Назначение: Класс, отвечающий за сериализацию и десериализацию
Входные данные: данные, которые необходимо сериализовать и десериализовать, а так же тип этих данных
Результат: либо объект, необходимого типа (при десериализации), либо строка (при сериализации)
ФИО: Ельденев Артем Тавросович
Дата написания: 15.10.2022 г
Версия: 2.0
*/

using Newtonsoft.Json;
using System;
using UnityEngine;

public class JsonSerializationOption : ISerializationOption
{
    // Заголовок Content-Type
    public string ContentType => "application/json";
    // Преобразование текста в формате JSON в объект
    public Response<T> Deserialize<T>(string text)
    {
        try
        {
            var result = JsonConvert.DeserializeObject<Response<T>>(text);
            Debug.Log($"Success: {text}");
            return result;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Could not parse response {text}.{ex.Message}");
            return new Response<T>()
            {
                isError = true,
                message = Message.CouldNotParseResponse,
                data = default(T)
            };
        }
    }

    // Преобразование объекта, определенного типа в строку формата JSON
    public string Serialize<T>(T data)
    {
        try
        {
            string result = JsonConvert.SerializeObject(data);
            Debug.Log($"Seccess serialize: {result}");
            return result;
        } catch (Exception ex)
        {
            Debug.LogError($"Could not serialize data. {ex.Message}");
            return default;
        }
    }
}