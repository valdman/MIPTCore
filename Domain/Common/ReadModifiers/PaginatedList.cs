using System.Collections.Generic;
using System.Linq;
using Common.Entities.Entities.ReadModifiers;

namespace Common.ReadModifiers
{
    public class PaginatedList<T>
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
        public T[] Docs { get; set; }

        public PaginatedList(PaginationParams filteringParams, IEnumerable<T> objects, int total)
        {
            Total = total;
            Page = filteringParams.Page;
            PerPage = filteringParams.PerPage;
            Docs = objects.ToArray();
        }
    }
}