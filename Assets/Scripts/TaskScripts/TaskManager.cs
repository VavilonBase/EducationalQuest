using System.Collections;
using UnityEngine;

namespace Assets.Scripts.TaskScripts
{
    public class TaskManager : MonoBehaviour
    {

        [Header("Components")]
        [SerializeField] private GameObject _taskAddUI;
        [SerializeField] private GameObject _taskListUI;

        /// <summary>
        /// Показывает окно с добавлением задания
        /// </summary>
        public void ViewTaskAdd()
        {
            CloseAllUI();
            _taskAddUI.SetActive(true);
        }

        /// <summary>
        /// Показывает окно со список заданий
        /// </summary>
        public void ViewTaskList()
        {
            CloseAllUI();
            _taskListUI.SetActive(true);
        }

        //------------ДОП. МЕТОДЫ-------------
        void CloseAllUI()
        {
            _taskAddUI.SetActive(false);
            _taskListUI.SetActive(false);
        }
    }
}