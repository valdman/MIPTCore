using Journalist.Extensions;

namespace Common.Entities.Entities.ReadModifiers
{
    public class OrderingParams
    {
        public string Field { get; set; }
        public string Order { get; set; }

        public bool IsEmpty() => Field.IsNullOrEmpty();
    }
}