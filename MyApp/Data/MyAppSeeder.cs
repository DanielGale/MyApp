using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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

        public MyAppSeeder(MyAppContext context, IHostingEnvironment hosting)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _hosting = hosting ?? throw new ArgumentNullException(nameof(hosting));
        }
        public void Seed()
        {
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
                var filepath = Path.Combine(_hosting.ContentRootPath, "Data/counties.json");
                var json = File.ReadAllText(filepath);
                var counties = JsonConvert.DeserializeObject<IEnumerable<County>>(json);
                _context.Counties.AddRange(counties);

                _context.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Counties] ON");
                _context.SaveChanges();
                _context.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Counties] OFF");
            }
        }
    }
}
