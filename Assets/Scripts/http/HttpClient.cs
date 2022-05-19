using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class HttpClient
{
    // Интерфейс для сериализации данных
    private readonly ISerializationOption _serializationOption;
    // Список заголовков, необходимых добавить к запросу
    public List<Header> headers { get; set; }

    // Конструктор
    public HttpClient()
    {

    }

    // Конструктор с параметром интерфейса для сериализации и десериализации
    public HttpClient(ISerializationOption serializationOption)
    {
        this._serializationOption = serializationOption;
    }

    // GET запрос
    public async Task<Response<TResultType>> Get<TResultType>(string url)
    {
        try
        {
            // Инициализируем соединение
            using var www = UnityWebRequest.Get(url);
            // Устанавливаем заголовки
            www.SetRequestHeader("Content-Type", _serializationOption.ContentType + "; charset=UTF8");
            SetHeaders(www);
            // Отправляем запрос
            var operation = www.SendWebRequest();
            // Ждем, когда запрос выполнится
            while (!operation.isDone)
                await Task.Yield();

            // Если возникла ошибка, выведем ее
            if (www.result != UnityWebRequest.Result.Success)
                return ReturnedNetworkErrors<TResultType>(www.result);

            // Получаем текст ответа
            var jsonResponse = www.downloadHandler.text;

            // Переводим текст в объект
            return _serializationOption.Deserialize<Response<TResultType>>(jsonResponse);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[{nameof(Get)}].{ex.Message}");
            return new Response<TResultType>()
            {
                isError = true,
                message = Message.UndefinedError,
                data = default(TResultType)
            };
        } finally
        {
            ClearHeaders();
        }
    }

    // POST запрос
    public async Task<Response<TResultType>> Post<TResultType, TRequestType>(TRequestType data, string url)
    {
        try
        {
            // Сериализуем данные
            string serializedData = _serializationOption.Serialize<TRequestType>(data);
            // Создаем форму для отправки данных
            WWWForm formData = new WWWForm();
            // Инициализируем соединение
            using var www = UnityWebRequest.Post(url, formData);
            // Сериализованную строку превращаем в массив байт в кодировке UTF8
            byte[] loginBytes = System.Text.Encoding.UTF8.GetBytes(serializedData);
            // Создаем обработчик обновления и помещаем массив байт в него
            UploadHandler uploadHandler = new UploadHandlerRaw(loginBytes);
            // Для созданного ранее соединения обновляем обработчик обновления, на созданный ранее
            www.uploadHandler = uploadHandler;
            // Устанавливаем заголовки
            www.SetRequestHeader("Content-Type", _serializationOption.ContentType);
            SetHeaders(www);
            // Отправляем запрос
            var operation = www.SendWebRequest();
            // Ждем, когда запрос выполнится
            while (!operation.isDone)
                await Task.Yield();

            // Если возникла ошибка, выведем ее
            if (www.result != UnityWebRequest.Result.Success)
                return ReturnedNetworkErrors<TResultType>(www.result);

            // Получаем текст ответа
            var jsonResponse = www.downloadHandler.text;

            // Переводим текст в объект
            return _serializationOption.Deserialize<Response<TResultType>>(jsonResponse);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[{nameof(Post)}].{ex.Message}");
            return new Response<TResultType>()
            {
                isError = true,
                message = Message.UndefinedError,
                data = default(TResultType)
            };
        } finally
        {
            ClearHeaders();
        }
    }

    // DELETE запрос
    public async Task<Response<TResultType>> Delete<TResultType>(string url)
    {
        try
        {
            // Инициализируем соединение
            using var www = UnityWebRequest.Delete(url);
            // Прикрепляем downloadHandler
            www.downloadHandler = new DownloadHandlerBuffer();
            // Устанавливаем заголовки
            www.SetRequestHeader("Content-Type", _serializationOption.ContentType + "; charset=UTF8");
            SetHeaders(www);
            // Отправляем запрос
            var operation = www.SendWebRequest();
            // Ждем, когда запрос выполнится
            while (!operation.isDone)
                await Task.Yield();

            // Если возникла ошибка, выведем ее
            if (www.result != UnityWebRequest.Result.Success)
                return ReturnedNetworkErrors<TResultType>(www.result);

            // Получаем текст ответа
            var jsonResponse = www.downloadHandler.text;

            // Переводим текст в объект
            return _serializationOption.Deserialize<Response<TResultType>>(jsonResponse);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[{nameof(Delete)}].{ex.Message}");
            return new Response<TResultType>()
            {
                isError = true,
                message = Message.UndefinedError,
                data = default(TResultType)
            };
        }
        finally
        {
            ClearHeaders();
        }
    }

    // POST запрос c возможностью добавления медиа-файлов
    public async Task<Response<TResultType>> PostMultipart<TResultType>(List<IMultipartFormSection> data, string url)
    {
        try
        {
            // Инициализируем соединение
            using var www = UnityWebRequest.Post(url, data);
            SetHeaders(www);
            // Отправляем запрос
            var operation = www.SendWebRequest();
            // Ждем, когда запрос выполнится
            while (!operation.isDone)
                await Task.Yield();
            // Получаем текст ответа
            var jsonResponse = www.downloadHandler.text;
            // Если возникла ошибка, выведем ее
            if (www.result != UnityWebRequest.Result.Success)
                return ReturnedNetworkErrors<TResultType>(www.result);

            // Переводим текст в объект
            return _serializationOption.Deserialize<Response<TResultType>>(jsonResponse);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[{nameof(Post)}].{ex.Message}");
            return new Response<TResultType>()
            {
                isError = true,
                message = Message.UndefinedError,
                data = default(TResultType)
            };
        }
        finally
        {
            ClearHeaders();
        }
    }

    // Получение изображения
    public async Task<Response<Texture2D>> GetTexture(string url)
    {
        try
        {
            // Инициализируем соединение
            using var www = UnityWebRequestTexture.GetTexture(url);

            // Отправляем запрос
            var operation = www.SendWebRequest();

            // Ждем, когда запрос выполнится
            while (!operation.isDone)
                await Task.Yield();

            // Если возникла ошибка, выведем ее
            if (www.result != UnityWebRequest.Result.Success)
                    return ReturnedNetworkErrors<Texture2D>(www.result);

            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            return new Response<Texture2D>()
            {
                isError = false,
                message = Message.NotError,
                data = texture
            };
        }
        catch (Exception ex)
        {
            Debug.LogError($"[{nameof(Post)}].{ex.Message}");
            return new Response<Texture2D>()
            {
                isError = true,
                message = Message.UndefinedError,
                data = default(Texture2D)
            };
        }
        finally
        {
            ClearHeaders();
        }
    }

    // Установка заголовков в запрос
    private void SetHeaders(UnityWebRequest www)
    {
        // Если нет header-ов, то возвращаемся из функции
        if (headers == null || headers.Count == 0) return;
        // Иначе выставляем заголовки
        foreach (Header header in headers)
        {
            www.SetRequestHeader(header.name, header.value);
        }
    } 

    // Очистка заголовков
    private void ClearHeaders()
    {
        headers = null;
    }

    // Метод по обработке ошибок, возникших при отправке запроса
    private Response<TResultType> ReturnedNetworkErrors<TResultType>(UnityWebRequest.Result result)
    {
        // Создаем ответ с ошибкой
        Response<TResultType> response = new Response<TResultType>()
        {
            isError = true,
            data = default(TResultType)
        };

        // Вычисляем какая ошибка произошла
        switch (result)
        {
            case UnityWebRequest.Result.ConnectionError:
                response.message = Message.ConnectionError;
                break;
            case UnityWebRequest.Result.ProtocolError:
                response.message = Message.ProtocolError;
                break;
            default:
                response.message = Message.UndefinedError;
                break;
        }

        // Возвращаем ответ
        return response;
    }
}