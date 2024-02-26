using AutoMapper;
using Contracts.Dto;
using Entities.RequestFeatures;
using Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TravelOrdersServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public EmployeeController(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet(Name = "GetEmployees")]
    public async Task<IActionResult> GetEmployees([FromQuery] RequestParameters requestParameters)
    {
        var employeesFromDb = await _repository.Employee.GetAllEmployeesAsync(requestParameters, trackChanges: false);

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(employeesFromDb.MetaData));

        var employeesDto = _mapper.Map<IEnumerable<CityDto>>(employeesFromDb);

        return Ok(employeesDto);
    }

    [HttpGet("employeesSelected", Name = "GetEmployeesSelected")]
    public IActionResult GetEmployeesSelected([FromQuery] RequestParameters requestParameters)
    {
        var employeesFromDb = _repository.Employee.GetAllEmployeesSelectedAsync(requestParameters);

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(employeesFromDb.MetaData));

        return Ok(employeesFromDb);
    }
}