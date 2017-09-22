using System;

namespace MIPTCore.Models
{
    public class CapitalModel : AbstractCapitalModel
    {
        public decimal Given { get; set; }
        public DateTimeOffset CreatingTime { get; set; }
    }
}