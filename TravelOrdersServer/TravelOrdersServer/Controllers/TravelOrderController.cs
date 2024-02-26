using AutoMapper;
using Contracts.Dto;
using Contracts.Models;
using Entities.RequestFeatures;
using Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TravelOrdersServer.ActionFilters;

namespace TravelOrdersServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TravelOrderController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public TravelOrderController(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet(Name = "GetTravelOrders")]
    public async Task<IActionResult> GetTravelOrders([FromQuery] RequestParameters requestParameters)
    {
        var travelOrdersFromDb =
            await _repository.TravelOrder.GetAllTravelOrdersAsync(requestParameters, trackChanges: false);

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(travelOrdersFromDb.MetaData));

        var travelOrdersDto = _mapper.Map<IEnumerable<TravelOrderDto>>(travelOrdersFromDb);

        return Ok(travelOrdersDto);
    }

    [HttpGet("travelOrdersSelected", Name = "GetTravelOrdersSelected")]
    public IActionResult GetTravelOrdersSelected([FromQuery] RequestParameters requestParameters)
    {
        var travelOrdersFromDb = _repository.TravelOrder.GetAllTravelOrdersSelectedAsync(requestParameters);

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(travelOrdersFromDb.MetaData));

        return Ok(travelOrdersFromDb);
    }

    [HttpGet("{id}", Name = "TravelOrderById")]
    public async Task<IActionResult> GetTravelOrder(int id)
    {
        var travelOrder = await _repository.TravelOrder.GetTravelOrderAsync(id, trackChanges: false);
        if (travelOrder == null)
        {
            return NotFound();
        }

        var travelOrderDto = _mapper.Map<TravelOrderDto>(travelOrder);
        return Ok(travelOrderDto);
    }

    [HttpGet("travelOrderSelected/{id}", Name = "TravelOrderSelectedById")]
    public IActionResult GetTravelOrderSelected(int id)
    {
        var travelOrder =  _repository.TravelOrder.GetTravelOrderSelectedAsync(id);
        if (travelOrder == null)
        {
            return NotFound();
        }

        return Ok(travelOrder);
    }

    [HttpPost(Name = "CreateTravelOrder")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateTravelOrder([FromBody] TravelOrderCreationDto travelOrder)
    {
        var startCity = await _repository.City.GetCityAsync(travelOrder.StartPlaceCityId, false);
        if (startCity == null)
            return BadRequest();

        var endCity = await _repository.City.GetCityAsync(travelOrder.EndPlaceCityId, false);
        if (endCity == null)
            return BadRequest();

        var employee = await _repository.Employee.GetEmployeeAsync(travelOrder.EmployeeId, false);
        if (employee == null)
            return BadRequest();

        var traffics = await _repository.Traffic.GetByIdsAsync(travelOrder.Traffics.Select(t => t.Id), true);
        if (traffics.Count() != travelOrder.Traffics.Count)
            return BadRequest();

        var travelOrderEntity = _mapper.Map<TravelOrder>(travelOrder);
        travelOrderEntity.Traffics = (ICollection<Traffic>) traffics;

        _repository.TravelOrder.CreateTravelOrder(travelOrderEntity);
        await _repository.SaveAsync();

        var travelOrderToReturn = _mapper.Map<TravelOrderDto>(travelOrderEntity);

        return CreatedAtRoute("TravelOrderById", new {id = travelOrderToReturn.Id}, travelOrderToReturn);
    }

    [HttpPut("{id}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ServiceFilter(typeof(ValidateTravelOrderExistsAttribute))]
    public async Task<IActionResult> UpdateTravelOrder(int id, [FromBody] TravelOrderUpdateDto travelOrder)
    {
        var travelOrderEntity = HttpContext.Items["travelOrder"] as TravelOrder;

        var startCity = await _repository.City.GetCityAsync(travelOrder.StartPlaceCityId, false);
        if (startCity == null)
            return BadRequest();

        var endCity = await _repository.City.GetCityAsync(travelOrder.EndPlaceCityId, false);
        if (endCity == null)
            return BadRequest();

        var employee = await _repository.Employee.GetEmployeeAsync(travelOrder.EmployeeId, false);
        if (employee == null)
            return BadRequest();

        var traffics = await _repository.Traffic.GetByIdsAsync(travelOrder.Traffics.Select(t => t.Id), true);
        if (traffics.Count() != travelOrder.Traffics.Count)
            return BadRequest();

        _mapper.Map(travelOrder, travelOrderEntity);
        travelOrderEntity.Traffics.Clear();
        travelOrderEntity.Traffics = (ICollection<Traffic>)traffics;

        await _repository.SaveAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ServiceFilter(typeof(ValidateTravelOrderExistsAttribute))]
    public async Task<IActionResult> DeleteTravelOrder(int id)
    {
        var travelOrder = HttpContext.Items["travelOrder"] as TravelOrder;

        _repository.TravelOrder.DeleteTravelOrder(travelOrder);
        await _repository.SaveAsync();

        return NoContent();
    }
}