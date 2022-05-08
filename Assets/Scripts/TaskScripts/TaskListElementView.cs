using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.TaskScripts
{
    public class TaskListElementView : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private RectTransform _transform;
        [Space]
        [SerializeField] private RawImage _image;
        [Space]
        [SerializeField] private Text _title;
        [Space]
        [SerializeField] private Button _editBtn;
        [Space]
        [SerializeField] private Button _deleteBtn;
        [Space]
        [SerializeField] private MenuTeacherTasksEditor _parentManager;
        //private string _direction;
        private int _numberQuestion;
        private void Awake()
        {
            SetListenerButtons();
        }

        //-----------------МЕТОДЫ НАЗНАЧЕНИЯ СОБЫТИЙ--------------
        void SetListenerButtons()
        {
            //Назначаем событие для кнопки Далее
            Button editBtnComponent = _editBtn.GetComponent<Button>();
            editBtnComponent.onClick.AddListener(ClickEditBtn);
            //Назначаем событие для кнопки Далее
            Button deleteBtnComponent = _deleteBtn.GetComponent<Button>();
            deleteBtnComponent.onClick.AddListener(ClickDeleteBtn);
        }

        //------------------------СОБЫТИЯ-------------------------
        void ClickEditBtn()
        {
            _parentManager.EditTask(_numberQuestion);
        }

        void ClickDeleteBtn()
        {
            _parentManager.DeleteTask(_numberQuestion);
        }
        /// <summary>
        /// Возвращает ширину элемета
        /// </summary>
        /// <returns>Ширина элемента (float)</returns>
        public float Width() => _transform.rect.width;
        /// <summary>
        ///Возвращает высоту элемента
        /// </summary>
        /// <returns>Высота элемента (float)</returns>
        public float Height() => _transform.rect.height;

        /// <summary>
        /// Установка текста заголовка
        /// </summary>
        /// <param name="title"></param>
        public void SetTitle(string title) => _title.text = title;

        /// <summary>
        /// Установка картинки
        /// </summary>
        /// <param name="image"></param>
        public void SetImage(Texture2D texture) => _image.texture = texture;

        /// <summary>
        /// Установка номера вопроса
        /// </summary>
        /// <param name="numberQestion"></param>
        public void SetNumberQuestion(int numberQestion) => _numberQuestion = numberQestion;

        /// <summary>
        /// Получает главный скрипт родителя
        /// </summary>
        /// <param name="parentManager"></param>
        public void SetTaskManagerSript(MenuTeacherTasksEditor parentManager) => _parentManager = parentManager;
    }
}