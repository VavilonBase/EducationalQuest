﻿using Newtonsoft.Json;
using System;
using UnityEngine;

public class JsonSerializationOption : ISerializationOption
{
    // Заголовок Content-Type
    public string ContentType => "application/json";
    // Преобразование текста в формате JSON в объект
    public T Deserialize<T>(string text)
    {
        try
        {
            var result = JsonConvert.DeserializeObject<T>(text);
            Debug.Log($"Success: {text}");
            return result;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Could not parse response {text}.{ex.Message}");
            return default;
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