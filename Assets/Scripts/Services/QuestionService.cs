using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class QuestionService
{
    /// <summary>
    /// Создание вопроса
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_testId">Тест, к которому принадлежит вопрос</param>
    /// <param name="_isText">Вопрос текстовый?</param>
    /// <param name="quesiton">Если вопрос текстовый, то текст вопроса, иначе путь до вопроса</param>
    /// <param name="scores">Количество очков за вопрос</param>
    /// <returns>Вернет новый вопрос
    /// IncorrectTokenFormat
    /// AccessDenied
    /// NotFoundRequiredData
    /// TestNotExist
    /// UserIsNotCreatorTest
    /// CanNotLoadFile
    /// CanNotPublishFile
    /// CanNotCreateFolder
    /// </returns>
    [System.Obsolete]
    public async static Task<Response<Question>> createQuestion(string jwt, int _testId, bool _isText, string _question, int _scores)
    {
        // URL
        string url = "https://educationalquest.herokuapp.com/question/create";
        // Создаем данные формы
        var formData = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("testId", _testId.ToString()),
            new MultipartFormDataSection("isText", _isText ? "1" : "0"),
            new MultipartFormDataSection("scores", _scores.ToString()),
        };
        // Если вопрос текстовый
        if (_isText)
        {
            // Добавляем в форму текстовый вопрос
            formData.Add(new MultipartFormDataSection("question", _question));
        }
        else
        {
            // Если вопрос ввиде файла
            // Получаем вопрос ввиде картинки
            WWW www = new WWW("file://" + _question);
            if (!string.IsNullOrEmpty(www.error))
            {
                return new Response<Question>() { data = null, isError = true, message = Message.CanNotLoadFile };
            }
            Texture2D texture = www.texture;
            // Кодируем его в байты
            byte[] questionBytes = texture.EncodeToPNG();
            // Добавляем в форму картинку
            formData.Add(new MultipartFormFileSection("question", questionBytes, "Question.jpg", "image/jpg"));
        }


        // Инициализируем соединение
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Добавляем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header(){name = "Authorization", value="Bearer " + jwt }
        };
        // Отправляем запрос
        return await httpClient.PostMultipart<Question>(formData, url);
    }

    /// <summary>
    /// Обновление вопроса
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_questionId">ID вопроса</param>
    /// <param name="_isText">Вопрос текстовый?</param>
    /// <param name="quesiton">Если вопрос текстовый, то текст вопроса, иначе путь до вопроса</param>
    /// <param name="scores">Количество очков за вопрос</param>
    /// <returns>Возвращает вопрос
    /// IncorrectTokenFormat
    /// AccessDenied
    /// NotFoundRequiredData
    /// TestNotExist
    /// UserIsNotCreatorTest
    /// CanNotLoadFile
    /// CanNotPublishFile
    /// CanNotCreateFolder
    /// </returns>
    [System.Obsolete]
    public async static Task<Response<Question>> updateQuestion(string jwt, int _questionId, bool _isText, string _question, int _scores)
    {
        // URL
        string url = "https://educationalquest.herokuapp.com/question/update";
        // Создаем данные формы
        var formData = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("questionId", _questionId.ToString()),
            new MultipartFormDataSection("isText", _isText ? "1" : "0"),
            new MultipartFormDataSection("scores", _scores.ToString()),
        };
        // Если вопрос текстовый
        if (_isText)
        {
            // Добавляем в форму текстовый вопрос
            formData.Add(new MultipartFormDataSection("question", _question));
        }
        else
        {
            // Если вопрос ввиде файла
            // Получаем вопрос ввиде картинки
            WWW www = new WWW("file://" + _question);
            if (!string.IsNullOrEmpty(www.error))
            {
                return new Response<Question>() { data = null, isError = true, message = Message.CanNotLoadFile };
            }
            Texture2D texture = www.texture;
            // Кодируем его в байты
            byte[] questionBytes = texture.EncodeToPNG();
            // Добавляем в форму картинку
            formData.Add(new MultipartFormFileSection("question", questionBytes, "Question.jpg", "image/jpg"));
        }

        // Инициализируем соединение
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Добавляем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header(){name = "Authorization", value="Bearer " + jwt }
        };
        // Отправляем запрос
        return await httpClient.PostMultipart<Question>(formData, url);
    }

    /// <summary>
    /// Удаление вопроса
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_questionId">Id вопроса</param>
    /// <returns>Возвращает true, если все хорошо, или null, если все плохо
    /// IncorrectTokenFormat
    /// AccessDenied
    /// NotFoundRequiredData
    /// NotFoundQuestion
    /// TestNotExist
    /// UserIsNotCreatorTest
    /// CanNotDeleteFileOrFolder
    /// </returns>
    public async static Task<Response<object>> delete(string jwt, int _questionId)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/question/delete?questionId=" + _questionId;
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

    /// <summary>
    /// Получение вопроса с ответами
    /// </summary>
    /// <param name="jwt">Токен</param>
    /// <param name="_questionId">ID вопроса</param>
    /// <returns>Вопрос с ответами
    /// IncorrectTokenFormat
    /// AccessDenied
    /// NotFoundRequiredData
    /// TestNotExist
    /// </returns>
    public async static Task<Response<ResponseQuestionWithAnswers>> getQuestionWithAnswers(string jwt, int _questionId)
    {
        // Задаем URL
        string url = "https://educationalquest.herokuapp.com/question/getQuestionWithAnswers?questionId=" + _questionId;
        // Инициализируем http client
        var httpClient = new HttpClient(new JsonSerializationOption());
        // Устанавливаем заголовки
        httpClient.headers = new List<Header>()
        {
            new Header() {name = "Authorization", value=$"Bearer {jwt}"}
        };
        // Отправляем запрос и ждем ответа
        var result = await httpClient.Get<ResponseQuestionWithAnswers>(url);
        return result;
    }
}
