using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskAddManager : MonoBehaviour
{
    public Button nextBtn;
    public Button backBtn;
    public Button addBtn;

    public GameObject directionField; //Окно с выбором раздела
    public GameObject addQuestionField; // Окно с добавлением вопроса
    public GameObject[] answersField; //Окна с добавлением ответов 
    private byte step = 0; //Этап на котором сейчас находится добавление

    // Start is called before the first frame update
    void Start()
    {

        //Скрываем все поля
        directionField.SetActive(false);
        addQuestionField.SetActive(false);
        HideAnswersField();

        //Скрываем кнопку Добавить и показываем кнопки Далее и Назад
        addBtn.gameObject.SetActive(false);
        nextBtn.gameObject.SetActive(true);
        backBtn.gameObject.SetActive(false);

        //Присвоение событий
        SetListenerCommonAllField();

        //Начальная инициализация
        //Начальная перерисовка окон
        RedrawWindow();
       
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
        foreach(GameObject answerField in answersField)
        {
            answerField.SetActive(false);
        }
    }
}
