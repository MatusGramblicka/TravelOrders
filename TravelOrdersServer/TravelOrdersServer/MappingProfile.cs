﻿using AutoMapper;
using Contracts.Dto;
using Contracts.Models;

namespace TravelOrdersServer;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TravelOrder, TravelOrderDto>();
        CreateMap<TravelOrderCreationDto, TravelOrder>();
        CreateMap<TravelOrderUpdateDto, TravelOrder>().ReverseMap();

        CreateMap<City, CityDto>();

        CreateMap<Employee, EmployeeDto>();

        CreateMap<Traffic, TrafficDto>(); 
        CreateMap<TrafficSelectedDto, Traffic>().ReverseMap();
    }
}