using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class List_view_admin : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform m_ContentTransform;
    [SerializeField] private RectTransform m_ContentRectTranform;

    [Header("Settings")]
    [SerializeField] private List<GameObject> m_elements;
    [SerializeField] private float m_offset;

    public List<GameObject> Elements { get { return m_elements; } set { m_elements = value; } }

    public GameObject Add(GameObject element)
    {
        GameObject createdElement = Instantiate(element, this.m_ContentTransform);

        if (this.m_elements.Count==0)
        {
            this.m_elements.Add(createdElement);
            return createdElement;
        }

        List_element_admin elementMeta = createdElement.GetComponent<List_element_admin>();
        GameObject LastElement = this.m_elements.Last();

        Vector3 lastElementPosition = LastElement.transform.localPosition;

        Vector3 newElementPosition = new Vector3
        {
            x = lastElementPosition.x,
            y = lastElementPosition.y - elementMeta.Height() - this.m_offset,
            z = lastElementPosition.z
        };

        createdElement.transform.localPosition = newElementPosition;

        this.m_elements.Add(createdElement);

        float contentHeight = this.m_ContentRectTranform.rect.height;

        contentHeight += this.m_offset + elementMeta.Height();

        this.m_ContentRectTranform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentHeight);

        return (createdElement);
    }


}
