using Common;

namespace MIPTCore.Models
{
    public abstract class AbstractCapitalModel
    {
        public string Name { get; set; }
        
        //todo: to component
        //public object Content { get; set; }
        public string Description { get; set; }

        public Image Image {get; set;}

        public decimal Need { get; set; }
    }
}