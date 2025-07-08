namespace Asp_.Net_Web_Api.Model.Domain
{
    public class Scheme
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Benefits { get; set; }
        public string State { get; set; }
        public string Category { get; set; }
        public string EligibilityCriteria { get; set; }
        public string ApplicationProcess { get; set; }
        public string RequiredDocuments { get; set; }
        public string ContactInformation { get; set; }
        public string WebsiteUrl { get; set; }
        public string HelplineNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
    }
}
