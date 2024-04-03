using AutoMapper;
using Contracts.Dto;
using Entities.RequestFeatures;
using Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TravelOrdersServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TrafficController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public TrafficController(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("trafficsSelected", Name = "GetTrafficsSelected")]
    public IActionResult GetTrafficsSelected([FromQuery] RequestParameters requestParameters)
    {
        var trafficsFromDb = _repository.Traffic.GetAllTrafficsSelectedAsync(requestParameters);
        
        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(trafficsFromDb.MetaData);

        return Ok(trafficsFromDb);
    }

    [Obsolete("Use endpoint GetTrafficsSelected instead.")]
    [HttpGet(Name = "GetTraffics")]
    public async Task<IActionResult> GetTraffics([FromQuery] RequestParameters requestParameters)
    {
        var trafficsFromDb = await _repository.Traffic.GetAllTrafficsAsync(requestParameters, trackChanges: false);

        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(trafficsFromDb.MetaData);

        var trafficsDto = _mapper.Map<IEnumerable<TrafficDto>>(trafficsFromDb);

        return Ok(trafficsDto);
    }
}