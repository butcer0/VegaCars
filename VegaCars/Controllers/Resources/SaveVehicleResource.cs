using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VegaCars.Controllers.Resources
{
    public class SaveVehicleResource
    {
        public int Id { get; set; }
        //Use foreign key to load model so not necessary to load entire Model object
        public int ModelId { get; set; }
        //There is no point in sending Id and object
        //public Model Model { get; set; }
        public bool IsRegistered { get; set; }
        [Required]
        public ContactResource Contact { get; set; }
        public ICollection<int> Features { get; set; }

        //Only send Id of features
        //public ICollection<VehicleFeature> Features { get; set; }

        public SaveVehicleResource()
        {
            Features = new Collection<int>();
        }
    }
}
