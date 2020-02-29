using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.API.Models;
using MyApp.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.API.Controllers
{
    [ApiController]
    [Route("api/states")]
    public class StatesController : ControllerBase
    {
        private readonly IMyAppRepository _myAppRepository;
        private readonly IMapper _mapper;

        public StatesController(IMyAppRepository myAppRepository, IMapper mapper)
        {
            _myAppRepository = myAppRepository ?? throw new ArgumentNullException(nameof(myAppRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetStates()
        {
            var stateEntities = _myAppRepository.GetStates();

            //var results = new List<StateWithoutCountiesDto>();
            //foreach (var stateEntity in stateEntities)
            //{
            //    results.Add(new StateWithoutCountiesDto
            //    {
            //        Id = stateEntity.Id,
            //        Name = stateEntity.Name,
            //        PostalCode = stateEntity.PostalCode
            //    });
            //}

            return Ok(_mapper.Map<IEnumerable<StateWithoutCountiesDto>>(stateEntities));
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetState(int id, bool includeCounties = false)
        {
            var state = _myAppRepository.GetState(id, includeCounties);

            if (state == null)
            {
                return NotFound();
            }

            if (includeCounties)
            {
                return Ok(_mapper.Map<StateDto>(state));
            }

            return Ok(_mapper.Map<StateWithoutCountiesDto>(state));
        }
    }
}
