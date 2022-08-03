namespace PrettyNotify.Models
{
    public class HitokotoEntity
    {
        public int Id { get; set; }
        public string Uuid { get; set; }
        public string Hitokoto { get; set; }
        public string Type { get; set; }
        public string From { get; set; }
        public string From_Who { get; set; }
        public string Creator { get; set; }
        public int Creator_Uid { get; set; }
        public int Reviewer { get; set; }
        public string Commit_From { get; set; }
        public string Created_At { get; set; }
        public int Length { get; set; }
    }

}
