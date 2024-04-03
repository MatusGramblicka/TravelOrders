﻿using Contracts.Dto;
using Contracts.Models;
using Entities.RequestFeatures;

namespace Interface;

public interface IEmployeeRepository
{
    PagedList<EmployeeSelectedDto> GetAllEmployeesSelected(RequestParameters requestParameters);

    [Obsolete($"Use method {nameof(GetAllEmployeesSelected)} instead.")]
    Task<PagedList<Employee>> GetAllEmployeesAsync(RequestParameters requestParameters, bool trackChanges);

    Task<Employee?> GetEmployeeAsync(string employeesId, bool trackChanges);

    Task<IEnumerable<Employee>> GetByIdsAsync(IEnumerable<string> ids, bool trackChanges);
}