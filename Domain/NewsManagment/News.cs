using System;
using Common;
using System.ComponentModel.DataAnnotations;

namespace NewsManagment
{
    public class News : PersistentEntity
    {
        public string Name { get; set; }
        
        [DataType(DataType.Url)]
        public string FullPageUri { get; set; }

        public string Description { get; set; }
        public string Content { get; set; }
        public DateTimeOffset? Date { get; set; }

        public Image Image { get; set; }
    }
}