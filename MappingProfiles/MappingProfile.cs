using AutoMapper;
using AcquisitionAPI.DTOs;
using AcquisitionAPI.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AcquisitionDto, Acquisition>();
        CreateMap<Acquisition, AcquisitionDto>();
    }
}
