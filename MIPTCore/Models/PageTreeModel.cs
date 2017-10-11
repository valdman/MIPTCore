using System.Collections.Generic;

namespace MIPTCore.Models
{
    public class PageTreeModel
    {
        public int Id { get; set; }

        public string PageName { get; set; }   
        
        public IList<PageTreeModel> Nodes { get; set; }
    }
}