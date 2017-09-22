using System;
using Common;

namespace UserManagment.Exceptions
{
    public class ProfileNotProvidedException : DomainException
    {
        public ProfileNotProvidedException()
        {
            FieldName = "AlumniProfile";
        }
        
        public override string Message => $"MIPT Alumni should provide '{FieldName}'";
    }

    public class ProfileShouldNotBeProvidedException : DomainException
    {
        public ProfileShouldNotBeProvidedException()
        {
            FieldName = "IsMiptAlumni";
        }
        
        public override string Message => $"'{FieldName}' is false but AlumniProfile presented";
    }
}