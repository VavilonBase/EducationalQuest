using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class List_element_admin : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform m_transform;
    [Space]
    [SerializeField] private Text m_title;
    [Space]
    [SerializeField] private Text m_description;
    [Space]
    [SerializeField] private Button m_ActionButton;
    [Space]
    [SerializeField] private string m_SomeId;
    [Space]
    [SerializeField] private Image m_image;

    public void SetSomeId(string someId) => this.m_SomeId = someId;
    public string GetSomeId() => this.m_SomeId;
    public void SetTitle(string title) => m_title.text=title;
    public void SetImage(Sprite image) => m_image.sprite = image;
    public void SetDescription(string description) => m_description.text = description;
    public float Width() => m_transform.rect.width;
    public float Height() => m_transform.rect.height;
    public Button GetActionButton() => this.m_ActionButton;

    public async void ActivateTeacher()
    {
        CsGlobals gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
        int id = Convert.ToInt32(m_SomeId);      
        var response = await UserService.activateTeacher(id, gl.playerInfo.responseUserData.jwt);
        if (response.isError)
            gl.ChangeMessageTemporary(response.message.ToString(), 5);
        else
        {
            gl.ChangeMessageTemporary("Учитель успешно активирован", 5);
            this.m_ActionButton.gameObject.SetActive(false);
        }
    }
}
