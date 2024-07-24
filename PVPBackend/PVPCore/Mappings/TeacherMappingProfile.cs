using AutoMapper;
using PVPCore.Requests.Teacher;
using PVPCore.Response.Teacher;
using PVPDomain.Entities;

namespace PVPCore.Mappings;

public class TeacherMappingProfile : Profile
{
    public TeacherMappingProfile()
    {
        CreateMap<Teacher, TeacherResponse>();
        CreateMap<TeacherRegisterRequest, Teacher>();
        CreateMap<TeacherLoginRequest, Teacher>();
    }
}