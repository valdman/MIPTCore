using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [DataType(DataType.Url)]
        public string FullPageUri { get; set; }

        public IEnumerable<Person> Founders { get; set; }
        public IEnumerable<Person> Recivers { get; set; }
    }
}