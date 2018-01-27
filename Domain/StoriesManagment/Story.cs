using System.ComponentModel.DataAnnotations;
using CapitalManagment;
using Common.Abstractions;

namespace StoriesManagment
{
    public class Story : PersistentEntity
    {
        public Person Owner { get; set; }
        public string Content { get; set; }
        
        [DataType(DataType.Url)]
        public string FullPageUri { get; set; }
    }
}