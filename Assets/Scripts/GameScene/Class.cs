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

    public void AssignInformation(string groupTitle, int groupID)
    {        
        roomTitle = groupTitle;
        roomGroupID = groupID;
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
    }

    private void Awake()
    {        
        FindObjects();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
