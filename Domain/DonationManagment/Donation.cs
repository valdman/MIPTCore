using System;
using System.Collections.Generic;
using Common.Abstractions;

namespace DonationManagment
{
    public class Donation : PersistentEntity
    {
        public int UserId { get; set; }

        public int CapitalId { get; set; }

        public string BankOrderId { get; set; }

        public decimal Value { get; set; }

        public PaymentType PaymentType { get; set; }
        
        public DateTimeOffset? CancelDate { get; set; }

        public bool IsRecursive { get; set; }

        public bool IsConfirmed { get; set; }

        public decimal DonatedTillMoment(DateTimeOffset? monent = null)
        {
            if (!IsConfirmed) return 0;
            if (!IsRecursive) return Value;

            var end = monent ?? CancelDate ?? DateTimeOffset.Now;
            var numberOfMonthDonated = (int) decimal.Ceiling((end - CreatingTime).Days / (decimal) 28.0);

            return Value * (numberOfMonthDonated > 0 ? numberOfMonthDonated : 0);
        }
        
        public class HashEqualityComparer : IEqualityComparer<Donation>
        {
            public bool Equals(Donation x, Donation y)
            {
                if (x == null || y == null)
                {
                    return false;
                }
                return x.GetHashCode() == y.GetHashCode();
            }

            public int GetHashCode(Donation obj)
            {
                return $"{obj.Id} {obj.UserId} {obj.CapitalId} {obj.Value} {obj.CancelDate}".GetHashCode();
            }
        }
    }
}