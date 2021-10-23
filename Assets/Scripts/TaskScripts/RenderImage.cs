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
        //��������� ������ ��������
        RenderTexture rndOld = RenderTexture.active;
        //����� ����� ��������
        canvas.gameObject.SetActive(true);
        //������� ������
        _camera.enabled = true;
        //����� ������
        _camera.Render();
        //����� ����� �� ��������
        canvas.gameObject.SetActive(false);
        //�������� ������
        _camera.enabled = false;
        //����� ��������, ������� ���������� ������
        RenderTexture.active = _camera.targetTexture;
        //�������� ��������, ������� ���������� ������ � �������� 
        _texture.ReadPixels(new Rect(0, 0, _camera.targetTexture.width, _camera.targetTexture.height), 0, 0);
        //������ ��������
        _texture.Apply();
        //��������� ������� ��������
        RenderTexture.active = rndOld;
        //������� ��������
        return _texture;
    }
}
