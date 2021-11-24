using Assets.Scripts.TaskScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
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

    [Header("Task Manager")]
    [SerializeField] private TaskManager _taskManager;

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
        RedrawList();
    }

    void ClickAddNewTaskBtn()
    {
        this._taskManager.ViewTaskAdd();
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

    void CreateElements(string direction)
    {
        for (int i = 0; i < _textures.Count; i++)
        {
            //Создаем новый элемент в списке по prefab
            GameObject element = this._listView.Add(this._prefab);
            //Получаем из него объект TaskListElementView
            TaskListElementView elementMeta = element.GetComponent<TaskListElementView>();
            //Заполняем содержимое элемента
            elementMeta.SetTitle(this._title + (i + 1).ToString());
            elementMeta.SetImage(_textures[i]);
            elementMeta.SetDirection(direction);
            elementMeta.SetNumberQuestion(i + 1);
            elementMeta.SetTaskManagerSript(this.gameObject.GetComponent<TaskListManger>());
        }
    }

    /// <summary>
    /// Заполняет поля при изменении вопроса
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="numberQuestion"></param>
    public void EditTask(string direction, int numberQuestion)
    {
        List<Texture2D> texturesList = new List<Texture2D>();
        //Поиск папки полученного направления для генерации png
        string pathFolder = direction switch
        {
            "Механика" => Application.dataPath + "/Resources/Mechanics",
            "Молекулярная физика" => Application.dataPath + "/Resources/Molecular",
            "Электричество" => Application.dataPath + "/Resources/Electricity",
            _ => "",
        };
        //Если такой директории нет, то список пуст
        if (!Directory.Exists(pathFolder)) return;

        //Получаем текстуру для вопроса
        WWW www = new WWW("file://" + pathFolder + "/Q" + numberQuestion.ToString() + ".png");
        texturesList.Add(www.texture);

        //Получаем все файлы
        string[] allFiles = Directory.GetFiles(pathFolder + "/Q" + numberQuestion.ToString());
        //Вместо файлов делаем числа из номера вопроса
        foreach (string file in allFiles)
        {
            //Если файл кончается на .png
            if (file.Substring(file.Length - 3, 3) == "png")
            {
                www = new WWW("file://" + file);
                texturesList.Add(www.texture);
            }
        }
        _taskManager.ViewTaskEdit(texturesList.ToArray(), direction, numberQuestion);
    }

    public void RedrawList()
    {
        //Очищаем списки
        this._listView.ClearList();
        this.ClearTexturesList();

        //Получаем направление
        string direction = this._directionDropdown.options[_directionDropdown.value].text;
        //Поиск папки полученного направления для генерации png
        string pathFolder = direction switch
        {
            "Механика" => Application.dataPath + "/Resources/Mechanics",
            "Молекулярная физика" => Application.dataPath + "/Resources/Molecular",
            "Электричество" => Application.dataPath + "/Resources/Electricity",
            _ => "",
        };

        //Если такой директории нет, то список пуст
        if (!Directory.Exists(pathFolder)) return;

        //Получаем все файлы
       string[] allFiles = this.SortFiles(Directory.GetFiles(pathFolder));
        //Вместо файлов делаем числа из номера вопроса
        foreach (string file in allFiles)
        {
            WWW www = new WWW("file://" + file);
            _textures.Add(www.texture);

        }

        this.CreateElements(direction);
    }
    
    string[] SortFiles(string[] files)
    {
        //Убираю все файлы, которые не кончаются на png
        List<string> newFiles = new List<string>();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Substring(files[i].Length - 3, 3) == "png")
            {
                newFiles.Add(files[i]);
            }
        }

        //Создаю отсортированный массив
        string[] sortFiles = new string[newFiles.Count];
        foreach (string file in newFiles)
        {
            //Поиск последнего вхождения обратного слеша \
            int indexSlash = file.LastIndexOf("\\", file.Length - 1, file.Length);
            //Поиск последнего вхождения точки .
            int indexDote = file.LastIndexOf(".", file.Length - 1, file.Length);
            //Поиск номера вопроса
            int numberQuestion = Convert.ToInt32(file.Substring(indexSlash + 2, indexDote - indexSlash - 2));
            sortFiles[numberQuestion - 1] = file;
        }
        return sortFiles;
    }
}
