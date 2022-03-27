using System.Collections.Generic;

public static class TestService
{
    public static Response<List<Test>> getAllTestsByGroupId(int groupId)
    {
        return new Response<List<Test>>(false, Message.NotError, null);
    }
}