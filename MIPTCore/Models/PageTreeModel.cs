using System.Collections.Generic;

namespace MIPTCore.Models
{
    public class PageTreeModel
    {
        public IList<PageTreeModel> Nodes { get; set; }

        public string PageName { get; set; }
    }
}