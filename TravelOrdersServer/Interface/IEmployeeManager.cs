﻿using Contracts.Dto;
using Entities.RequestFeatures;

namespace Interface;

public interface IEmployeeManager
{
    PagedList<EmployeeSelectedDto> GetAllEmployeesSelected(RequestParameters requestParameters);

    [Obsolete($"Use method {nameof(GetAllEmployeesSelected)} instead.")]
    Task<(IEnumerable<EmployeeDto>, MetaData)> GetAllEmployeesAsync(RequestParameters requestParameters,
        bool trackChanges);
}