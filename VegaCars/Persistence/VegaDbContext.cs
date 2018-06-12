﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VegaCars.Models;
using VegaCars.Persistence.Interfaces;

namespace VegaCars.Persistence
{
    public class VegaDbContext : DbContext, IVegaDbContext
    {
        public DbSet<Make> Makes { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        

        public VegaDbContext(DbContextOptions<VegaDbContext> options)
            :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleFeature>().HasKey(vf =>   new { vf.VehicleId, vf.FeatureId });
        }
    }
}
