using Entities.RequestFeatures;
using Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TravelOrdersServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeManager _employeeManager;

    public EmployeeController(IEmployeeManager employeeManager)
    {
        _employeeManager = employeeManager;
    }

    [HttpGet("employeesSelected", Name = "GetEmployeesSelected")]
    public IActionResult GetEmployeesSelected([FromQuery] RequestParameters requestParameters)
    {
        var employeesFromDb = _employeeManager.GetAllEmployeesSelected(requestParameters);

        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(employeesFromDb.MetaData);

        return Ok(employeesFromDb);
    }

    [Obsolete("Use endpoint GetEmployeesSelected instead.")]
    [HttpGet(Name = "GetEmployees")]
    public async Task<IActionResult> GetEmployees([FromQuery] RequestParameters requestParameters)
    {
        var (employeesFromDb, metaData) =
            await _employeeManager.GetAllEmployeesAsync(requestParameters, trackChanges: false);

        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(metaData);

        return Ok(employeesFromDb);
    }
}