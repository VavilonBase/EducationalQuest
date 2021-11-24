using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextureGenerator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Canvas _textureGeneratorCanvas;
    [SerializeField] private Camera _textureGeneratorCamera;

    private RawImage _textureGeneratorRawImage;
    private Text _textureGeneratorText;
    private Texture2D _texture;

    void Awake()
    {
        _textureGeneratorRawImage = _textureGeneratorCanvas.GetComponentInChildren<RawImage>();
        _textureGeneratorText = _textureGeneratorRawImage.GetComponentInChildren<Text>();
        _texture = new Texture2D(2048, 1024);
    }

    /// <summary>
    /// Генерирует текстуру на основе входных данных
    /// </summary>
    /// <param name="textAnchor">Положение текста</param>
    /// <param name="text">Сам текст</param>
    /// <param name="background">Задний фон</param>
    /// <param name="textUI">Cам текст, если нужен свой</param>
    /// <param name="rawImageUI">Cама картинка, если нужена своя</param>
    /// <param name="colorText">Цвет текста</param>
    /// <returns>Сгенерировнную текстуру, если rawImageUI == null и background == null, то функция вернет null</returns>
    public Texture2D GenerateTexture(Text textUI = null, RawImage rawImageUI = null, 
        TextAnchor textAnchor = TextAnchor.UpperCenter, string text = "", Texture2D background = null, Color colorText = default)
    {
        //Сохраняем старые значения
        RawImage oldRawImage = _textureGeneratorRawImage;
        Text oldText = _textureGeneratorText;
        Debug.Log(textUI);
        Debug.Log(rawImageUI);

        Debug.Log(textAnchor);

        Debug.Log(text);
        Debug.Log(background);
        Debug.Log(colorText);
        Debug.Log("--------------------------------");


        //Если передали саму картинку
        if (rawImageUI != null)
        {
            _textureGeneratorRawImage = rawImageUI;
        }
        else
        {
            //Если передали задний фон, то присваеваем его текстуре, иначе возвращаем null
            if (background != null)
            {
                _textureGeneratorRawImage.texture = background;
            }
            else
            {
                return null;
            }
            
        }

        //Если передали сам текст
        if (textUI != null)
        {
            _textureGeneratorText = textUI;
        }
        else
        {
            _textureGeneratorText.alignment = textAnchor;
            _textureGeneratorText.text = text;
            _textureGeneratorText.color = colorText == default ? Color.white : colorText;
        }

        //Запоминаю старую текстуру
        RenderTexture rndOld = RenderTexture.active;
        //Делаю холст активным
        _textureGeneratorCanvas.gameObject.SetActive(true);
        //Включаю камеру
        _textureGeneratorCamera.enabled = true;
        //Делаю рендер
        _textureGeneratorCamera.Render();
        //Делаю холст не активным
        _textureGeneratorCanvas.gameObject.SetActive(false);
        //Выключаю камеру
        _textureGeneratorCamera.enabled = false;
        //Задаю текстуру, которую срендерила камера
        RenderTexture.active = _textureGeneratorCamera.targetTexture;
        //Считываю текстуру, которую срендерила камера в текстуру 
        _texture.ReadPixels(new Rect(0, 0, _textureGeneratorCamera.targetTexture.width, _textureGeneratorCamera.targetTexture.height), 0, 0);
        //Создаю текстуру
        _texture.Apply();
        //Возвращаю обратно текстуру
        RenderTexture.active = rndOld;

        //Возвращаем старые значения
        _textureGeneratorRawImage = oldRawImage;
        _textureGeneratorText = oldText;

        //Возврат текстуры
        return _texture;
    }
}