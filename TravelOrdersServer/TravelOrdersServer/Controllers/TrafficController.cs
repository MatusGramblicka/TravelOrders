using Contracts.RequestFeatures;
using Interface.Managers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TravelOrdersServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TrafficController(ITrafficManager cityManager) : ControllerBase
{
    [HttpGet("trafficsSelected", Name = "GetTrafficsSelected")]
    public IActionResult GetTrafficsSelected([FromQuery] RequestParameters requestParameters)
    {
        var traffics = cityManager.GetTrafficsSelected(requestParameters);

        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(traffics.MetaData);

        return Ok(traffics);
    }
}