using MyApp.API.Entities;
using MyApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.API.Services
{
    public interface IMyAppRepository
    {
        IEnumerable<State> GetStates();
        State GetState(int stateId, bool includeCounties);
        IEnumerable<County> GetCountiesForState(int stateId);
        County GetCountyForState(int stateId, int countyId);
        bool StateExists(int stateId);
        void AddCountyForState(int stateId, County county);
        void DeleteCountyForState(County county);
        void UpdateCountyForState(int stateId, County county);
        bool Save();
    }
}
