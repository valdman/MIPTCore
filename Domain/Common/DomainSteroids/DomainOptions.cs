using Common.Abstractions;

namespace Common.DomainSteroids
{
    public class DomainOptions : AbstractIdentifyable
    {
        public decimal SizeOfFund { get; set; }
    }
}