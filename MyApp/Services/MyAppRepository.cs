using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyApp.API.Contexts;
using MyApp.API.Entities;
using MyApp.API.Models;

namespace MyApp.API.Services
{
    public class MyAppRepository : IMyAppRepository
    {
        private readonly MyAppContext _context;

        public MyAppRepository(MyAppContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<State> GetStates()
        {
            return _context.States.OrderBy(s => s.Name).ToList();
        }

        public State GetState(int stateId, bool includeCounties)
        {
            if(includeCounties)
            {
                return _context.States.Include(s => s.Counties)
                    .Where(s => s.Id == stateId).FirstOrDefault();
            }

            return _context.States.Where(s => s.Id == stateId).FirstOrDefault();
        }

        public IEnumerable<County> GetCountiesForState(int stateId)
        {
            return _context.Counties
                .Where(c => c.StateId == stateId).OrderBy(c => c.Name).ToList();
        }

        public County GetCountyForState(int stateId, int countyId)
        {
            return _context.Counties
                .Where(c => c.StateId == stateId && c.Id == countyId).FirstOrDefault();
        }

        public bool StateExists(int stateId)
        {
            return _context.States.Any(s => s.Id == stateId);
        }

        public void AddCountyForState(int stateId, County county)
        {
            var state = GetState(stateId, false);
            state.Counties.Add(county);

        }

        public void UpdateCountyForState(int stateId, County county)
        {
            
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void DeleteCountyForState(County county)
        {
            _context.Counties.Remove(county);
        }
    }
}
