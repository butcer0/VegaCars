using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VegaCars.Core;
using VegaCars.Core.Models;
using VegaCars.Extensions;

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

        public async Task<IEnumerable<Vehicle>> GetVehicles(VehicleQuery queryObject)
        {
            var query = context.Vehicles
                .Include(v => v.Model)
                    .ThenInclude(m => m.Make)
                .Include(v => v.Features)
                    .ThenInclude(vf => vf.Feature)
                .AsQueryable();

            if(queryObject.MakeId.HasValue)
            {
                query = query.Where(v => v.Model.MakeId == queryObject.MakeId.Value);
            }

            if (queryObject.ModelId.HasValue)
            {
                query = query.Where(v => v.ModelId == queryObject.ModelId.Value);
            }

            var columnsMap = new Dictionary<string, Expression<Func<Vehicle, object>>>()
            {
                ["make"] = v => v.Model.Make.Name,
                ["model"] = v => v.Model.Name,
                ["contactName"] = v => v.ContactName,
                //["id"] = v => v.Id
            };

            query = query.ApplyingOrdering(queryObject, columnsMap);

            #region Depricated - Refactored with Dictionary
            //if(queryObject.SortBy == "make")
            //{
            //    query = (queryObject.IsSortAscending) ? query.OrderBy(v => v.Model.Make.Name) : query.OrderByDescending(v => v.Model.Make.Name);
            //}

            //if (queryObject.SortBy == "model")
            //{
            //    query = (queryObject.IsSortAscending) ? query.OrderBy(v => v.Model.Name) : query.OrderByDescending(v => v.Model.Name);
            //}

            //if (queryObject.SortBy == "contactName")
            //{
            //    query = (queryObject.IsSortAscending) ? query.OrderBy(v => v.ContactName) : query.OrderByDescending(v => v.ContactName);
            //}

            //if (queryObject.SortBy == "id")
            //{
            //    query = (queryObject.IsSortAscending) ? query.OrderBy(v => v.Model.Id) : query.OrderByDescending(v => v.Model.Id);
            //}
            #endregion

            return await query.ToListAsync();
        }

       

    }
}
