using System;
using Common;

namespace DonationManagment
{
    public class RollbackDonationException : DomainException
    {
        public RollbackDonationException()
        {
            FieldName = "rollbacking confirmed donation";
        }
    }
}