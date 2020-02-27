using MyApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.API
{
    public class StateDataStore
    {
        public static StateDataStore Current { get; } = new StateDataStore();
        public List<StateDto> States { get; set; }

        public StateDataStore()
        {
            States = new List<StateDto>()
            {
                new StateDto()
                {
                    Id = 13,
                    Name = "Georgia",
                    PostalCode = "GA",
                    Counties = new List<CountyDto>()
                    {
                        new CountyDto()
                        {
                            Id = 13319,
                            Name = "Wilkinson"
                        },
                        new CountyDto()
                        {
                            Id = 13021,
                            Name = "Bibb"
                        },
                    }
                },
                new StateDto()
                {
                    Id = 12,
                    Name = "Florida",
                    PostalCode = "FL"
                }
            };
        }
    }
}
