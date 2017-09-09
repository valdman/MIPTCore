using System;

namespace DataAccess
{
    public abstract class PersistentEntity
    {
        public int Id { get; set; }

        public DateTimeOffset CreatedTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}