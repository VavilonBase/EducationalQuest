
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

class AnswerService
{

    /// <summary>
    /// Создание ответа
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_answerId">ID ответа</param>
    /// <param name="_questionId">ID вопроса</param>
    /// <param name="answer">Ответы</param>
    /// <param name="_isText">Ответ текстовый?</param>
    /// <param name="_isRightAnswer">Ответ правильный?</param>
    /// <returns>Возвращает новый ответ
    /// IncorrectTokenFormat
    /// AccessDenied
    /// NotFoundRequiredData
    /// TestNotExist
    /// UserIsNotCreatorTest
    /// CanNotLoadFile
    /// CanNotPublishFile
    /// </returns>
    [System.Obsolete]
    public async static Task<Response<Answer>> createAnswer(string jwt, int _questionId, string _answer, bool _isText, bool _isRightAnswer)
    {
        // URL
        string url = "https://educationalquest.herokuapp.com/answer/create";
        // Создаем данные формы
        var formData = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("questionId", _questionId.ToString()),
            new MultipartFormDataSection("answer", _answer),
            new MultipartFormDataSection("isText", _isText ? "1" : "0"),
            new MultipartFormDataSection("isRightAnswer", _isRightAnswer ? "1" : "0")
        };
        // Если ответ текстовый
        if (_isText)
        {
            // Добавляем в форму текстовый ответ
            formData.Add(new MultipartFormDataSection("answer", _answer));
        }
        else
        {
            // Если ответ ввиде файла
            // Получаем ответ ввиде картинки
            WWW www = new WWW("file://" + _answer);
            if (!string.IsNullOrEmpty(www.error))
            {
                return new Response<Answer>() { data = null, isError = true, message = Message.CanNotLoadFile };
            }
            Texture2D texture = www.texture;
            // Кодируем его в байты
            byte[] answerBytes = texture.EncodeToPNG();
            // Добавляем в форму картинку
            formData.Add(new MultipartFormFileSection("answer", answerBytes, "Answer.jpg", "image/jpg"));
        }


        // Инициализируем соединение
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Добавляем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header(){name = "Authorization", value="Bearer " + jwt }
        };
        // Отправляем запрос
        return await httpClient.PostMultipart<Answer>(formData, url);
    }

    /// <summary>
    /// Обновление ответа
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="answerId">ID ответа</param>
    /// <param name="_questionId">ID вопроса</param>
    /// <param name="_answerOrFilePath">Ответ</param>
    /// <param name="_isText">Ответ текстовый?</param>
    /// <param name="_isRightAnswer">Ответ правильный?</param>
    /// <returns>
    /// IncorrectTokenFormat
    /// AccessDenied
    /// NotFoundRequiredData
    /// TestNotExist
    /// UserIsNotCreatorTest
    /// CanNotLoadFile
    /// CanNotPublishFile
    /// </returns>
    [System.Obsolete]
    public async static Task<Response<Answer>> updateAnswer(string jwt, int _answerId, int _questionId, string _answerOrFilePath, bool _isText, bool _isRightAnswer)
    {
        // URL
        string url = "https://educationalquest.herokuapp.com/answer/update";
        // Создаем данные формы
        var formData = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("answerId", _answerId.ToString()),
            new MultipartFormDataSection("questionId", _questionId.ToString()),
            new MultipartFormDataSection("answer", _answerOrFilePath),
            new MultipartFormDataSection("isText", _isText ? "1" : "0"),
            new MultipartFormDataSection("isRightAnswer", _isRightAnswer ? "1" : "0")
        };
        // Если ответ текстовый
        if (_isText)
        {
            // Добавляем в форму текстовый вопрос
            formData.Add(new MultipartFormDataSection("answer", _answerOrFilePath));
        }
        else
        {
            // Если ответ ввиде файла
            // Получаем ответ ввиде картинки
            WWW www = new WWW(_answerOrFilePath);
            if (!string.IsNullOrEmpty(www.error))
            {
                return new Response<Answer>() { data = null, isError = true, message = Message.CanNotLoadFile };
            }
            Texture2D texture = www.texture;
            // Кодируем его в байты
            byte[] answerBytes = texture.EncodeToPNG();
            // Добавляем в форму картинку
            formData.Add(new MultipartFormFileSection("answer", answerBytes, "Answer.jpg", "image/jpg"));
        }

        // Инициализируем соединение
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Добавляем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header(){name = "Authorization", value="Bearer " + jwt }
        };
        // Отправляем запрос
        return await httpClient.PostMultipart<Answer>(formData, url);
    }

    /// <summary>
    /// Удаление ответа
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_answerId">ID ответа</param>
    /// <returns>Возвращает true, если все хорошо, или null, если все плохо
    /// IncorrectTokenFormat
    /// AccessDenied
    /// NotFoundRequiredData
    /// NotFoundQuestion
    /// TestNotExist
    /// UserIsNotCreatorTest
    /// CanNotDeleteFileOrFolder
    /// </returns>
    public async static Task<Response<object>> delete(string jwt, int _answerId)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/answer/delete?answerId=" + _answerId;
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Delete<object>(url);
        return result;
    }
}
