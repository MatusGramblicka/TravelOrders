using AutoMapper;
using Contracts.Dto;
using Entities.RequestFeatures;
using Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TravelOrdersServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CityController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public CityController(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet(Name = "GetCities")]
    public async Task<IActionResult> GetCities([FromQuery] RequestParameters requestParameters)
    {
        var cityFromDb = await _repository.City.GetAllCitiesAsync(requestParameters, trackChanges: false);

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(cityFromDb.MetaData));

        var cityDto = _mapper.Map<IEnumerable<CityDto>>(cityFromDb);

        return Ok(cityDto);
    }

    [HttpGet("citiesSelected", Name = "GetCitiesSelected")]
    public IActionResult GetCitiesSelected([FromQuery] RequestParameters requestParameters)
    {
        var cityFromDb = _repository.City.GetAllCitiesSelectedAsync(requestParameters);

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(cityFromDb.MetaData));

        return Ok(cityFromDb);
    }
}