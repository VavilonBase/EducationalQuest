/*using UnityEngine;

class Test1
{
    public Test1()
    {
        var response = UserService.login("admin", "admin");
        if (response.isError)
        {
            if (response.message == message.IncorrectPassword)
            {
                Debug.Log("");
            }
        } else
        {
            var user = response.data;
        }
        var response1 = GroupService.getAllGroups();
        if (response1.isError)
        {
            if (response.message == message.IncorrectPassword)
            {
                Debug.Log("");
            }
        }
        else
        {
            var groups = response1.data;
            
        }
    }
}*/