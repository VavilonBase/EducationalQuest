using System.Threading.Tasks;

public class Question
{
    public int questionId { get; set; } //Идентификатор вопроса
    public int testId { get; set; } // Идентификатор теста
    public string question { get; set; } // Сам вопрос, либо ссылка на него в виде фотографии
    public bool isText { get; set; } // Вопрос является текстом
    public int scores { get; set; } // Очки за вопрос

    /// <summary>
    /// Получение картинки с вопросом
    /// </summary>
    /// <returns>Если вопрос является картинкой, то вернется картинка, иначе вернется null</returns>
    public async Task<Response<UnityEngine.Texture2D>> GetTexture()
    {
        if (!isText)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetTexture(question);
            return response;
        }
        return null;
    }

    public override string ToString()
    {
        return $"ID: {questionId}\n" +
            $"testID: {testId}\n" +
            $"question: {question}\n" +
            $"isText: {isText}\n" +
            $"scores: {scores}";
    }
}