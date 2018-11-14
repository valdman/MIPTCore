using System;
using System.ComponentModel.DataAnnotations;
using Common.Abstractions;
using Common.Entities;

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
        public bool IsVisible { get; set; }

        public Image Image { get; set; }
        public Image PreviewImage { get; set; }
    }
}