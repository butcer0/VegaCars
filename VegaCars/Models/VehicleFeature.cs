using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VegaCars.Models
{
    [Table("VehicleFeatures")]
    public class VehicleFeature
    {
        //The composit key vehicleId + featureId should be unique to identify a vehiclefeature
        public int VehicleId { get; set; }
        public int FeatureId { get; set; }
        public Vehicle Vehicle { get; set; }
        public Feature Feature { get; set; }
    }
}
