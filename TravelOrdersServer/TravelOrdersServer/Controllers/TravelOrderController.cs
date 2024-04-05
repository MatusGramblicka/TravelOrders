using Contracts.Dto;
using Contracts.Models;
using Contracts.RequestFeatures;
using Interface.Managers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TravelOrdersServer.ActionFilters;

namespace TravelOrdersServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TravelOrderController : ControllerBase
{
    private readonly ITravelOrderManager _travelOrderManager;

    public TravelOrderController(ITravelOrderManager travelOrderManager)
    {
        _travelOrderManager = travelOrderManager;
    }

    [HttpGet("travelOrdersSelected", Name = "GetTravelOrdersSelected")]
    public IActionResult GetTravelOrdersSelected([FromQuery] RequestParameters requestParameters)
    {
        var travelOrders = _travelOrderManager.GetAllTravelOrdersSelected(requestParameters);

        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(travelOrders.MetaData);

        return Ok(travelOrders);
    }

    [HttpGet("travelOrderSelected/{id}", Name = "TravelOrderSelectedById")]
    public async Task<IActionResult> GetTravelOrderSelected(int id)
    {
        var travelOrder = await _travelOrderManager.GetTravelOrderSelectedAsync(id);

        if (travelOrder == null)
            return NotFound();

        return Ok(travelOrder);
    }

    [HttpPost(Name = "CreateTravelOrder")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateTravelOrder([FromBody] TravelOrderCreationDto travelOrder)
    {
        TravelOrderDto travelOrderToReturn;

        try
        {
            travelOrderToReturn = await _travelOrderManager.CreateTravelOrderAsync(travelOrder);
        }
        catch (Exception e)
        {
            return BadRequest();
        }

        return CreatedAtRoute("TravelOrderById", new {id = travelOrderToReturn.Id}, travelOrderToReturn);
    }

    [HttpPut("{id}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ServiceFilter(typeof(ValidateTravelOrderExistsAttribute))]
    public async Task<IActionResult> UpdateTravelOrder(int id, [FromBody] TravelOrderUpdateDto travelOrder)
    {
        var travelOrderEntity = HttpContext.Items["travelOrder"] as TravelOrder;

        try
        {
            await _travelOrderManager.UpdateTravelOrder(travelOrder, travelOrderEntity);
        }
        catch (Exception e)
        {
            return BadRequest();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ServiceFilter(typeof(ValidateTravelOrderExistsAttribute))]
    public async Task<IActionResult> DeleteTravelOrder(int id)
    {
        var travelOrder = HttpContext.Items["travelOrder"] as TravelOrder;

        await _travelOrderManager.DeleteTravelOrder(travelOrder);

        return NoContent();
    }
}