using Common;

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