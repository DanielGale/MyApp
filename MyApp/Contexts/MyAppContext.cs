using Microsoft.EntityFrameworkCore;
using MyApp.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.API.Contexts
{
    public class MyAppContext : DbContext
    {
        public DbSet<State> States { get; set; }
        public DbSet<County> Counties { get; set; }

        public MyAppContext(DbContextOptions<MyAppContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<State>().Property(s => s.Id).ValueGeneratedNever();
            modelBuilder.Entity<County>().Property(c => c.Id).ValueGeneratedNever();
            //    modelBuilder.Entity<State>()
            //        .HasData(
            //        new State()
            //        {
            //            Id = 13,
            //            Name = "Georgia",
            //            PostalCode = "GA"
            //        },
            //        new State()
            //        {
            //            Id = 12,
            //            Name = "Florida",
            //            PostalCode = "FL"
            //        }
            //        );

            //    modelBuilder.Entity<County>()
            //        .HasData(
            //        new County()
            //        {
            //            Id = 13319,
            //            Name = "Wilkinson",
            //            StateId = 13
            //        },
            //        new County()
            //        {
            //            Id = 13021,
            //            Name = "Bibb",
            //            StateId = 13
            //        }
            //        );

            base.OnModelCreating(modelBuilder);
        }
    }
}
