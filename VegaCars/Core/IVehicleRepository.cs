using System.Collections.Generic;
using System.Threading.Tasks;
using VegaCars.Core.Models;

namespace VegaCars.Core
{
    public interface IVehicleRepository
    {

        Task<Vehicle> GetVehicle(int id, bool includeRelated = true);
        Task<Vehicle[]> GetVehiclesAsync(int page, int itemsPerPage = 3);

        void Add(Vehicle vehicle);
        void Remove(Vehicle vehicle);
    }
}