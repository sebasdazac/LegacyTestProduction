namespace LegacyTest.Models.Request
{
    public class ReportRequest
    {
        public long Id { get; set; }
        public string NameForm { get; set; }
        public string? DescriptionReport { get; set; }
        public string? TitleScale { get; set; }
        public ICollection<ReportScale> ReportScaleList { get; set; }
    }
}
