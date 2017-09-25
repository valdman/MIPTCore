using System.Collections.Generic;
using CapitalManagment;
using Common;

namespace MIPTCore.Models
{
    public abstract class AbstractCapitalModel
    {
        public string Name { get; set; }
        
        //todo: to component
        //public object Content { get; set; }
        public string Description { get; set; }

        public ImageModel Image {get; set;}

        public decimal Need { get; set; }

        public string FullPageUri { get; set; }
        
        public IEnumerable<PersonModel> Founders { get; set; }
        public IEnumerable<PersonModel> Recivers { get; set; }
    }
}