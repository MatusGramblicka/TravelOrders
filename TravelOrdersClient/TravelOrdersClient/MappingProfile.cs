using AutoMapper;
using Contracts.Dto;

namespace TravelOrdersClient;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TravelOrderSelectedDto, TravelOrderUpdateDto>();
    }
}