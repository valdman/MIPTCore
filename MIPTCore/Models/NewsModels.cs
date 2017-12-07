using System;
using System.ComponentModel.DataAnnotations;

namespace MIPTCore.Models
{
    public abstract class AbstractNewsModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        
        [Required]
        public string Content { get; set; }
        public DateTimeOffset? Date { get; set; }

        public ImageModel Image { get; set; }
        
        public string FullPageUri { get; set; }
    }

    public class NewsModel : AbstractNewsModel
    {
        public int Id { get; set; }
        public DateTimeOffset CreatingTime { get; set; }
    }
    
    public class NewsCreationModel : AbstractNewsModel
    {}

    public class NewsUpdateModel : AbstractNewsModel
    {}
}