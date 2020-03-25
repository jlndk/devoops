namespace MiniTwit.Web.App.Models.Api
{
    public class PostFollowDTO
    {
        public string Follow { get; set; }
        public string UnFollow { get; set; }
        public int? Latest { get; set; }
    }
}