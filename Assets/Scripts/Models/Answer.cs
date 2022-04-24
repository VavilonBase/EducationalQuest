public class Answer
{
    public int answerId { get; set; } // Идентификатор ответа
    public int questionId { get; set; } // Идентификатор вопроса
    public string answer { get; set; } // Сам ответ, либо ссылка на него в виде фотографии
    public bool isText { get; set; } // Ответ является текстом, или фотографией
    public bool isRightAnswer { get; set; } // Ответ является правильным

    /// <summary>
    /// Получение картинки с ответом
    /// </summary>
    /// <returns>Если ответ является картинкой, то вернется картинка, иначе вернется null</returns>
    public async System.Threading.Tasks.Task<UnityEngine.Texture2D> GetTexture()
    {
        if (!isText)
        {
            var httpClient = new HttpClient();
            UnityEngine.Texture2D texture = await httpClient.GetTexture(answer);
            return texture;
        }
        return null;
    }
}