using Common.Abstractions;

namespace PagesManagment
{
    public class Page : PersistentEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        
        public string Content { get; set; }
    }
}