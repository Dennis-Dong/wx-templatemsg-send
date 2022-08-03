namespace PrettyNotify.Models
{
    public class AccessToken
    {
        public string Access_Token { get; set; }
        public string Expires_In { get; set; }
    }

    public class SendMsgEntity
    {
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }
        public string MsgId { get; set; }
    }
}
