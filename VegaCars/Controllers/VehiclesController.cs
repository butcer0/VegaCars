using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VegaCars.Controllers.Resources;
using VegaCars.Core;
using VegaCars.Core.Models;

#region Depricated - Removed Low-level Dependency
//using VegaCars.Persistence;
#endregion

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VegaCars.Controllers
{
    [Route("/api/vehicles")]
    public class VehiclesController : Controller
    {
        private readonly IMapper mapper;
        #region Depricated - Decouple with IUnitOfWork & IVehicleRepository
        //private readonly VegaDbContext context;
        #endregion
        private readonly IVehicleRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public VehiclesController(IMapper mapper, IVehicleRepository repository, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        #region Depricated - Decoupled VegaDbContext with IUnitOfWork & IVehicleRepository
        //public VehiclesController(IMapper mapper, VegaDbContext context, IVehicleRepository repository, IUnitOfWork unitOfWork)
        //{
        //    this.mapper = mapper;
        //    this.context = context;
        //    this.repository = repository;
        //    this.unitOfWork = unitOfWork;
        //}
        #endregion

        [HttpPost]
       public async Task<IActionResult> CreateVehicle([FromBody]SaveVehicleResource vehicleResource)
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

            var vehicle = mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource);
            vehicle.LastUpdate = DateTime.Now;

            repository.Add(vehicle);
            await unitOfWork.CompleteAsync();

            #region Depricated - Pass through Repository to Decouple
            //context.Vehicles.Add(vehicle);
            #endregion
            #region Depricated - Moved to IUnitOfWork
            //await context.SaveChangesAsync();
            #endregion

            #region One way of including Eager loaded data for VehicleResource return
            //// Not necessary to return this to vehicle.Model -> once EF has loaded into context will update internally
            //await context.Models.Include(m => m.Make).SingleOrDefaultAsync(m => m.Id == vehicle.ModelId);
            #endregion

            // Fetch complete vehicle object - reset object ("rehydrate")
            vehicle = await repository.GetVehicle(vehicle.Id);

            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);

            #region Depricated - Should be returning domain resources not models directly
            //return Ok(vehicle);
            #endregion

            ////Will serialize as json and return with 200 success response
            //return Ok(vehicle);
        }

        [HttpPut("{id}")] // /api/vehicles/{id}
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody]SaveVehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // Fetch complete vehicle object - reset object ("rehydrate")
            var vehicle = await repository.GetVehicle(id);

            // Without including the features, unable to remove VehicleFeatures that are already present in table during mapping
            //var vehicle = await context.Vehicles.Include(v => v.Features).SingleOrDefaultAsync(v => v.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }
                
            #region Depricated - Include Features to verify
            //var vehicle = await context.Vehicles.FindAsync(id);
            #endregion
            // Introduced the retrieved vehicle as the destination for the mapping
            mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource, vehicle);
            vehicle.LastUpdate = DateTime.Now;

            await unitOfWork.CompleteAsync();

            #region Depricated - Moved to IUnitOfWork
            //await context.SaveChangesAsync();
            #endregion

            // 'Rehydrate' after update so value is present
            vehicle = await repository.GetVehicle(vehicle.Id);
            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await repository.GetVehicle(id, includeRelated: false);
            #region Depricated - Use repository without 'full hydration'
            //var vehicle = await context.Vehicles.FindAsync(id);
            #endregion

            if (vehicle == null)
            {
                return NotFound();
            }

            repository.Remove(vehicle);
            await unitOfWork.CompleteAsync();

            #region Depricated - Moved to Repository
            //context.Remove(vehicle);
            #endregion
            #region Depricated - Moved to IUnitOfWork
            //await context.SaveChangesAsync();
            #endregion

            return Ok(vehicle);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            // Vehicle -> VehicleFeature -> Feature
            var vehicle = await repository.GetVehicle(id);

            #region Depricated - ThenIclude vehicle -> VehicleFeature -> Feature
            //var vehicle = await context.Vehicles.Include(v => v.Features).SingleOrDefaultAsync(v => v.Id == id);
            #endregion
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

        [HttpGet]
        public async Task<IEnumerable<VehicleResource>> GetVehicles(VehicleQueryResource filterResource)
        {
            var filter = mapper.Map<VehicleQueryResource, VehicleQuery>(filterResource);
            var vehicles = await repository.GetVehicles(filter);

            return mapper.Map<IEnumerable<Vehicle>, IEnumerable<VehicleResource>>(vehicles);
        }
    }
}
