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
    }
}