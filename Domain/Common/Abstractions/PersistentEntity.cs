using System;

namespace Common.Abstractions
{
    public abstract class PersistentEntity : AbstractIdentifyable
    {
        public DateTimeOffset CreatingTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}