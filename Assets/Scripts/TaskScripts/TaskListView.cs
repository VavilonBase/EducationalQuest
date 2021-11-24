using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.TaskScripts
{
    public class TaskListView : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Transform _transform;
        [SerializeField] private RectTransform _contectRectTransform;
        [SerializeField] private Text _listEmptyText;

        [Header("Settings")]
        [SerializeField] private List<GameObject> _elements;
        [SerializeField] private float _offset;

        /// <summary>
        /// Добавляет элемент prefab в список
        /// 
        /// (WARNING): Объект должен иметь компонент TaskListElementView
        /// </summary>
        /// <param name="element"></param>
        /// <returns>Ссылку на созданный объект</returns>
        public GameObject Add(GameObject element)
        {
            //Убираем текст _listEmptyText
            _listEmptyText.gameObject.SetActive(false);
            //Создаем элемент
            GameObject createdElement = Instantiate(element, this._transform);

            //Объявляем переменную компонента TaskListElementView elementMeta
            TaskListElementView elementMeta = createdElement.GetComponent<TaskListElementView>(); ;

            //Проверяем не пустой ли список
            if (this._elements.Count == 0)
            {
                //Добавляем созданный элемент в список
                this._elements.Add(createdElement);

                //Изменяеим высоту контента
                this.ChangeContentHeight(elementMeta.Height());

                return createdElement;
            }

            //Получаем последний созданный элемент
            GameObject lastElement = this._elements.Last();

            //Получаем позицию последнего созданного элемента
            Vector3 lastElementPosition = lastElement.transform.localPosition;

            //Вычисляем позицию нового элемента
            Vector3 newElementPosition = new Vector3
            {
                x = lastElementPosition.x,
                y = lastElementPosition.y - elementMeta.Height() - this._offset,
                z = lastElementPosition.z
            };

            //Устанавливаем новую позицию элементу
            createdElement.transform.localPosition = newElementPosition;

            //Добавляем элемент в список
            this._elements.Add(createdElement);

            //Изменяеим высоту контента
            this.ChangeContentHeight(elementMeta.Height());

            return createdElement;  
        }

        /// <summary>
        /// Удаляет все элементы из списка
        /// </summary>
        public void ClearList()
        {
            while (_elements.Count != 0)
            {
                GameObject element = _elements.First();
                _elements.Remove(element);
                Destroy(element);
            }
            //Изменяем высоту контента на 0
            this._contectRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
            _listEmptyText.gameObject.SetActive(true);
        }

        void ChangeContentHeight(float changeHeight)
        {
            //Получаем высоту контента
            float contentHeight = this._contectRectTransform.rect.height;

            //Вычисляем новую высоту контента
            contentHeight += this._offset + changeHeight;

            //Изменяем высоту контента
            this._contectRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentHeight);
        }
    }
}