using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VegaCars.Models;
using VegaCars.Persistence;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VegaCars.Controllers
{
    public class MakesController : Controller
    {
        VegaDbContext context = null;
        public MakesController(VegaDbContext context)
        {
            this.context = context;
        }

        [HttpGet("/api/makes/")]
        public async Task<IEnumerable<Make>> GetMakes()
        {
            return await context.Makes.Include(m => m.Models).ToListAsync();
        }

    }
}
