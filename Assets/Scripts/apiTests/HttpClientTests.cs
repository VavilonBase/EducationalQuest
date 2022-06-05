
using System.Collections.Generic;
using UnityEngine;

class HttpClientTests : MonoBehaviour
{
    [SerializeField] private string url;
    [SerializeField] private string jwt;
    [SerializeField] private Texture2D texture;

    [ContextMenu("Get запрос неверный url")]
    [System.Obsolete]
    public async void getWithUncorrectUrl()
    {
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        httpClient.headers = new List<Header>()
        {
            new Header() { name = "Authorization", value = $"Bearer {jwt}" }

        };
        // Создаем данные
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Get<ResponseUserData>(url);

        if (result.isError)
        {
            Debug.LogError(result.message);
        } else
        {
            Debug.Log(result.data.user.ToString());
        }
    }


    [ContextMenu("Delete запрос")]
    [System.Obsolete]
    public async void deleteQuestion()
    {
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        httpClient.headers = new List<Header>()
        {
            new Header() { name = "Authorization", value = $"Bearer {jwt}" }

        };
        // Создаем данные
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Get<ResponseUserData>(url);

        if (result.isError)
        {
            Debug.LogError(result.message);
        }
        else
        {
            Debug.Log(result.data.user.ToString());
        }
    }

    [ContextMenu("Получение изображения")]
    [System.Obsolete]
    public async void getTexture()
    {
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        httpClient.headers = new List<Header>()
        {
            new Header() { name = "Authorization", value = $"Bearer {jwt}" }

        };
        // Создаем данные
        // Отправляем запрос и ждем ответа
        var result = await httpClient.GetTexture(url);

        if (result.isError)
        {
            Debug.LogError(result.message);
        }
        else
        {
            texture = result.data;
        }
    }
}

