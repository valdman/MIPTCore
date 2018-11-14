using Common.Abstractions;

namespace CapitalManagment
{
    public class Capitalization : AbstractIdentifyable
    {
        public int Year { get; set; }
        public int IncomePercentage { get; set; }
        public int SpentPercentage { get; set; }

        public static readonly int DefaultPercentage = 0;
        public static readonly int YearOfStartFunding = 2017;
    }
}