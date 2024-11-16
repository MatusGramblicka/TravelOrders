using Contracts.Dto;
using Contracts.Exceptions;
using Contracts.Models;
using Contracts.RequestFeatures;
using Interface.Managers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TravelOrdersServer.ActionFilters;

namespace TravelOrdersServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TravelOrderController(ITravelOrderManager travelOrderManager) : ControllerBase
{
    [HttpGet("travelOrdersSelected", Name = "GetTravelOrdersSelected")]
    public IActionResult GetTravelOrdersSelected([FromQuery] RequestParameters requestParameters)
    {
        var travelOrders = travelOrderManager.GetAllTravelOrdersSelected(requestParameters);

        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(travelOrders.MetaData);

        return Ok(travelOrders);
    }

    [HttpGet("travelOrderSelected/{id}", Name = "TravelOrderSelectedById")]
    public async Task<IActionResult> GetTravelOrderSelected(int id)
    {
        var travelOrder = await travelOrderManager.GetTravelOrderSelectedAsync(id);

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
            travelOrderToReturn = await travelOrderManager.CreateTravelOrderAsync(travelOrder);
        }
        catch (CityMissingException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (EmployeeMissingException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (TrafficMissingException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return BadRequest("Unspecified problem");
        }

        return CreatedAtRoute("TravelOrderSelectedById", new {id = travelOrderToReturn.Id}, travelOrderToReturn);
    }

    [HttpPut("{id}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ServiceFilter(typeof(ValidateTravelOrderExistsAttribute))]
    public async Task<IActionResult> UpdateTravelOrder(int id, [FromBody] TravelOrderUpdateDto travelOrderDto)
    {
        var travelOrderFromDb = HttpContext.Items["travelOrder"] as TravelOrder;

        try
        {
#pragma warning disable CS8604 // Checked already for null in ValidateTravelOrderExistsAttribute filter
            await travelOrderManager.UpdateTravelOrder(travelOrderFromDb, travelOrderDto);
#pragma warning restore CS8604
        }
        catch (CityMissingException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (EmployeeMissingException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (TrafficMissingException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return BadRequest("Unspecified problem");
        }

        return NoContent();
    }

    [HttpPut("direct/{id}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ServiceFilter(typeof(ValidateTravelOrderExistsAttribute))]
    public async Task<IActionResult> UpdateTravelOrderOrderDirectMapping(int id, [FromBody] TravelOrderUpdateDto travelOrderDto)
    {
        var travelOrderFromDb = HttpContext.Items["travelOrder"] as TravelOrder;

        try
        {
#pragma warning disable CS8604 // Checked already for null in ValidateTravelOrderExistsAttribute filter
            await travelOrderManager.UpdateTravelOrderDirectMapping(travelOrderFromDb, travelOrderDto);
#pragma warning restore CS8604
        }
        catch (CityMissingException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (EmployeeMissingException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (TrafficMissingException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest("Unspecified problem");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ServiceFilter(typeof(ValidateTravelOrderExistsAttribute))]
    public async Task<IActionResult> DeleteTravelOrder(int id)
    {
        var travelOrderFromDb = HttpContext.Items["travelOrder"] as TravelOrder;

#pragma warning disable CS8604 // Checked already for null in ValidateTravelOrderExistsAttribute filter
        await travelOrderManager.DeleteTravelOrder(travelOrderFromDb);
#pragma warning restore CS8604

        return NoContent();
    }
}