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
    [SerializeField] private Button m_ActionButton;
    [Space]
    [SerializeField] private string m_SomeId;

    public void SetSomeId(string someId) => this.m_SomeId = someId;
    public string GetSomeId() => this.m_SomeId;
    public void SetTitle(string title) => m_title.text=title;
    public float Width() => m_transform.rect.width;
    public float Height() => m_transform.rect.height;
    public Button GetActionButton() => this.m_ActionButton;

}
