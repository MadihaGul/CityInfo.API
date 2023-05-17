using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        //private readonly LocalMailService _mailService;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            //LocalMailService mailService
            IMailService mailService,
            ICityInfoRepository cityInfoRepository,
            IMapper mapper
            ) 
        { 
            _logger = logger?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            if (! await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with id {cityId} wasn't found when accessing point of interest");
                return NotFound();
            }
            var pointsOfInterestForCity = await _cityInfoRepository.GetPointsofInterestForCityAsync(cityId);
            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));

            //try
            //{
            //    //throw new Exception("ex sample");
            //    var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            //    if (city == null)
            //    {
            //        _logger.LogInformation($"City with id {cityId} wasnt found when assessing points of interest.");
            //        return NotFound();
            //    }
            //    return Ok(city.PointsOfInterest);

            //}
            //catch (Exception ex) {
            //    _logger.LogCritical($"Exception when getting point of interest for city with id {cityId}", ex
            //    );
            //    return StatusCode(500,"A problem occured while handling your request.");
            //}
            
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointsOfInterest(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var pointOfInterest = await _cityInfoRepository
                .GetPointOfInterestAsync(cityId, pointOfInterestId);
            if (pointOfInterest == null) { 
                return NotFound();
            }
            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
            //var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null) { return NotFound(); }
            //var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            //if (pointOfInterest == null)
            //{ return NotFound(); }
            //return Ok(pointOfInterest);
        }

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterest)
        {
            
            if (! await _cityInfoRepository.CityExistsAsync(cityId)) { return NotFound(); }

            var finalPointOfInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);
            
            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalPointOfInterest);
            await _cityInfoRepository.SaveChangesAsync();

            var createdPointOfInterestToReturn = _mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);
            return CreatedAtRoute("GetPointOfInterest",
                new 
                {
                    cityId = cityId,
                    pointOfInterestId = createdPointOfInterestToReturn.Id
                }, createdPointOfInterestToReturn
                );

        }

        [HttpPut("{pointOfInterestId}")]

        public async Task<ActionResult> UpdatePointOfInterest
            (int cityId, int pointOfInterestId,
            PointOfInterestForUpdateDto pointOfInterest)
        {
            
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) 
            { return NotFound(); }

            var pointofinterestEntity = await _cityInfoRepository
                .GetPointOfInterestAsync(cityId, pointOfInterestId);

            if (pointofinterestEntity == null) {  return NotFound(); }

            _mapper.Map(pointOfInterest, pointofinterestEntity);// overrrides destination object with those from source object
            await _cityInfoRepository.SaveChangesAsync();
            ////find point of interest
            //var pointofinterestfromstore = city.pointsofinterest.firstordefault(
            //    c => c.id == pointofinterestid); //??

            //if (pointofinterestfromstore == null)
            //{ return notfound(); }

            //pointofinterestfromstore.name = pointofinterest.name;
            //pointofinterestfromstore.description = pointofinterest.description;

            return NoContent();
        }

        [HttpPatch("{pointOfInterestId}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(
            int cityId, int pointOfInterestId,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument
            )
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) 
            { return NotFound(); }

            //find point of interest
            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestAsync(cityId,pointOfInterestId);
            if (pointOfInterestEntity == null)
            { return NotFound(); }

            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);
            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            { return BadRequest(ModelState); }

            if (!TryValidateModel(pointOfInterestToPatch))
            { return BadRequest(ModelState); }

            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();
            return NoContent();

        }

        [HttpDelete("{pointofinterestid}")]

        public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)

        {
            
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) { return NotFound(); }

            var pointOfInterestEntity = await _cityInfoRepository
                .GetPointOfInterestAsync(cityId, pointOfInterestId);
            if (pointOfInterestEntity == null)
            { return NotFound(); }
         
            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();
            _mailService.Send("point of interest deleted", $"point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted");
            return NoContent();
        }
    }
}
