using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VegaCars.Controllers.Resources
{
    public class PhotoResource
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
    }
}
