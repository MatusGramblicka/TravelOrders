using Contracts.RequestFeatures;
using Interface.Managers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TravelOrdersServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController(IEmployeeManager employeeManager) : ControllerBase
{
    [HttpGet("employeesSelected", Name = "GetEmployeesSelected")]
    public IActionResult GetEmployeesSelected([FromQuery] RequestParameters requestParameters)
    {
        var employees = employeeManager.GetEmployeesSelected(requestParameters);

        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(employees.MetaData);

        return Ok(employees);
    }
}