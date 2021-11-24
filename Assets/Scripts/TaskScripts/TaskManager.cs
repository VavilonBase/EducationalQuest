using UnityEngine;

public class TaskManager : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private GameObject _taskAddUI;
    [SerializeField] private GameObject _taskListUI;
    [SerializeField] private GameObject _taskEditUI;
    [Space]
    [Header("Task Generator")]
    public TextureGenerator textureGenerator;

    private TaskEditManager _taskEditManager;
    private TaskListManger _taskListManager;

    private void Awake()
    {
        CloseAllUI();
        _taskListUI.SetActive(true);
        _taskEditManager = _taskEditUI.GetComponent<TaskEditManager>();
        _taskListManager = _taskListUI.GetComponent<TaskListManger>();
    }

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
        _taskListManager.RedrawList();
        _taskListUI.SetActive(true);
    }

    /// <summary>
    /// Показывает окно с изменением задания
    /// </summary>
    public void ViewTaskEdit(Texture2D[] textures, string direction, int numberQuestion)
    {
        CloseAllUI();
        _taskEditUI.SetActive(true);
        _taskEditManager.Initialized(textures, direction, numberQuestion);
    }

    //------------ДОП. МЕТОДЫ-------------
    void CloseAllUI()
    {
        _taskAddUI.SetActive(false);
        _taskListUI.SetActive(false);
        _taskEditUI.SetActive(false);
    }
}