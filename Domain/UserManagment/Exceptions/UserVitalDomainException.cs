using System;
using System.Collections;

namespace UserManagment.Exceptions
{
    public class UserVitalDomainException : Exception
    {
        public UserVitalDomainException(string additionalInfo)
        {
            Message = $"Domain-level exception ocuurs in User Managment. " 
                      + additionalInfo;
        }
        
        public override string Message { get; }
    }
}