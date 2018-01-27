using Common.Abstractions;
using Common.Entities;

namespace CapitalManagment
{
    public class Person : AbstractIdentifyable
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Quote { get; set; }

        public Image Image { get; set; }
    }
}