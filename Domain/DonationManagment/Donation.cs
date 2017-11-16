using System;
using Common;

namespace DonationManagment
{
    public class Donation : PersistentEntity
    {
        public int UserId { get; set; }

        public int CapitalId { get; set; }

        public decimal Value { get; set; }

        public DateTimeOffset Date { get; set; }

        public bool IsRecursive { get; set; }

        public bool IsConfirmed { get; set; }
    }
}