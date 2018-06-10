using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VegaCars.Controllers.Resources
{
    public class VehicleResource
    {
        public int Id { get; set; }
        public MakeResource Make { get; set; }
        public ModelResource Model { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public IEnumerable<FeatureResource> Features { get; set; }
        public DateTime LastUpdate { get; set; }

    }
}
