namespace Common
{
    public class Capital : PersistentEntity
    {
        public string Name { get; set; }
        
        //todo: To component
        public string Content { get; set; }
        
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }

        public Image Image {get; set;}

        public decimal Need { get; set; }
        public decimal Given { get; set; }
    }
}