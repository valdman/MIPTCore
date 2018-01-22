using System.Collections.Generic;

namespace Common
{
    public class PaginatedList<T>
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
        public IEnumerable<T> Docs { get; set; }

        public PaginatedList(PaginationAndFilteringParams filteringParams, IEnumerable<T> objects, int total)
        {
            Total = total;
            Page = filteringParams.Page;
            PerPage = filteringParams.PerPage;
            Docs = objects;
        }
    }
}