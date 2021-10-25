using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class TaskGenerateImage : MonoBehaviour
{
    [Header("Settings")]
    public Texture2D baseBackgroundTexture; //Основная текстура заднего фона
    [Space]
    public TextAnchor textAnchor; //Переменная, которая при генерации изображения указывает положение текста
    [Space]
    public Color textColor; //Переменная, которая при генерации изображения указывает цвет текста
    [Space]
    [Header("Scripts")]
    [SerializeField] private TextureGenerator _scriptTextureGenerator; //Скрипт для генерации текстуры в случае текстового ввода

    private Texture2D _texture; //Основная текстура
    private string _pathBackground; //Путь до картинки заднего фона
    private string _path; //Переменная для внутренней работы срипта (ее использую всегда, когда нужен путь)
    private Texture2D _baseBackgroundTexture; //Основная текстура заднего фона
    private GameObject _field; //Поле с которым работает данный скрипт
    private Button _openExplorerBtn; //Кнопка открытия проводника
    private InputField _input; //Поля ввода текста
    private RawImage _image; //Картинка результата
    private Text _text; //Текст результата
    private Toggle _toggle; //Переключатель выбора ввода через текст или картинка
    void Start()
    {
        //Получение gameObject текущего объекта
        _field = this.gameObject;

        //Создание текстуры размером 500x500
        _texture = new Texture2D(2048, 1024);

        //Получение всех UI-объектов
        _toggle = _field.GetComponentInChildren<Toggle>();
        _input = _field.GetComponentInChildren<InputField>();
        _image = _field.GetComponentInChildren<RawImage>();
        _text = _image.gameObject.GetComponentInChildren<Text>();
        _openExplorerBtn = _field.GetComponentInChildren<Button>();

        //Создание обработчиков
        //Событие клика на кнопку открытие проводника
        _openExplorerBtn.onClick.AddListener(OpenExplorer);
        //Событие изменения переключателя Ввода текста или выбора картинки
        _toggle.onValueChanged.AddListener( delegate { ChangeValueToggle(); });
        //Событие изменения текста в input
        _input.onValueChanged.AddListener(delegate { ChangeValueInputField(); });

        //Начальная инициализация
        //Вызов события переключения переключателя Ввода текста или выбора картинки для начального показа/скрытия окон
        ChangeValueToggle();
        //Постановка базовой текстуры
        SetBaseTexture();
    }


    //---------------СОБЫТИЯ-----------------
    //Открывает проводник и при выборе картинки, обновляет изображение
    void OpenExplorer()
    {
        OpenFileName openFileName = new OpenFileName();
        if (LocalDialog.GetOpenFileName(openFileName))
        {
            _path = openFileName.file;
        };
        GetImage();
    }

    //Событие переключения переключателя Ввода текста или выбора картинки
    void ChangeValueToggle()
    {
        //Если стоит галочка Ввести вопрос текстом
        _input.gameObject.SetActive(_toggle.isOn); //Показываем/скрываем поле ввода
        _text.gameObject.SetActive(_toggle.isOn); // Показываем/скрываем поле ввода
        _openExplorerBtn.gameObject.SetActive(!_toggle.isOn); //Показываем/скрываем кнопку открытия проводника
        //Если переключатель стоит, то надо получить базовую текстуру
        if (_toggle.isOn)
        {
            SetBaseTexture();
        }
    }
    
    //Событие изменения текста в input
    //Функция делает следующее:
    //1. Текст text на картинке image меняет на тот, который в InputField
    //2. Текст renderText на картинке для генерации renderImage так же меняется на тот, который в InputField
    void ChangeValueInputField()
    {
        _text.text = _input.text;
    }
    //-------------------ДОП. МЕТОДЫ--------------------------
    //Постановка базовой текстуры
    void SetBaseTexture()
    {
        _texture = baseBackgroundTexture;
        UpdateImage();
    }
    //Если есть пусть до картинки, получает ее в виде текстуры
    void GetImage()
    {
        if (_path != null)
        {
            WWW www = new WWW("file://" + _path);
            _texture = www.texture;
            UpdateImage();
        }
    }

    //Если есть текстура, то обновляет текстуру избражения
    void UpdateImage()
    {
        if (_texture != null)
        {
            _image.texture = _texture;
        }
    }

    public void GeneratePng(string fileSave)
    {
        //Смотрим, надо ли генерировать текстуру или нет
        if (_toggle.isOn)
        {
            //Генерируем текстуру
            _texture = _scriptTextureGenerator.GenerateTexture(textAnchor: textAnchor, text: _text.text,
                background: baseBackgroundTexture, colorText: textColor);
        }
        if (_texture != null)
        {
            byte[] pngBytes = _texture.EncodeToPNG();
            File.WriteAllBytes(fileSave, pngBytes);
        }
    }
   }
