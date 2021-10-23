using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.TaskScripts
{
    public class TaskListManger : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TaskListView _listView;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Dropdown _directionDropdown;
        [SerializeField] private Button _addNewTaskBtn;

        [Header("Settings")]
        [SerializeField] private string _title = "Вопрос ";
        [SerializeField] public List<Texture2D> _textures;

        [Header("Sripts")]
        [SerializeField] private TaskManager _scriptTaskManager;

        private void Awake()
        {
            this.SetListenerDirectionDropdown();
            this.SetListenerAddNewTaskBtn();
            this.OnValueChangeDirectionDropdown();
        }

        //--------------МЕТОДЫ НАЗНАЧЕНИЯ СОБЫТИЙ------------------
        void SetListenerDirectionDropdown()
        {
            this._directionDropdown.onValueChanged.AddListener(delegate { OnValueChangeDirectionDropdown(); });
        }
        void SetListenerAddNewTaskBtn()
        {
            this._addNewTaskBtn.onClick.AddListener(this.ClickAddNewTaskBtn);
        }

        //--------------------СОБЫТИЯ------------------------------
        void OnValueChangeDirectionDropdown()
        {
            //Очищаем списки
            this._listView.ClearList();
            this.ClearTexturesList();

            //Получаем направление
            string direction = this._directionDropdown.options[_directionDropdown.value].text;
            //Переменная для хранения папки полученного направления
            string pathFolder = "";
            //Поиск папки полученного направления для генерации png, если ее нет, то создаем
            switch (direction)
            {
                case "Механика":
                    pathFolder = Application.dataPath + "/Resources/Mechanics";
                    //Если такой директории нет, то список пуст
                    if (!Directory.Exists(pathFolder)) return;
                    break;
                case "Молекулярная физика":
                    pathFolder = Application.dataPath + "/Resources/Molecular";
                    if (!Directory.Exists(pathFolder)) return;
                    break;
                case "Электричество":
                    pathFolder = Application.dataPath + "/Resources/Electricity";
                    if (!Directory.Exists(pathFolder)) return;
                    break;
                default:
                    break;
            }

            if (pathFolder != "")
            {
                //Получаем все файлы
                string[] allFiles = Directory.GetFiles(pathFolder);
                //Вместо файлов делаем числа из номера вопроса
                foreach (string file in allFiles)
                {
                    //Если файл кончается на .png
                    if (file.Substring(file.Length - 3, 3) == "png")
                    {
                        WWW www = new WWW("file://" + file);
                        _textures.Add(www.texture);
                    }
                }
            }

            this.CreateElements();
        }

        void ClickAddNewTaskBtn()
        {
            this._scriptTaskManager.ViewTaskAdd();
        }
        //------------------ДОП. МЕТОДЫ---------------------
        void ClearTexturesList()
        {
            while (_textures.Count != 0)
            {
                Texture2D texture = _textures.First();
                _textures.Remove(texture);
                Destroy(texture);
            }
        }

        void CreateElements()
        {
            for (int i = 0; i < _textures.Count; i++)
            {
                //Создаем новый элемент в списке по prefab
                GameObject element = this._listView.Add(this._prefab);
                //Получаем из него объект TaskListElementView
                TaskListElementView elementMeta = element.GetComponent<TaskListElementView>();
                //Заполняем содержимое элемента
                elementMeta.SetTitle(this._title + i);
                elementMeta.SetImage(_textures[i]);
            }
        }
    }
}