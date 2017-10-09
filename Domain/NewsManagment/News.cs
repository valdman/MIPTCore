using System;
using Common;

namespace NewsManagment
{
    public class News : PersistentEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public string Content { get; set; }
        public DateTimeOffset? Date { get; set; }

        public Image Image { get; set; }
    }
}