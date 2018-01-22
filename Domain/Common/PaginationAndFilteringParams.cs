namespace Common
{
    public class PaginationAndFilteringParams
    {
        public string Field { get; set; }
        public string Order { get; set; }

        public int Page { get; set; }
        public int PerPage { get; set; }
    }
}