using System;
using System.Collections.Generic;
using CapitalManagment;

namespace MIPTCore.Models
{
    public abstract class AbstractCapitalModel
    {
        public string Name { get; set; }

        public string Content { get; set; }
        public string Description { get; set; }
        public decimal Given { get; set; }

        public ImageModel Image { get; set; }
        
        public string BankAccountInformation { get; set; }

        public string OfferLink { get; set; }

        public string FullPageUri { get; set; }

        public IEnumerable<PersonModel> Founders { get; set; }
        public IEnumerable<PersonModel> Recivers { get; set; }
    }

    public class CapitalModel : AbstractCapitalModel
    {
        public int Id { get; set; }
        
        public DateTimeOffset CreatingTime { get; set; }
    }
    
    public class CapitalModelForAdmin : CapitalModel
    {
        public CapitalCredentialsModel CapitalCredentials { get; set; }
    }
    
    public class CapitalCreatingModel : AbstractCapitalModel
    {
        public CapitalCredentialsModel CapitalCredentials { get; set; }
    }

    public class CapitalUpdatingModel : AbstractCapitalModel
    {
        public CapitalCredentialsModel CapitalCredentials { get; set; }
    }
}