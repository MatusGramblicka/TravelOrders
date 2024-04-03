using Entities.RequestFeatures;
using Interface;
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
        var citiesFromDb = _cityManager.GetAllCitiesSelected(requestParameters);

        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(citiesFromDb.MetaData);

        return Ok(citiesFromDb);
    }

    [Obsolete("Use endpoint GetCitiesSelected instead.")]
    [HttpGet(Name = "GetCities")]
    public async Task<IActionResult> GetCities([FromQuery] RequestParameters requestParameters)
    {
        var (citiesFromDb, metaData) =
            await _cityManager.GetAllCitiesAsync(requestParameters, trackChanges: false);

        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(metaData);

        return Ok(citiesFromDb);
    }
}