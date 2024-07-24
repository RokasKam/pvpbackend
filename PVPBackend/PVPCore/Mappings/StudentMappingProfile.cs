using AutoMapper;
using PVPCore.Requests.Student;
using PVPCore.Response.Student;
using PVPDomain.Entities;

namespace PVPCore.Mappings;

public class StudentMappingProfile : Profile
{
    public StudentMappingProfile()
    {
        CreateMap<Student, StudentResponse>();
        CreateMap<StudentLoginRequest, Student>();
        CreateMap<StudentRegisterRequest, Student>();
    }
}