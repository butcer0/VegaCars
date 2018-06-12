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
    [Route("/api/vehicles")]
    public class VehiclesController : Controller
    {
        private readonly IMapper mapper;
        private readonly VegaDbContext context;

        public VehiclesController(IMapper mapper, VegaDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        [HttpPost]
       public async Task<IActionResult> CreateVehicle([FromBody]VehicleResource vehicleResource)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }




            #region Unnecessary ModelId, FeatureId Validation - comes from drop-down in client
            //var model = await context.Models.FindAsync(vehicleResource.ModelId);
            //if (model == null)
            //{
            //    ModelState.AddModelError("ModelId", "Invalid modelId.");
            //    return BadRequest(ModelState);
            //}

            //var notFoundFeatures = vehicleResource.Features.Where(f => context.Features.Find(f) == null);
            //if (notFoundFeatures.Any())
            //{
            //    ModelState.AddModelError("FeatureId", "Invalid featureId.");
            //    return BadRequest(ModelState);
            //}
            #region Depricated - Rewritten as lambda
            //foreach (int vId in vehicleResource.Features)
            //{
            //    var feature = await context.Features.FindAsync(vId);
            //    if (feature == null)
            //    {
            //        ModelState.AddModelError("FeatureId", "Invalid featureId.");
            //        return BadRequest(ModelState);
            //    }
            //}
            #endregion
            #endregion


            #region Business Rule Validation Sample
            // An example would be: don't allow more than 5 cars to be introduced per business day
            //if(//Put Business Rules Logic in Method/Service and Call Here)
            //{
            //    ModelState.AddModelError("..", "error");
            //    return BadRequest(ModelState);
            //}
            #endregion

            var vehicle = mapper.Map<VehicleResource, Vehicle>(vehicleResource);
            vehicle.LastUpdate = DateTime.Now;

            context.Vehicles.Add(vehicle);
            await context.SaveChangesAsync();

            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);

            #region Depricated - Should be returning domain resources not models directly
            //return Ok(vehicle);
            #endregion

            ////Will serialize as json and return with 200 success response
            //return Ok(vehicle);
        }

        [HttpPut("{id}")] // /api/vehicles/{id}
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody]VehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // Without including the features, unable to remove VehicleFeatures that are already present in table during mapping
            var vehicle = await context.Vehicles.Include(v => v.Features).SingleOrDefaultAsync(v => v.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }
                
            #region Depricated - Include Features to verify
            //var vehicle = await context.Vehicles.FindAsync(id);
            #endregion
            // Introduced the retrieved vehicle as the destination for the mapping
            mapper.Map<VehicleResource, Vehicle>(vehicleResource, vehicle);
            vehicle.LastUpdate = DateTime.Now;

            await context.SaveChangesAsync();

            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await context.Vehicles.FindAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }
                
            context.Remove(vehicle);
            await context.SaveChangesAsync();

            return Ok(vehicle);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            var vehicle = await context.Vehicles.Include(v => v.Features).SingleOrDefaultAsync(v => v.Id == id);
            #region Depricated - Eager Load Features to be Included in Response
            //var vehicle = await context.Vehicles.FindAsync(id);
            #endregion

            if (vehicle == null)
            {
                return NotFound();
            }

            var vehicleResource = mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(vehicleResource);
        }
    }
}
