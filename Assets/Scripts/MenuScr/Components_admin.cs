using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Components_admin : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private List_view_admin m_ListView;
    [SerializeField] private GameObject m_Prefab;

    [Header ("Settings")] // ѕосле изменени€ надо удалить это
    [SerializeField] private string m_Tittle;
    [Space]
    [SerializeField] private int m_Count;

    private void Awake()
    {
        for (int i=0; i<this.m_Count; i++) // вместо this.m_Count надо вставить количество учителей
        {
            GameObject element = this.m_ListView.Add(this.m_Prefab);

            List_element_admin elementMeta = element.GetComponent<List_element_admin>();
            elementMeta.SetTitle(i+1 + ". "+ this.m_Tittle ); // вместо this.m_Tittle надо выводить ‘»ќ учител€

            Button actionButton = elementMeta.GetActionButton();
            actionButton.onClick.AddListener(SomeAction);

            //string id_button = ; // надо придать значение такое же как и у id учител€, чтобы по нажатию активировалс€ нужный учитель
            // elementMeta.SetSomeId(id_button);
        }
    }

    public void SomeAction()
    {
        //надо как то придумать чтобы активировалс€ тот который надо
    }
}
