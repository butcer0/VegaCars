using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VegaCars.Controllers.Resources
{
    public class ModelResponseResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MakeResource Make { get; set; }
    }
}