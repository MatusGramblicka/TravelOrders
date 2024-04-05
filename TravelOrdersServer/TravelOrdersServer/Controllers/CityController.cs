using Contracts.RequestFeatures;
using Interface.Managers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TravelOrdersServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CityController : ControllerBase
{
    private readonly ICityManager _cityManager;

    public CityController(ICityManager cityManager)
    {
        _cityManager = cityManager;
    }

    [HttpGet("citiesSelected", Name = "GetCitiesSelected")]
    public IActionResult GetCitiesSelected([FromQuery] RequestParameters requestParameters)
    {
        var cities = _cityManager.GetAllCitiesSelected(requestParameters);

        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(cities.MetaData);

        return Ok(cities);
    }

    [Obsolete($"Use endpoint {nameof(GetCitiesSelected)} instead.")]
    [HttpGet(Name = "GetCities")]
    public async Task<IActionResult> GetCities([FromQuery] RequestParameters requestParameters)
    {
        var (cities, metaData) =
            await _cityManager.GetAllCitiesAsync(requestParameters, trackChanges: false);

        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(metaData);

        return Ok(cities);
    }
}