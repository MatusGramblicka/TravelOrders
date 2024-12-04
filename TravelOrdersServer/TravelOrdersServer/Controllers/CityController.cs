using Contracts.RequestFeatures;
using Interface.Managers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TravelOrdersServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CityController(ICityManager cityManager) : ControllerBase
{
    [HttpGet("citiesSelected", Name = "GetCitiesSelected")]
    public IActionResult GetCitiesSelected([FromQuery] RequestParameters requestParameters)
    {
        var cities = cityManager.GetCitiesSelected(requestParameters);

        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(cities.MetaData);

        return Ok(cities);
    }
}