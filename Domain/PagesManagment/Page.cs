using Common;

namespace PagesManagment
{
    public class Page : PersistentEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        
        public string Data { get; set; }
    }
}