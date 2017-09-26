namespace MIPTCore.Models
{
    public class AbstractPageModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        
        public string Data { get; set; }
    }
    
    public class PageUpdateModel : AbstractPageModel {}
    public class PageModel : AbstractPageModel {}
    public class PageCreationModel : AbstractPageModel {}
}