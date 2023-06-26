namespace FindJobs.DataAccess.Entities
{
    public class Culture:BaseEntity
    {
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public string Language { get; set; }
        public bool IsEnabled { get; set; }

        #region Relations
        public Country Countries { get; set; }
        #endregion
    }
}