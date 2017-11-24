using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Common;

namespace CapitalManagment
{
    public class Capital : PersistentEntity
    {
        public string Name { get; set; }
        
        public string Content { get; set; }
        public string Description { get; set; }

        public Image Image {get; set;}

        public decimal Given { get; set; }

        [DataType(DataType.Url)]
        public string FullPageUri { get; set; }

        public CapitalCredentials CapitalCredentials { get; set; }

        public IEnumerable<Person> Founders { get; set; }
        public IEnumerable<Person> Recivers { get; set; }
    }
}