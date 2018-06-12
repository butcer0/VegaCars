using Microsoft.EntityFrameworkCore;
using VegaCars.Models;

namespace VegaCars.Persistence.Interfaces
{
    public interface IVegaDbContext 
    {
        DbSet<Feature> Features { get; set; }
        DbSet<Make> Makes { get; set; }
        DbSet<Model> Models { get; set; }
        DbSet<Vehicle> Vehicles { get; set; }
    }
}