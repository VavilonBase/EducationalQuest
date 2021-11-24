using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TaskWindowComponent : MonoBehaviour
{
    [Header("Settings")]
    public Texture2D baseBackgroundTexture; //Основная текстура заднего фона
    [Space]
    public TextAnchor textAnchor; //Переменная, которая при генерации изображения указывает положение текста
    [Space]
    public Color textColor; //Переменная, которая при генерации изображения указывает цвет текста

    private Texture2D _texture; //Основная текстура
    private string _path; //Переменная для внутренней работы срипта (ее использую всегда, когда нужен путь)
    private GameObject _field; //Поле с которым работает данный скрипт
    private Button _openExplorerBtn; //Кнопка открытия проводника
    private InputField _input; //Поля ввода текста
    private RawImage _image; //Картинка результата
    private Text _text; //Текст результата
    private Toggle _toggle; //Переключатель выбора ввода через текст или картинка

    public bool ToggleIsOn { 
        get { 
            return this._toggle.isOn; 
        }
        set { 
            this._toggle.isOn = value; 
        } 
    }

    public Texture2D Texture
    {
        get { 
            return this._texture == null ? this.baseBackgroundTexture : this._texture; 
        }
        set {
            this._texture = value;
            this._image.texture = this.Texture;
        }
    }
    void Awake()
    {
        //Получение gameObject текущего объекта
        this._field = this.gameObject;

        //Создание текстуры размером 2048, 1024
        this._texture = new Texture2D(2048, 1024);

        //Получение всех UI-объектов
        this._toggle = this._field.GetComponentInChildren<Toggle>();
        this._input = this._field.GetComponentInChildren<InputField>();
        this._image = this._field.GetComponentInChildren<RawImage>();
        this._text = this._image.gameObject.GetComponentInChildren<Text>();
        this._openExplorerBtn = this._field.GetComponentInChildren<Button>();
        //Создание обработчиков
        //Событие клика на кнопку открытие проводника
        this._openExplorerBtn.onClick.AddListener(this.OpenExplorer);
        //Событие изменения переключателя Ввода текста или выбора картинки
        this._toggle.onValueChanged.AddListener( delegate { this.ChangeValueToggle(); });
        //Событие изменения текста в input
        this._input.onValueChanged.AddListener(delegate { this.ChangeValueInputField(); });

        //Начальная инициализация
        //Вызов события переключения переключателя Ввода текста или выбора картинки для начального показа/скрытия окон
        this.ChangeValueToggle();
        //Постановка базовой текстуры
        this.SetBaseTexture();
    }


    //-------------МЕТОД ИНИЦИАЛИЗАЦИИ В СЛУЧАЕ ИЗМЕНЕНИЯ ЗАДАНИЯ---------------------
    public void Initialized(Texture2D texture)
    {
        //Делаем окно активным
        this.SetActive();
        //Сбрасываю переключатель Ввода текста
        this.ToggleIsOn = false;
        //Задаем текстуру картинки
        this.Texture = texture;
        //Делаем окно не активным
        this.SetInactive();
    }

    //---------------СОБЫТИЯ-----------------
    //Открывает проводник и при выборе картинки, обновляет изображение
    void OpenExplorer()
    {
        OpenFileName openFileName = new OpenFileName();
        if (LocalDialog.GetOpenFileName(openFileName))
        {
            this._path = openFileName.file;
        };
        this.GetImage();
    }

    //Событие переключения переключателя Ввода текста или выбора картинки
    void ChangeValueToggle()
    {
        //Если стоит галочка Ввести вопрос текстом
        this._input.gameObject.SetActive(this.ToggleIsOn); //Показываем/скрываем поле ввода
        this._text.gameObject.SetActive(this.ToggleIsOn); // Показываем/скрываем поле ввода
        this._openExplorerBtn.gameObject.SetActive(!this.ToggleIsOn); //Показываем/скрываем кнопку открытия проводника
        //Если переключатель стоит, то надо получить базовую текстуру
        if (this.ToggleIsOn)
        {
            this.SetBaseTexture();
        }
    }
    
    //Событие изменения текста в input
    //Функция делает следующее:
    //1. Текст text на картинке image меняет на тот, который в InputField
    //2. Текст renderText на картинке для генерации renderImage так же меняется на тот, который в InputField
    void ChangeValueInputField()
    {
        this._text.text = this._input.text;
    }
    //-------------------ДОП. МЕТОДЫ--------------------------
    //Постановка базовой текстуры
    void SetBaseTexture()
    {
        this.Texture = this.baseBackgroundTexture;
    }
    //Если есть пусть до картинки, получает ее в виде текстуры
    void GetImage()
    {
        if (this._path != null)
        {
            WWW www = new WWW("file://" + this._path);
            this.Texture = www.texture;
        }
    }

    /// <summary>
    /// Генерирует png картинку на основен текстуры, или текста, или текстуры и текста
    /// </summary>
    /// <param name="fileSave">Путь и название файла в который необходимо сохранить картинку, если такого файла не существует, он создается автоматически</param>
    public void GeneratePng(TextureGenerator textureGenerator, string fileSave)
    {
        Texture2D texture = this.Texture != null ? this._texture : this.baseBackgroundTexture;
        //Генерируем текстуру
        this._texture = textureGenerator.GenerateTexture(textAnchor: textAnchor, text: _text.text,
        background: texture, colorText: textColor);
       
        
        byte[] pngBytes = this._texture.EncodeToPNG();
        File.WriteAllBytes(fileSave, pngBytes);
        
    }

    /// <summary>
    /// Сбрасывает поле к исходному состоянию
    /// </summary>
    public void ResetWindow()
    {
        this.SetActive();
        //Сбрасываем переключатель Ввода текстом, при этом вызовется событие при переключении
        this._toggle.isOn = false;
        //Удаляем текст с input, при этом вызовется событие при смене текста в input
        this._input.text = "";
        //Поменять картинку на базовую
        this.SetBaseTexture();
        this.SetInactive();
    }

    /// <summary>
    /// Отображает окно
    /// </summary>
    public void SetActive()
    {
        this.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// Скрывает окно
    /// </summary>
    public void SetInactive()
    {
        this.gameObject.SetActive(false);   
    }
}
