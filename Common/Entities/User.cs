namespace Common.Entities
{
    public class User : PersistentEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}