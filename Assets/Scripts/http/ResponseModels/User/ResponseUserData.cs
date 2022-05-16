public class ResponseUserData
{
    public ResponseUser user { get; set; }
    public string jwt { get; set; }

    public ResponseUserData()
    {
        jwt = "0";
    }
}