using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuStudentJoinGroup : MonoBehaviour
{
    private CsGlobals gl;
    private GameObject buttonJoinGroup;
    private InputField inputCodeWord;

    private void Awake()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
        buttonJoinGroup = this.transform.Find("Join").gameObject;
        inputCodeWord = this.transform.Find("InputField").GetComponent<InputField>();
        buttonJoinGroup.GetComponent<Button>().onClick.AddListener(delegate { JoinGroup(); });
    }

    async void JoinGroup()
    {
        string codeWord = inputCodeWord.text;
        if (codeWord=="")
            gl.ChangeMessageTemporary("Введи кодовое слово, полученное от учителя", 5);
        else
        {
            var response = await GroupService.joinStudentToTheGroup(gl.playerInfo.responseUserData.jwt, codeWord);
            if (response.isError)
                switch (response.message)
                {
                    case Message.StudentIsInAGroup:
                        gl.ChangeMessageTemporary("Ты уже числишься в этой группе", 5);
                        break;
                    case Message.GroupNotFound:
                        gl.ChangeMessageTemporary("Неверное кодовое слово", 5);
                        break;
                    default:
                        gl.ChangeMessageTemporary(response.message.ToString(), 5);
                        break;
                }
            else
                gl.ChangeMessageTemporary("Успешное вступление в группу", 5);
        }        
    }
}
