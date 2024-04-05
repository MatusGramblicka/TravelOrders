using Contracts.RequestFeatures;
using Interface.Managers;
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
        var employees = _employeeManager.GetAllEmployeesSelected(requestParameters);

        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(employees.MetaData);

        return Ok(employees);
    }
}