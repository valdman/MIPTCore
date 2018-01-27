using Common.DomainSteroids;

namespace CapitalsTableHelper
{
    public class RelatedCapitalNotExists : DomainException
    {
        public RelatedCapitalNotExists()
        {
            FieldName = "CapitalId";
        }
    }
}