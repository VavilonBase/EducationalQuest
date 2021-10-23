using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderImage : MonoBehaviour
{
    public Canvas canvas;
    private Camera _camera;
    private Texture2D _texture;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        _texture = new Texture2D(2048, 1024);
    }
    public Texture2D GenerateTexture()
    {
        //Запоминаю старую текстуру
        RenderTexture rndOld = RenderTexture.active;
        //Делаю холст активным
        canvas.gameObject.SetActive(true);
        //Включаю камеру
        _camera.enabled = true;
        //Делаю рендер
        _camera.Render();
        //Делаю холст не активным
        canvas.gameObject.SetActive(false);
        //Выключаю камеру
        _camera.enabled = false;
        //Задаю текстуру, которую срендерила камера
        RenderTexture.active = _camera.targetTexture;
        //Считываю текстуру, которую срендерила камера в текстуру 
        _texture.ReadPixels(new Rect(0, 0, _camera.targetTexture.width, _camera.targetTexture.height), 0, 0);
        //Создаю текстуру
        _texture.Apply();
        //Возвращаю обратно текстуру
        RenderTexture.active = rndOld;
        //Возврат текстуры
        return _texture;
    }
}
