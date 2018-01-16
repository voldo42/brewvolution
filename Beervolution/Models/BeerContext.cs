using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Beervolution.Models
{
    public class BeerContext : DbContext
    {
        public BeerContext() : base("BeerContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BeerContext, Beervolution.Migrations.Configuration>("BeerContext"));
        }

        public DbSet<Beer> Beers { get; set; }

        public DbSet<Variables> Variables { get; set; }

        public DbSet<Brew> Brews { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}