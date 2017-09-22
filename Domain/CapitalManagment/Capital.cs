using Common;

namespace CapitalManagment
{
    public class Capital : PersistentEntity
    {
        public string Name { get; set; }
        
        //todo: to component
        public string Content { get; set; }
        public string Description { get; set; }

        public Image Image {get; set;}

        public decimal Need { get; set; }
        public decimal Given { get; set; }
    }
}