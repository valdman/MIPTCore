using System;
using System.Collections.Generic;

namespace MIPTCore.Models
{
    public abstract class AbstractCapitalModel
    {
        public string Name { get; set; }

        public string Content { get; set; }
        public string Description { get; set; }

        public ImageModel Image { get; set; }

        public string FullPageUri { get; set; }

        public IEnumerable<PersonModel> Founders { get; set; }
        public IEnumerable<PersonModel> Recivers { get; set; }
    }

    public class CapitalModel : AbstractCapitalModel
    {
        public int Id { get; set; }
        public decimal Given { get; set; }
        public DateTimeOffset CreatingTime { get; set; }
    }
    
    public class CapitalCreatingModel : AbstractCapitalModel
    {
        public decimal Given { get; set; }
    }

    public class CapitalUpdatingModel : AbstractCapitalModel {}
}