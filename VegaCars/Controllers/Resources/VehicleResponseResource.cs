using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VegaCars.Controllers.Resources
{
    public class VehicleResponseResource
    {
        public int Id { get; set; }
       
        public ModelResponseResource Model { get; set; }
        
        //public MakeResource Make { get; set; }

        public bool IsRegistered { get; set; }
        [Required]
        public ContactResource Contact { get; set; }
        public ICollection<int> Features { get; set; }

        //Only send Id of features
        //public ICollection<VehicleFeature> Features { get; set; }

        public VehicleResponseResource()
        {
            Features = new Collection<int>();
        }
    }
}