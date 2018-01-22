namespace Common
{
    public class PaginationAndFilteringParams
    {
        public string Field { get; set; }
        public string Order { get; set; }

        public int Take { get; set; }
        public int Skip { get; set; }
    }
}