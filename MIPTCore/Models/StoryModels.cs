using System;
using System.ComponentModel.DataAnnotations;

namespace MIPTCore.Models
{
    public abstract class AbstractStoryModel
    {
        [Required]
        public PersonModel Owner { get; set; }
        
        [Required]
        public string Content { get; set; }
        
        public string FullPageUri { get; set; }
    }

    public class StoryModel : AbstractStoryModel
    {
        public int Id { get; set; }
        public DateTimeOffset CreatingTime { get; set; }
    }
    
    public class StoryCreationModel : AbstractStoryModel
    {}

    public class StoryUpdateModel : AbstractStoryModel
    {}
}