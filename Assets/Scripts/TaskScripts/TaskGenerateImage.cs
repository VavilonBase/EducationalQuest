using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class TaskGenerateImage : MonoBehaviour
{
    private Texture2D _texture; //Основная текстура

    public string pathBackgroundFromAssets = "/Materials/Materials/Materials2/background_question.png"; //Путь до картинки заднего фона от папки Assets
    private string _pathBackground; //Путь до картинки заднего фона
    private string _path; //Переменная для внутренней работы срипта (ее использую всегда, когда нужен путь)

    private GameObject _field; //Поле с которым работает данный скрипт
    
    private Button _openExplorerBtn; //Кнопка открытия проводника
    private InputField _input; //Поля ввода текста
    private RawImage _image; //Картинка результата
    private Text _text; //Текст результата
    private Toggle _toggle; //Переключатель выбора ввода через текст или картинка
    public RawImage renderImage; //Картинка для генерации в случае ввода через текст
    private Text _renderText; //Текст для генерации в случае ввода через текст
    void Start()
    {
        //Получение gameObject текущего объекта
        _field = this.gameObject;
        //Инициализация поля пути до файла с задним фоном
        _pathBackground = Application.dataPath + pathBackgroundFromAssets;

        //Создание текстуры размером 500x500
        _texture = new Texture2D(500, 500);

        //Получение всех UI-объектов
        _toggle = _field.GetComponentInChildren<Toggle>();
        _input = _field.GetComponentInChildren<InputField>();
        _image = _field.GetComponentInChildren<RawImage>();
        _text = _image.gameObject.GetComponentInChildren<Text>();
        _openExplorerBtn = _field.GetComponentInChildren<Button>();
        _renderText = renderImage.gameObject.GetComponentInChildren<Text>();

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
        //Получение базовой текстуры
        GetBaseTexture();
        //Присвоение базовой текстуры renderImage
        renderImage.texture = _texture;
    }


    //---------------СОБЫТИЯ-----------------
    //Открывает проводник и при выборе картинки, обновляет изображение
    void OpenExplorer()
    {
        _path = EditorUtility.OpenFilePanel("Выберите картинку", "", "png");
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
            GetBaseTexture();
        }
    }
    
    //Событие изменения текста в input
    //Функция делает следующее:
    //1. Текст text на картинке image меняет на тот, который в InputField
    //2. Текст renderText на картинке для генерации renderImage так же меняется на тот, который в InputField
    void ChangeValueInputField()
    {
        _text.text = _input.text;
        _renderText.text = _input.text;
    }
    //-------------------ДОП. МЕТОДЫ--------------------------
    //Возвращает исходную структуру
    void GetBaseTexture()
    {
        _path = _pathBackground;
        GetImage();
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
   }
