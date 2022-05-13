using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_appearance : MonoBehaviour
{
    /*
    private Key parentKey;
    CsGlobals gl;
    
    private bool playerIsCloseEnough = false; // находитс€ ли игрок достаточно близко, чтобы взаимодействовать

    void Start()
    {
        parentKey = this.GetComponentInParent<Key>();
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsCloseEnough && Input.GetKeyDown(KeyCode.F))
        {
            gl.playerInfo.GetKey(); // теперь ключ в инвентаре            
            parentKey.ChangeKeyPosition(gl.null_position); // убираем ключ из области видимости игрока
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == gl.player)
        {
            gl.PrintLabel("Ќажми F, чтобы вз€ть ключ"); // выводим сообщение: доступно взаимодействие
            playerIsCloseEnough = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == gl.player)
        {
            gl.HideLabel(); // убираем сообщение: взаимодействие больше недоступно
            playerIsCloseEnough = false;
        }
    }
    */
}
