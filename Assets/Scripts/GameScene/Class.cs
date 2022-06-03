using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Class : MonoBehaviour
{
    public bool isRoomOpen;
    public string roomTitle;
    public int roomGroupID;

    private Door thisDoor;
    private Board thisBoard;
    private TextMesh thisPlate;

    public void AssignInformation(string groupTitle, int groupID)
    {
        roomGroupID = groupID;
        roomTitle = groupTitle;
        thisPlate.text = "Класс " + roomTitle;        
        OpenRoom();        
    }
    public void OpenRoom()
    {
        thisDoor.Unlock();        
        thisBoard.LoadGroupTests(roomGroupID);
        isRoomOpen = true;
    }

    private void FindObjects()
    {
        thisDoor = this.transform.Find("Interior").Find("door").Find("trigger").gameObject.AddComponent<Door>();
        thisBoard = this.transform.Find("Board").gameObject.AddComponent<Board>();
        thisPlate = this.transform.Find("Interior").Find("Table").Find("Text").GetComponent<TextMesh>();
    }

    private void Start()
    {        
        FindObjects();
    }
}
