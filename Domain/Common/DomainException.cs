using System;

namespace Common
{
    public class DomainException : Exception
    {
        public string FieldName { get; protected set; }
    }
}