using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TaskAddManager : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] private GameObject _directionField; //Окно с выбором раздела
    [SerializeField] private TaskWindowComponent _addQuestionWindow; // Окно с добавлением вопроса
    [SerializeField] private TaskWindowComponent[] _addAnswersWindows; //Окна с добавлением ответов 
    [Space]
    [Header("Buttons")]
    [SerializeField] private Button _nextBtn; //Кнопка Далее
    [SerializeField] private Button _backBtn; //Кнопка Назаж
    [SerializeField] private Button _addBtn; //Кнопка Добавить
    [SerializeField] private Button _taskListBtn; //Кнопка для перехода к списку заданий
    [Header("Components")]
    [SerializeField] private Text _errorText; // Текст с ошибкой
    [SerializeField] private Toggle[] _answerTrueToggles; //Переключатели выбора правильного ответа
    [Space]
    [Header("Task Manager")]
    [SerializeField] private TaskManager _taskManager;

    private TaskBoardInformation _taskBoardInformation;
    private CsGlobals _globalOptions;
    private Dropdown _directionDropdown; //Выпадающее поле для выбора направления
    private byte _step = 0; //Этап на котором сейчас находится добавление

    // Start is called before the first frame update
    void Awake()
    {

        //Начальная инициализация
        //Получаем объекты поля Направление
        GetObjectsFromDirectionField();
        //Выключаем переключатели выбора правильного ответа
        OffAnswerTrueToggles();
        //Поиск глобального объекта
        _globalOptions = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;

        //Присвоение событий
        SetListenerCommonAllField();
        SetListenerAnswerTrueToggles();

        //Скрываем все поля
        _directionField.SetActive(false);
        _addQuestionWindow.SetInactive();
        HideAnswersField();
        _errorText.gameObject.SetActive(false);

        //Скрываем кнопку Добавить и показываем кнопки Далее и Назад
        _addBtn.gameObject.SetActive(false);
        _nextBtn.gameObject.SetActive(true);
        _backBtn.gameObject.SetActive(false);
        //Начальная перерисовка окон
        RedrawWindow();
       
    }

    //---------------------МЕТОДЫ ПОЛУЧЕНИЯ ОБЪЕКТОВ ПОЛЕЙ---------------------------
    //Получение объектов из поля Направление
    void GetObjectsFromDirectionField()
    {
        //Получение направления
        _directionDropdown = _directionField.GetComponentInChildren<Dropdown>();
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
        Button addBtnComponent = _addBtn.GetComponent<Button>();
        addBtnComponent.onClick.AddListener(ClickAddBtn);

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
        if (_step >= (_addAnswersWindows.Length + 1))
        {
            _nextBtn.gameObject.SetActive(false); //Скрываем кнопку Далее
            _addBtn.gameObject.SetActive(true); //Показываем кнопку Добавить
        }
        else
        {
            _addBtn.gameObject.SetActive(false); //Скрываем кнопку Добавить
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

    //Событие клика для кнопки Добавить
    void ClickAddBtn()
    {
        if (CheckExistTrueToggle())
        {
            //Получаем направление
            string direction = _directionDropdown.options[_directionDropdown.value].text;
            //Получаем путь до папки нужного направления
            string pathFolder = GetOrCreateDirectionDirectory(direction);
            //Если папка есть, то добавляем файлы
            if (pathFolder != "")
            {
                //Выясняем номера вопросов
                List<int> questionNumbersList = GetTasksNumbersListFromDirectory(pathFolder);

                int nextQuestionNumber = 1;//Номер следующего вопроса, в случае если вопросов не будет в папке, то это значит, что это первый вопрос

                //Смотрим, если ли вообще вопросы
                if (questionNumbersList.Count != 0)
                {
                    //Сортируем массив по возрастанию
                    questionNumbersList.Sort();
                    //Получаем номер следующего вопроса
                    nextQuestionNumber = questionNumbersList[questionNumbersList.Count - 1] + 1;
                }
                //Создаем путь до папки со следующем вопросом
                string pathAnswersFolder = pathFolder + "/Q" + nextQuestionNumber.ToString();
                //Создаем эту папку, если ее нет
                if (!Directory.Exists(pathAnswersFolder)) Directory.CreateDirectory(pathAnswersFolder);
                //Генерируем png для вопроса
                _addQuestionWindow.GeneratePng(_taskManager.textureGenerator, pathFolder + "/Q" + nextQuestionNumber.ToString() + ".png");
                //Генерируем png для ответов
                for (int i = 0; i < _addAnswersWindows.Length; i++)
                {
                   _addAnswersWindows[i].GeneratePng(_taskManager.textureGenerator, pathAnswersFolder + "/" + (i + 1).ToString() + ".png");
                }
            }
            byte rightAnswer = this.GetRightAnswer();
            if (this._taskBoardInformation != null)
            {
                if (!this._taskBoardInformation.AddQuestion(rightAnswer))
                {
                    Debug.Log("Нифига не добавилось!");
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
        switch (this._step)
        {
            case 0:
                _directionField.SetActive(true);
                _addQuestionWindow.SetInactive();
                break;
            case 1:
                _directionField.SetActive(false);
                _addQuestionWindow.SetActive();
                HideAnswersField();
                break;
            default: //Обработка полей с ответами
                _addQuestionWindow.SetInactive();
                HideAnswersField();
                _addAnswersWindows[_step - 2].SetActive();
                break;
        }
    }

    //--------------------ДОП. МЕТОДЫ------------------------
    //Сокрытие всех полей с ответами
    void HideAnswersField()
    {
        foreach (TaskWindowComponent _addAnswerWindow in _addAnswersWindows)
        {
            _addAnswerWindow.SetInactive();
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
    
    byte GetRightAnswer()
    {
        for (byte i = 0; i < this._answerTrueToggles.Length; i++)
        {
            if (this._answerTrueToggles[i].isOn)
            {
                return i;
            }
        }
        return 0;
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
        _addQuestionWindow.ResetWindow();
        
        //Сброс AnswersField
        foreach (TaskWindowComponent _addAnswerWindow in _addAnswersWindows)
        {
            _addAnswerWindow.ResetWindow();
        }
        //Выключаю все переключатели Правильного ответа
        //Выключаю обработку события изменения значения переключателей
        OffListnerAnswerTrueToggles();
        //Сбрасываю все переключатели
        OffAnswerTrueToggles();
        //Включаю обработку события изменения значения переключателей
        SetListenerAnswerTrueToggles();

        //Скрываем кнопку Добавить и Назад и показываем кнопки Далее
        _addBtn.gameObject.SetActive(false);
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
        this._taskManager.ViewTaskList();
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
    string GetOrCreateDirectionDirectory(string direction)
    {
        //Переменная для хранения папки полученного направления
        string pathFolder;
        //Поиск папки полученного направления для генерации png, если ее нет, то создаем
        switch (direction)
        {
            case "Механика":
                pathFolder = Application.dataPath + "/Resources/Mechanics";
                if (!Directory.Exists(pathFolder)) Directory.CreateDirectory(pathFolder);
                this._taskBoardInformation = this._globalOptions.boardsInfo[0];
                break;
            case "Молекулярная физика":
                pathFolder = Application.dataPath + "/Resources/Molecular";
                if (!Directory.Exists(pathFolder)) Directory.CreateDirectory(pathFolder);
                this._taskBoardInformation = this._globalOptions.boardsInfo[2];
                break;
            case "Электричество":
                pathFolder = Application.dataPath + "/Resources/Electricity";
                if (!Directory.Exists(pathFolder)) Directory.CreateDirectory(pathFolder);
                this._taskBoardInformation = this._globalOptions.boardsInfo[1];
                break;
            default:
                pathFolder = "";
                break;
        }
        return pathFolder;
    }

    /// <summary>
    /// Возврщает все номера заданий из директории
    /// </summary>
    /// <param name="pathFolder">Путь до директории с заданиями</param>
    /// <returns>Возвращает список номеров заданий для нужной директории</returns>
    List<int> GetTasksNumbersListFromDirectory(string pathFolder)
    {
        //Получаем все файлы
        string[] allFiles = Directory.GetFiles(pathFolder);
        List<int> questionNumbersList = new List<int>();
        //Вместо файлов делаем числа из номера вопроса
        foreach (string file in allFiles)
        {
            //Если файл кончается на .png
            if (file.Substring(file.Length - 3, 3) == "png")
            {
                //Поиск последнего вхождения обратного слеша \
                int indexSlash = file.LastIndexOf("\\", file.Length - 1, file.Length);
                //Поиск последнего вхождения точки .
                int indexDote = file.LastIndexOf(".", file.Length - 1, file.Length);
                //Поиск номера вопроса
                int numberQuestion = Convert.ToInt32(file.Substring(indexSlash + 2, indexDote - indexSlash - 2));
                questionNumbersList.Add(numberQuestion);
            }
        }
        return questionNumbersList;
    }
}
