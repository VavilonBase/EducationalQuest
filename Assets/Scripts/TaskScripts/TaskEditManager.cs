﻿using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class TaskEditManager : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] private GameObject _directionField; //Окно с выбором раздела
    [SerializeField] private GameObject _addQuestionField; // Окно с добавлением вопроса
    [SerializeField] private GameObject[] _answersField; //Окна с добавлением ответов 
    [Space]
    [Header("Buttons")]
    [SerializeField] private Button _nextBtn; //Кнопка Далее
    [SerializeField] private Button _backBtn; //Кнопка Назаж
    [SerializeField] private Button _editBtn; //Кнопка Добавить
    [SerializeField] private Button _taskListBtn; //Кнопка для перехода к списку заданий
    [Header("Components")]
    [SerializeField] private Text _errorText; // Текст с ошибкой
    [SerializeField] private Toggle[] _answerTrueToggles; //Переключатели выбора правильного ответа
    [Space]
    [Header("Scripts")]
    [SerializeField] private TaskManager _scriptTaskManager;

    private TaskFieldComponent _scriptQuestionField; //Скрипт для генерации вопроса 
    private TaskFieldComponent[] _scriptsAnswersFields; //Скрипт для генерации ответа
    private Dropdown _directionDropdown; //Выпадающее поле для выбора направления
    private int _numberQuestion; //Номер вопроса
    private byte _step = 0; //Этап на котором сейчас находится добавление

    // Start is called before the first frame update
    void Awake()
    {

        //Начальная инициализация
        //Получаем объекты поля Направление
        GetObjectsFromDirectionField();
        //Выключаем переключатели выбора правильного ответа
        OffAnswerTrueToggles();

        //Получения скрипта для генерации вопроса
        GetScriptFromQuestionAddField();

        //Получение скриптов для генерации ответов
        GetScriptsFromAnswersAddFields();

        //Присвоение событий
        SetListenerCommonAllField();
        SetListenerAnswerTrueToggles();

        //Скрываем все поля
        _directionField.SetActive(false);
        _addQuestionField.SetActive(false);
        HideAnswersField();
        _errorText.gameObject.SetActive(false);

        //Скрываем кнопку Добавить и Назад и показываем кнопки Далее
        _editBtn.gameObject.SetActive(false);
        _nextBtn.gameObject.SetActive(true);
        _backBtn.gameObject.SetActive(false);
        //Начальная перерисовка окон
        RedrawWindow();

    }

    //--------------------НАЧАЛЬНАЯ ИНИЦИАЛИЗАЦИЯ ДЛЯ ИЗМЕНЕНИЯ----------------------
    public void Initialized(Texture2D[] textures, string direction, int numberQuestion)
    {
        //Инициализируем TaskEditManager
        //Задаем направление и номер вопроса
        _directionDropdown.value = direction switch
        {
            "Механика" => 0,
            "Молекулярная физика" => 1,
            "Электричество" => 2,
            _ => 0,
        };
        //Задаем номер вопроса
        _numberQuestion = numberQuestion;
        //Выключаем переключатели Правильного ответа
        //Выключаю обработку события изменения значения переключателей
        OffListnerAnswerTrueToggles();
        //Сбрасываю все переключатели
        OffAnswerTrueToggles();
        //Включаю обработку события изменения значения переключателей
        SetListenerAnswerTrueToggles();
        //Инициализируем поля
        _scriptQuestionField.Initialized(textures[0]);
        byte i = 1;
        foreach (TaskFieldComponent _scriptAnswerField in _scriptsAnswersFields)
        {
            _scriptAnswerField.Initialized(textures[i]);
            i++;
        }
    }

    //---------------------МЕТОДЫ ПОЛУЧЕНИЯ ОБЪЕКТОВ ПОЛЕЙ---------------------------
    //Получение объектов из поля Направление
    void GetObjectsFromDirectionField()
    {
        //Получение направления
        _directionDropdown = _directionField.GetComponentInChildren<Dropdown>();
    }
    //Получение скриптов из поля добавления вопроса
    void GetScriptFromQuestionAddField()
    {
        _scriptQuestionField = _addQuestionField.GetComponent<TaskFieldComponent>();
    }

    //Получение скриптов из полей добавления ответа
    void GetScriptsFromAnswersAddFields()
    {
        _scriptsAnswersFields = new TaskFieldComponent[_answersField.Length];
        for (int i = 0; i < _answersField.Length; i++)
        {
            _scriptsAnswersFields[i] = _answersField[i].GetComponent<TaskFieldComponent>();
        }
    }

    //---------------------МЕТОДЫ НАЗНАЧЕНИЯ СОБЫТИЙ-------------------------------
    void SetListenerCommonAllField()
    {
        //Назначаем событие для кнопки Далее
        Button nextBtnComponent = _nextBtn.GetComponent<Button>();
        nextBtnComponent.onClick.AddListener(ClickNextBtn);

        //Назначаем событие для кнопки Назад
        Button backBtnComponent = _backBtn.GetComponent<Button>();
        backBtnComponent.onClick.AddListener(ClickBackBtn);

        //Назначаем событие для кнопки Добавить
        Button editBtnComponent = _editBtn.GetComponent<Button>();
        editBtnComponent.onClick.AddListener(ClickEditBtn);

        //Назначаем событие для кнопки К списку заданий
        Button taskListBtnComponent = _taskListBtn.GetComponent<Button>();
        taskListBtnComponent.onClick.AddListener(ClickTaskListBtn);
    }
    //Назначение событий для переключателей правильного ответа
    void SetListenerAnswerTrueToggles()
    {
        foreach (Toggle answerTrueToggle in _answerTrueToggles)
        {
            answerTrueToggle.onValueChanged.AddListener(delegate { ChangedValueAnswerTrueToggle(answerTrueToggle); });
        }
    }

    //----------------------УДАЛЕНИЕ СОБЫТИЙ------------------

    //Выключить обработку событий с переключателей правильного ответа
    void OffListnerAnswerTrueToggles()
    {
        foreach (Toggle answerTrueToggle in _answerTrueToggles)
        {
            answerTrueToggle.onValueChanged.RemoveAllListeners();
        }
    }


    //----------------------СОБЫТИЯ--------------------
    //Событие для кнопки Дальше
    void ClickNextBtn()
    {
        _step++;
        //Если этап больше чем кол-во ответов + 1 (не +2, так как отсчет с 0), то скрываем кнопку Далее и показываем кнопку Добавить
        if (_step >= (_answersField.Length + 1))
        {
            _nextBtn.gameObject.SetActive(false); //Скрываем кнопку Далее
            _editBtn.gameObject.SetActive(true); //Показываем кнопку Добавить
        }
        else
        {
            _editBtn.gameObject.SetActive(false); //Скрываем кнопку Добавить
        }
        //Если этап больше 0, то показываем кнопку Назад
        _backBtn.gameObject.SetActive(_step > 0);
        RedrawWindow();
    }

    //Событие для кнопки Назад
    void ClickBackBtn()
    {
        _step--;
        //Если этап меньше или равен 0, то скрываем кнопку Назад
        if (_step <= 0)
        {
            _backBtn.gameObject.SetActive(false);
        }
        //Если этап больше нуля, то показываем кнопку Вперед
        _nextBtn.gameObject.SetActive(_step >= 0);
        RedrawWindow();

    }

    //Событие для кнопки К списку заданий
    void ClickTaskListBtn()
    {
        ViewTaskList();
    }

    //Событие клика для кнопки Изменить
    void ClickEditBtn()
    {
        if (CheckExistTrueToggle())
        {
            //Скрываем текст с ошибкой
            HideErrorText();
            //Получаем направление
            string direction = _directionDropdown.options[_directionDropdown.value].text;
            //Получаем путь до папки нужного направления
            string pathFolder = GetDirectionDirectory(direction);
            //Если папка есть, то добавляем файлы
            if (pathFolder != "")
            {
                Debug.Log("hi");
                //Создаем путь до папки со следующем вопросом
                string pathAnswersFolder = pathFolder + "/Q" + _numberQuestion.ToString();
                //Создаем эту папку, если ее нет
                if (!Directory.Exists(pathAnswersFolder))
                {
                    ViewTaskList();
                    return;
                }
                //Генерируем png для вопроса
                _scriptQuestionField.GeneratePng(pathFolder + "/Q" + _numberQuestion.ToString() + ".png");
                //Генерируем png для ответов
                for (int i = 0; i < _scriptsAnswersFields.Length; i++)
                {
                    _scriptsAnswersFields[i].GeneratePng(pathAnswersFolder + "/answer" + (i + 1).ToString() + ".png");
                }
            }
            ViewTaskList();
        }
        else
        {
            //Выводим текст с ошибкой
            ViewErrorText("Выберите правильный ответ!");
        }

    }

    //Изменение значения переключателя правильного ответа
    void ChangedValueAnswerTrueToggle(Toggle toggle)
    {
        //Запоминаю текущее состояние переключателя
        bool isOn = toggle.isOn;
        //Выключаю обработку события изменения значения переключателей
        OffListnerAnswerTrueToggles();
        //Сбрасываю все переключатели
        OffAnswerTrueToggles();
        //Возвращаю переключателю его исходное состояние
        toggle.isOn = isOn;
        //Если выключатель выключили, показаываем ошибку, что надо выбрать правильный ответ
        if (!isOn)
        {
            //Выводим текст с ошибкой
            ViewErrorText("Выберите правильный ответ!");
        }
        else
        {
            //Иначе убираем текст с ошибкой
            HideErrorText();
        }
        //Включаю обработку события изменения значения переключателей
        SetListenerAnswerTrueToggles();
    }

    //---------------------ПЕРЕРИСОВКА-------------------
    //Перерисовка окна в зависомости от значения переменной _step
    void RedrawWindow()
    {
        switch (_step)
        {
            case 0:
                _directionField.SetActive(true);
                _addQuestionField.SetActive(false);
                break;
            case 1:
                _directionField.SetActive(false);
                _addQuestionField.SetActive(true);
                HideAnswersField();
                break;
            default: //Обработка полей с ответами
                _addQuestionField.SetActive(false);
                HideAnswersField();
                _answersField[_step - 2].SetActive(true);
                break;
        }
    }

    //--------------------ДОП. МЕТОДЫ------------------------
    //Сокрытие всех полей с ответами
    void HideAnswersField()
    {
        foreach (GameObject answerField in _answersField)
        {
            answerField.SetActive(false);
        }
    }

    //Убрать галочки с переключателей правильного ответа
    void OffAnswerTrueToggles()
    {
        foreach (Toggle answerTrueToggle in _answerTrueToggles)
        {
            answerTrueToggle.isOn = false;
        }
    }

    //Проверка стоит ли галочка вообще
    bool CheckExistTrueToggle()
    {
        foreach (Toggle answerTrueToggle in _answerTrueToggles)
        {
            if (answerTrueToggle.isOn)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Сброс всех полей
    /// </summary>
    void ResetAllFields()
    {
        //Сброс текущего этапа
        _step = 0;
        //Сброс directionField
        _directionDropdown.value = 0;

        //Сброс QuestionField
        _addQuestionField.gameObject.SetActive(true);
        _addQuestionField.GetComponent<TaskFieldComponent>().ResetField();
        _addQuestionField.gameObject.SetActive(false);

        //Сброс AnswersField
        foreach (GameObject _answerField in _answersField)
        {
            _answerField.gameObject.SetActive(true);
            _answerField.GetComponent<TaskFieldComponent>().ResetField();
            _answerField.gameObject.SetActive(false);
        }
        //Выключаю все переключатели Правильного ответа
        //Выключаю обработку события изменения значения переключателей
        OffListnerAnswerTrueToggles();
        //Сбрасываю все переключатели
        OffAnswerTrueToggles();
        //Включаю обработку события изменения значения переключателей
        SetListenerAnswerTrueToggles();

        //Скрываем кнопку Добавить и Назад и показываем кнопки Далее
        _editBtn.gameObject.SetActive(false);
        _nextBtn.gameObject.SetActive(true);
        _backBtn.gameObject.SetActive(false);

        //Перерисовка окна
        RedrawWindow();
    }

    /// <summary>
    /// Сбрасывает все поля и переходит к окну Список заданий
    /// </summary>
    void ViewTaskList()
    {
        ResetAllFields();
        this._scriptTaskManager.ViewTaskList();
    }

    /// <summary>
    /// Скрываем текст с ошибкой
    /// </summary>
    void HideErrorText()
    {
        _errorText.text = "";
        _errorText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Показываем текст с ошибкой
    /// </summary>
    /// <param name="errorText">Текст ошибки</param>
    void ViewErrorText(string errorText)
    {
        _errorText.text = errorText;
        _errorText.gameObject.SetActive(true);
    }

    /// <summary>
    /// По названию направления вычисляет есть ли для него папка и если нет создает ее
    /// </summary>
    /// <param name="direction">Направление</param>
    /// <returns>Возвращает путь до папки</returns>
    string GetDirectionDirectory(string direction)
    {
        string pathFolder = direction switch
        {
            "Механика" => Application.dataPath + "/Resources/Mechanics",
            "Молекулярная физика" => Application.dataPath + "/Resources/Molecular",
            "Электричество" => Application.dataPath + "/Resources/Electricity",
            _ => "",
        };
        if (!Directory.Exists(pathFolder)) return "";
        return pathFolder;
    }
}
