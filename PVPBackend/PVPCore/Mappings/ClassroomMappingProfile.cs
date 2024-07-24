using AutoMapper;
using PVPCore.Requests.Classroom;
using PVPCore.Response.Classroom;
using PVPDomain.Entities;

namespace PVPCore.Mappings;

public class ClassroomMappingProfile : Profile
{
    public ClassroomMappingProfile()
    {
        CreateMap<Classroom, ClassroomResponse>();
        CreateMap<ClassroomRequest, Classroom>();
    }
}