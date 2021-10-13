using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTask : MonoBehaviour
{
    public Canvas canvas;
    private Camera _camera;
    public RenderTexture rndTex;
    [SerializeField] private Texture2D texture;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        texture = new Texture2D(500, 500);
    }

    // Update is called once per frame
    void Update()
    {
        

        RenderTexture rndOld = RenderTexture.active;
        canvas.gameObject.SetActive(true);
        _camera.enabled = true;

        _camera.Render();

        canvas.gameObject.SetActive(false);
        _camera.enabled = false;
        
        RenderTexture.active = _camera.targetTexture;
        
        // Read the text data    
        texture.ReadPixels(new Rect(0, 0, _camera.targetTexture.width, _camera.targetTexture.height), 0, 0);
        texture.Apply();
        
        RenderTexture.active = rndOld;
    }
}
