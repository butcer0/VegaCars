using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VegaCars.Controllers.Resources;
using VegaCars.Models;
using VegaCars.Persistence;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VegaCars.Controllers
{
    public class MakesController : Controller
    {
        private readonly VegaDbContext context;
        private readonly IMapper mapper;

        public MakesController(VegaDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("/api/makes/")]
        public async Task<IEnumerable<MakeResource>> GetMakes()
        {
            var makes = await context.Makes.Include(m => m.Models).ToListAsync();
            return mapper.Map<List<Make>, List<MakeResource>>(makes);
        }

        [HttpGet("/api/features")]
        public async Task<IEnumerable<FeatureResource>> GetFeatures()
        {
            var features = await context.Features.ToListAsync();
            return mapper.Map<List<Feature>, List<FeatureResource>>(features);
        }

        [HttpGet("/api/vehicles")]
        public async Task<IEnumerable<VehicleResource>> GetVehicles()
        {
            var vehicles = await context.Vehicles.ToListAsync();
            return mapper.Map<List<Vehicle>, List<VehicleResource>>(vehicles);
        }

        [HttpPost("/api/vehicle")]
        public async Task<IActionResult> AddVehicle([FromBody] Vehicle newVehicle)
        {
            await context.Vehicles.AddAsync(newVehicle);
            int updatedRows = await context.SaveChangesAsync();
            return (updatedRows > 0) ? Ok() as StatusCodeResult : BadRequest() as StatusCodeResult;

        }
        [HttpPut("/api/vehicle")]
        public async Task<IActionResult> UpdateVehicle([FromBody] Vehicle vehicle)
        {
            context.Vehicles.Update(vehicle);
            int updatedRows = await context.SaveChangesAsync();
            return (updatedRows > 0) ? Ok() as StatusCodeResult : BadRequest() as StatusCodeResult;

        }

        [HttpDelete("/api/vehicle")]
        public async Task<IActionResult> DeleteVehicle([FromBody] Vehicle vehicle)
        {
            context.Vehicles.Remove(vehicle);
            int deletedRows = await context.SaveChangesAsync();
            return (deletedRows > 0) ? Ok() as StatusCodeResult : BadRequest() as StatusCodeResult;
        }
    }
}
