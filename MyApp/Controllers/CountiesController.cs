using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyApp.API.Models;
using MyApp.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.API.Controllers
{
    [ApiController]
    [Route("api/states/{stateId}/counties")]
    public class CountiesController : ControllerBase
    {
        private readonly ILogger<CountiesController> _logger;
        private readonly IMailService _mailService;
        private readonly IMyAppRepository _myAppRepository;
        private readonly IMapper _mapper;

        public CountiesController(ILogger<CountiesController> logger,
            IMailService mailService, IMyAppRepository myAppRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _myAppRepository = myAppRepository ?? throw new ArgumentNullException(nameof(myAppRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public IActionResult GetCounties(int stateId)
        {
            try
            {
                if(!_myAppRepository.StateExists(stateId))
                {
                    _logger.LogInformation($"State with Fips {stateId} wasn't found when " +
                        $"accessing Counties");
                    return NotFound();
                }

                var countiesForState = _myAppRepository.GetCountiesForState(stateId);
                
                return Ok(_mapper.Map<IEnumerable<CountyDto>>(countiesForState));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest while state with and a Fips code of {stateId}", ex);
                return StatusCode(500, "A problem happened while handling this request.");
            }
        }

        [HttpGet("{id}", Name = "GetCounty")]
        public IActionResult GetCounty(int stateId, int id)
        {
            if (!_myAppRepository.StateExists(stateId))
            {
                return NotFound();
            }

            var county = _myAppRepository.GetCountyForState(stateId, id);

            if (county == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CountyDto>(county));
        }

        [HttpPost]
        public IActionResult CreateCounty(int stateId,
            [FromBody] CountyForCreationDto county)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_myAppRepository.StateExists(stateId))
            {
                return NotFound();
            }

            var finalCounty = _mapper.Map<Entities.County>(county);

            _myAppRepository.AddCountyForState(stateId, finalCounty);
            _myAppRepository.Save();

            var createdCountyToReturn = _mapper
                .Map<Models.CountyDto>(finalCounty);

            return CreatedAtRoute(
                nameof(GetCounty),
                new { stateId, id = createdCountyToReturn.Id },
                createdCountyToReturn);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCounty(int stateId, int id,
            [FromBody] CountyForUpdateDto county)
        {
            if(_myAppRepository.StateExists(stateId))
            {
                return NotFound();
            }

            var countyEntity = _myAppRepository
                .GetCountyForState(stateId, id);

            if (countyEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(county, countyEntity);

            _myAppRepository.UpdateCountyForState(stateId, countyEntity);

            _myAppRepository.Save();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateCounty(int stateId, int id,
            [FromBody] JsonPatchDocument<CountyForUpdateDto> patchDoc)
        {
            if (!_myAppRepository.StateExists(stateId))
            {
                return NotFound();
            }

            var countyEntity = _myAppRepository
                .GetCountyForState(stateId, id);
            if (countyEntity == null)
            {
                return NotFound();
            }

            var countyToPatch = _mapper
                .Map<CountyForUpdateDto>(countyEntity);

            patchDoc.ApplyTo(countyToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(countyToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(countyToPatch, countyEntity);

            _myAppRepository.UpdateCountyForState(stateId, countyEntity);

            _myAppRepository.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCounty(int stateId, int id)
        {
            if(!_myAppRepository.StateExists(stateId))
            {
                return NotFound();
            }

            var countyEntity = _myAppRepository
                .GetCountyForState(stateId, id);

            if(countyEntity == null)
            {
                return NotFound();
            }

            _myAppRepository.DeleteCountyForState(countyEntity);
            _myAppRepository.Save();

            _mailService.Send("County Delete",
                $"{countyEntity.Name} county with Fips {countyEntity.Id} was deleted.");
            
            return NoContent();
        }
    }
}
