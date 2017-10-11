using System;

namespace MIPTCore.Models
{
    public class AbstractPageModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        
        public string Content { get; set; }
    }

    public class PageModel : AbstractPageModel
    {
        public int Id { get; set; }
        public DateTimeOffset CreatingTime { get; set; }
    }
    public class PageUpdateModel : AbstractPageModel {}
    public class PageCreationModel : AbstractPageModel {}
}