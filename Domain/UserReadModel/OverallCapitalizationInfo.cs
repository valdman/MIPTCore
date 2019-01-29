namespace UserReadModel
{
    public class OverallCapitalizationInfo
    {
        public int CapitalId { get; set; }
        public string CapitalName { get; set; }
        public string CapitalDescription { get; set; }
        
        public decimal Donated { get; set; }
        public decimal Income { get; set; }
        public decimal Spent { get; set; }
    }
}