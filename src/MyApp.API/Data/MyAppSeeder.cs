using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyApp.API.Contexts;
using MyApp.API.Entities;
using Newtonsoft.Json;

namespace MyApp.API.Data
{
    public class MyAppSeeder
    {
        private readonly MyAppContext _context;
        private readonly IHostingEnvironment _hosting;
        private readonly UserManager<MyAppUser> _userManager;

        public MyAppSeeder(MyAppContext context, IHostingEnvironment hosting, UserManager<MyAppUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _hosting = hosting ?? throw new ArgumentNullException(nameof(hosting));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        public async Task SeedAsync()
        {
            //_context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            MyAppUser user = await _userManager.FindByEmailAsync("user@myApp.com");
            if (user == null)
            {
                user = new MyAppUser
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "user@myApp.com",
                    UserName = "user@myApp.com"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in seeder");
                }
            }

            if (!_context.States.Any())
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var filepath = Path.Combine(_hosting.ContentRootPath, "Data/states.json");
                    var json = File.ReadAllText(filepath);
                    var states = JsonConvert.DeserializeObject<IEnumerable<State>>(json);
                    _context.States.AddRange(states);

                    _context.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[States] ON");
                    _context.SaveChanges();
                    _context.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[States] OFF");

                    transaction.Commit();
                }
            }

            if (!_context.Counties.Any())
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var filepath = Path.Combine(_hosting.ContentRootPath, "Data/counties.json");
                    var json = File.ReadAllText(filepath);
                    var counties = JsonConvert.DeserializeObject<IEnumerable<County>>(json);

                    foreach (var state in _context.States)
                    {
                        foreach (var county in counties.Where(x => x.StateId == state.Id))
                        {
                            state.Counties.Add(county);
                        }

                    }

                    _context.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Counties] ON");
                    _context.SaveChanges();
                    _context.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Counties] OFF");

                    transaction.Commit();
                }
            }

        }
        public void Seed()
        {
            //_context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            if (!_context.States.Any())
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var filepath = Path.Combine(_hosting.ContentRootPath, "Data/states.json");
                    var json = File.ReadAllText(filepath);
                    var states = JsonConvert.DeserializeObject<IEnumerable<State>>(json);
                    _context.States.AddRange(states);

                    _context.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[States] ON");
                    _context.SaveChanges();
                    _context.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[States] OFF");

                    transaction.Commit();
                }
            }

            if (!_context.Counties.Any())
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var filepath = Path.Combine(_hosting.ContentRootPath, "Data/counties.json");
                    var json = File.ReadAllText(filepath);
                    var counties = JsonConvert.DeserializeObject<IEnumerable<County>>(json);

                    foreach (var state in _context.States)
                    {
                        foreach (var county in counties.Where(x => x.StateId == state.Id))
                        {
                            state.Counties.Add(county);
                        }

                    }

                    _context.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Counties] ON");
                    _context.SaveChanges();
                    _context.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Counties] OFF");

                    transaction.Commit();
                }
            }
        }
    }
}
