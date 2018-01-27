using System;

namespace Common.DomainSteroids
{
    public class DomainException : Exception
    {
        public string FieldName { get; protected set; }
    }
}