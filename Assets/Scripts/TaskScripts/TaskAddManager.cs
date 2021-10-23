using Assets.Scripts.TaskScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TaskAddManager : MonoBehaviour
{
    [Header("Components")]
    public Button nextBtn; //Кнопка Далее
    public Button backBtn; //Кнопка Назаж
    public Button addBtn; //Кнопка Добавить
    public Text errorText; // Текст с ошибкой
    [Space]
    public GameObject directionField; //Окно с выбором раздела
    public GameObject addQuestionField; // Окно с добавлением вопроса
    public GameObject[] answersField; //Окна с добавлением ответов 
    public Toggle[] answerTrueToggles; //Переключатели выбора правильного ответа
    private Dropdown _directionDropdown; //Выпадающее поле для выбора направления
    [Space]
    [Header("Scripts")]
    [SerializeField] private TaskManager _scriptTaskManager;
    private TaskGenerateImage _scriptGenerateQuestionImage; //Скрипт для генерации вопроса 
    private TaskGenerateImage[] _scriptsGenerateAnswerImage; //Скрипт для генерации ответа
    private byte step = 0; //Этап на котором сейчас находится добавление

    // Start is called before the first frame update
    void Start()
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
        directionField.SetActive(false);
        addQuestionField.SetActive(false);
        HideAnswersField();
        errorText.gameObject.SetActive(false);

        //Скрываем кнопку Добавить и показываем кнопки Далее и Назад
        addBtn.gameObject.SetActive(false);
        nextBtn.gameObject.SetActive(true);
        backBtn.gameObject.SetActive(false);
        //Начальная перерисовка окон
        RedrawWindow();
       
    }

    //---------------------МЕТОДЫ ПОЛУЧЕНИЯ ОБЪЕКТОВ ПОЛЕЙ---------------------------
    //Получение объектов из поля Направление
    void GetObjectsFromDirectionField()
    {
        //Получение направления
        _directionDropdown = directionField.GetComponentInChildren<Dropdown>();
    }
    //Получение скриптов из поля добавления вопроса
    void GetScriptFromQuestionAddField()
    {
        _scriptGenerateQuestionImage = addQuestionField.GetComponent<TaskGenerateImage>();
    }

    //Получение скриптов из полей добавления ответа
    void GetScriptsFromAnswersAddFields()
    {
        _scriptsGenerateAnswerImage = new TaskGenerateImage[answersField.Length];
        for (int i = 0; i < answersField.Length; i++)
        {
            _scriptsGenerateAnswerImage[i] = answersField[i].GetComponent<TaskGenerateImage>();
        }
    }

    //---------------------МЕТОДЫ НАЗНАЧЕНИЯ СОБЫТИЙ-------------------------------
    void SetListenerCommonAllField()
    {
        //Назначаем событие для кнопки Далее
        Button nextBtnComponent = nextBtn.GetComponent<Button>();
        nextBtnComponent.onClick.AddListener(ClickNextBtn);

        //Назначаем событие для кнопки Назад
        Button backBtnComponent = backBtn.GetComponent<Button>();
        backBtnComponent.onClick.AddListener(ClickBackBtn);

        //Назначаем событие для кнопки Добавить
        Button addBtnComponent = addBtn.GetComponent<Button>();
        addBtnComponent.onClick.AddListener(ClickAddBtn);
    }
    
    //----------------------НАЗНАЧЕНИЕ И И УДАЛЕНИЕ СОБЫТИЙ------------------
    //Назначение событий для переключателей правильного ответа
    void SetListenerAnswerTrueToggles()
    {
        foreach (Toggle answerTrueToggle in answerTrueToggles)
        {
            answerTrueToggle.onValueChanged.AddListener(delegate { ChangedValueAnswerTrueToggle(answerTrueToggle); });
        }
    }

    //Выключить обработку событий с переключателей правильного ответа
    void OffListnerAnswerTrueToggles()
    {
        foreach (Toggle answerTrueToggle in answerTrueToggles)
        {
            answerTrueToggle.onValueChanged.RemoveAllListeners();
        }
    }

    //----------------------СОБЫТИЯ--------------------
    //Событие для кнопки Дальше
    void ClickNextBtn()
    {
        step++;
        //Если этап больше чем кол-во ответов + 1 (не +2, так как отсчет с 0), то скрываем кнопку Далее и показываем кнопку Добавить
        if (step >= (answersField.Length + 1))
        {
            nextBtn.gameObject.SetActive(false); //Скрываем кнопку Далее
            addBtn.gameObject.SetActive(true); //Показываем кнопку Добавить
        }
        else
        {
            addBtn.gameObject.SetActive(false); //Скрываем кнопку Добавить
        }
        //Если этап больше 0, то показываем кнопку Назад
        backBtn.gameObject.SetActive(step > 0);
        RedrawWindow();
    }

    //Событие для кнопки Назад
    void ClickBackBtn()
    {
        step--;
        //Если этап меньше или равен 0, то скрываем кнопку Назад
        if (step <= 0)
        {
            backBtn.gameObject.SetActive(false);
        }
        //Если этап больше нуля, то показываем кнопку Вперед
        nextBtn.gameObject.SetActive(step >= 0);
        RedrawWindow();
        
    }

    //Событие клика для кнопки Добавить
    void ClickAddBtn()
    {
        if (CheckExistTrueToggle())
        {
            //Скрываем текст с ошибкой
            errorText.text = "";
            errorText.gameObject.SetActive(false);
            //Получаем направление
            string direction = _directionDropdown.options[_directionDropdown.value].text;
            //Переменная для хранения папки полученного направления
            string pathFolder = "";
            //Поиск папки полученного направления для генерации png, если ее нет, то создаем
            switch (direction)
            {
                case "Механика":
                    pathFolder = Application.dataPath + "/Resources/Mechanics";
                    if (!Directory.Exists(pathFolder)) Directory.CreateDirectory(pathFolder);
                    break;
                case "Молекулярная физика":
                    pathFolder = Application.dataPath + "/Resources/Molecular";
                    if (!Directory.Exists(pathFolder)) Directory.CreateDirectory(pathFolder);
                    break;
                case "Электричество":
                    pathFolder = Application.dataPath + "/Resources/Electricity";
                    if (!Directory.Exists(pathFolder)) Directory.CreateDirectory(pathFolder);
                    break;
                default:
                    break;
            }
            //Если папка есть, то добавляем файлы
            if (pathFolder != "")
            {
                //Выясняем номер вопроса
                //Получаем все файлы
                string[] allFiles = Directory.GetFiles(pathFolder);
                List<int> questionNumbers = new List<int>();
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
                        questionNumbers.Add(Convert.ToInt32(file.Substring(indexSlash + 2, indexDote - indexSlash - 2)));
                    }
                }
                int nextQuestionNumber = 1;//Номер следующего вопроса, в случае если вопросов не будет в папке, то это значит, что это первый вопрос
                                           //Смотрим, если ли вообще вопросы
                if (questionNumbers.Count != 0)
                {
                    //Сортируем массив по возрастанию
                    questionNumbers.Sort();
                    //Получаем номер следующего вопроса
                    nextQuestionNumber = questionNumbers[questionNumbers.Count - 1] + 1;
                }
                //Создаем путь до папки со следующем вопросом
                string pathAnswersFolder = pathFolder + "/Q" + nextQuestionNumber.ToString();
                //Создаем эту папку, если ее нет
                if (!Directory.Exists(pathAnswersFolder)) Directory.CreateDirectory(pathAnswersFolder);
                //Генерируем png для вопроса
                _scriptGenerateQuestionImage.GeneratePng(pathFolder + "/Q" + nextQuestionNumber.ToString() + ".png");
                //Генерируем png для ответов
                for (int i = 0; i < _scriptsGenerateAnswerImage.Length; i++)
                {
                    _scriptsGenerateAnswerImage[i].GeneratePng(pathAnswersFolder + "/answer" + (i + 1).ToString() + ".png");
                }
            }
            this._scriptTaskManager.ViewTaskList();
        }
        else
        {
            errorText.text = "Выберите правильный ответ!";
            errorText.gameObject.SetActive(true);
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
            errorText.text = "Выберите правильный ответ!";
            errorText.gameObject.SetActive(true);
        }
        else
        {
            //Иначе убираем текст с ошибкой
            errorText.text = "";
            errorText.gameObject.SetActive(false);
        }
        //Включаю обработку события изменения значения переключателей
        SetListenerAnswerTrueToggles();
    }

    //---------------------ПЕРЕРИСОВКА-------------------
    //Перерисовка окна в зависомости от значения переменной step
    void RedrawWindow()
    {
        switch (step)
        {
            case 0:
                directionField.SetActive(true);
                addQuestionField.SetActive(false);
                break;
            case 1:
                directionField.SetActive(false);
                addQuestionField.SetActive(true);
                HideAnswersField();
                break;
            default: //Обработка полей с ответами
                    addQuestionField.SetActive(false);
                    HideAnswersField();
                    answersField[step - 2].SetActive(true);
                break;
        }
    }

    //--------------------ДОП. МЕТОДЫ------------------------
    //Сокрытие всех полей с ответами
    void HideAnswersField()
    {
        foreach (GameObject answerField in answersField)
        {
            answerField.SetActive(false);
        }
    }
    
    //Убрать галочки с переключателей правильного ответа
    void OffAnswerTrueToggles()
    {
        foreach (Toggle answerTrueToggle in answerTrueToggles)
        {
            answerTrueToggle.isOn = false;
        }
    }
    
    //Проверка стоит ли галочка вообще
    bool CheckExistTrueToggle()
    {
        foreach (Toggle answerTrueToggle in answerTrueToggles)
        {
            if (answerTrueToggle.isOn)
            {
                return true;
            }
        }
        return false;
    }
}
