namespace Website.Models
{
    public class JobPool
    {
        public int Id { get; set; }
        public int job_id { get; set; }
        public Nullable<int> finished { get; set; }
        public string solution { get; set; }
    }
}
