using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyApp.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.API.Contexts
{
    public class MyAppContext : IdentityDbContext<MyAppUser>
    {
        public DbSet<State> States { get; set; }
        public DbSet<County> Counties { get; set; }

        public MyAppContext(DbContextOptions<MyAppContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}