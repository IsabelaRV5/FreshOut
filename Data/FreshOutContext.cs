using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Data
{
    public class FreshOutContext : DbContext
    {
        //DbSets
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<GroceryItem> GroceryItems { get; set; }
        public DbSet<GroceryList> GroceryLists { get; set; }
        public DbSet<LocationItem> LocationItems { get; set; }
        public DbSet<LocationList> LocationLists { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder
        .UseNpgsql(GetConnectionString());

        private string GetConnectionString()
        {
            var configuration = new ConfigurationBuilder()
             .SetBasePath(AppContext.BaseDirectory)
             .AddUserSecrets<FreshOutContext>(optional: true)
             .Build();

            var connectionString = configuration["ConnectionString"];

            return connectionString;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
