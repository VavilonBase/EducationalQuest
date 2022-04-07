using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTeacherGroupsListInteractions : MonoBehaviour
{
    private CsGlobals gl;
    public List<Group> listGroups;

    async void OnEnable()
    {
        Dropdown dd = this.transform.Find("Dropdown").GetComponent<Dropdown>();
        dd.ClearOptions();

        listGroups = await MenuInteractions.ShowAllGroupsList(gl.playerInfo.responseUserData.jwt);
        if (listGroups == null)
        {
            Debug.Log("Список групп пуст");
        }
        else
        {
            List<string> m_DropOptions = new List<string>();
            foreach (Group g in listGroups)
            {
                m_DropOptions.Add(g.title);
            }
            dd.AddOptions(m_DropOptions);
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
