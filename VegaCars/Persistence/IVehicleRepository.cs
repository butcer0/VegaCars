using System.Threading.Tasks;
using VegaCars.Models;

namespace VegaCars.Persistence
{
    public interface IVehicleRepository
    {
        Task<Vehicle> GetVehicle(int id);
    }
}