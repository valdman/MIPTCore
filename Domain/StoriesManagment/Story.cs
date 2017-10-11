using CapitalManagment;
using Common;

namespace StoriesManagment
{
    public class Story : PersistentEntity
    {
        public Person Owner { get; set; }
        public string Content { get; set; }
    }
}