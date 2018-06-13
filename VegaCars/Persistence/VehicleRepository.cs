using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VegaCars.Models;

namespace VegaCars.Persistence
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VegaDbContext context;

        public VehicleRepository(VegaDbContext context)
        {
            this.context = context;
        }

        #region Depricated - Introduced includeRelated
        //public async Task<Vehicle> GetVehicle(int id)
        #endregion
        public async Task<Vehicle> GetVehicle(int id, bool includeRelated = true)
        {
            if (!includeRelated)
                return await context.Vehicles.FindAsync(id);

            return await context.Vehicles
              .Include(v => v.Features)
                .ThenInclude(vf => vf.Feature)
              .Include(v => v.Model)
                .ThenInclude(m => m.Make)
              .SingleOrDefaultAsync(v => v.Id == id);
        }

        #region Sample - If you only wanted to partially hydrate the return object
        //public async Task<Vehicle> GetVehicleWithMake(int id)
        //{
        //    return await context.Vehicles
        //        .Include(v => v.Model)
        //            .ThenInclude(m => m.Make)
        //        .SingleOrDefaultAsync(v => v.Id == id);
        //}
        #endregion

        public void Add(Vehicle vehicle)
        {
            context.Vehicles.Add(vehicle);
        }

        public void Remove(Vehicle vehicle)
        {
            context.Vehicles.Remove(vehicle);
        }

    }
}
