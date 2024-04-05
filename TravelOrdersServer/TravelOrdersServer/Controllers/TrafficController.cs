using Contracts.RequestFeatures;
using Interface.Managers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TravelOrdersServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TrafficController : ControllerBase
{
    private readonly ITrafficManager _trafficManager;

    public TrafficController(ITrafficManager cityManager)
    {
        _trafficManager = cityManager;
    }

    [HttpGet("trafficsSelected", Name = "GetTrafficsSelected")]
    public IActionResult GetTrafficsSelected([FromQuery] RequestParameters requestParameters)
    {
        var traffics = _trafficManager.GetAllTrafficsSelected(requestParameters);

        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(traffics.MetaData);

        return Ok(traffics);
    }
}