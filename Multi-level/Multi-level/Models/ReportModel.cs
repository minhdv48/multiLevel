namespace Multi_level.Models
{
    public class ReportModel
    {
        public string FullName { get; set; }
        public int Levels { get; set; }
        public int ParentId { get; set; }
        public decimal Profit { get; set; }
    }
}
